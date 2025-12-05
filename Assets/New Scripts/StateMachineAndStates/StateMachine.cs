// StateMachine.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyStateMachine
{
    /// <summary>
    /// Generic Unity state machine.
    /// 
    /// Usage:
    /// public class CharacterStateMachine : StateMachine<CharacterStateMachine> { ... }
    /// 
    /// - Drag & drop State components into 'states' list in inspector.
    /// - Optionally assign an initial state.
    /// - Change state via ChangeState<SomeState>();
    /// </summary>
    public abstract class StateMachine<TMachine> : MonoBehaviour, IStateMachine
        where TMachine : StateMachine<TMachine>
    {
        [Header("States (drag & drop here)")]
        [SerializeField] private List<State> states = new List<State>();

        [Header("Optional initial state (drag one of the above)")]
        [SerializeField] private State initialState;

        private readonly Dictionary<Type, State> _stateLookup = new Dictionary<Type, State>();
        private State _currentState;
        private Coroutine _transitionRoutine;

        public State CurrentState => _currentState;

        protected virtual void Awake()
        {
            BuildStateLookup();
        }

        protected virtual void Start()
        {
            if (initialState != null)
            {
                ChangeState(initialState.GetType());
            }
        }

        private void BuildStateLookup()
        {
            _stateLookup.Clear();

            foreach (var s in states)
            {
                if (s == null) continue;

                var type = s.GetType();

                if (_stateLookup.ContainsKey(type))
                {
                    Debug.LogWarning($"StateMachine {name}: Duplicate state type {type.Name}.");
                    continue;
                }

                s.Initialize(this);
                _stateLookup.Add(type, s);
            }
        }

        // -------- IStateMachine implementation --------

        public void AddState(State state)
        {
            if (state == null)
            {
                Debug.LogError("Trying to add a null state.");
                return;
            }

            var type = state.GetType();
            if (_stateLookup.ContainsKey(type))
            {
                Debug.LogWarning($"StateMachine {name}: State of type {type.Name} already exists. Skipping.");
                return;
            }

            state.Initialize(this);
            states.Add(state);
            _stateLookup.Add(type, state);
        }

        public TState AddState<TState>() where TState : State
        {
            var type = typeof(TState);

            if (_stateLookup.ContainsKey(type))
                return (TState)_stateLookup[type];

            // Try to find an existing component
            TState state = GetComponent<TState>();
            if (state == null)
            {
                state = gameObject.AddComponent<TState>();
            }

            AddState(state);
            return state;
        }

        public void ChangeState<TState>() where TState : State
        {
            ChangeState(typeof(TState));
        }

        public void ChangeState(Type stateType)
        {
            if (stateType == null || !typeof(State).IsAssignableFrom(stateType))
            {
                Debug.LogError($"StateMachine {name}: Invalid state type passed to ChangeState.");
                return;
            }

            if (!_stateLookup.TryGetValue(stateType, out var newState))
            {
                Debug.LogError($"StateMachine {name}: State of type {stateType.Name} not found. Did you register it or drag it to the list?");
                return;
            }

            if (newState == _currentState)
                return;

            if (_transitionRoutine != null)
                StopCoroutine(_transitionRoutine);

            _transitionRoutine = StartCoroutine(TransitionRoutine(newState));
        }

        private IEnumerator TransitionRoutine(State newState)
        {
            // EXIT current
            if (_currentState != null)
            {
                if (_currentState is IAsyncState asyncExit)
                    yield return asyncExit.ExitAsync();
                else
                    _currentState.Exit();
            }

            _currentState = newState;

            // ENTER new
            if (_currentState != null)
            {
                if (_currentState is IAsyncState asyncEnter)
                    yield return asyncEnter.EnterAsync();
                else
                    _currentState.Enter();
            }

            _transitionRoutine = null;
        }

        // -------- Unity forwards --------

        protected virtual void Update()
        {
            _currentState?.UpdateTick();
        }

        protected virtual void FixedUpdate()
        {
            _currentState?.FixedUpdateTick();
        }
    }
}
