// State.cs
using UnityEngine;

namespace MyStateMachine
{
    /// <summary>
    /// Base class for all states. Attach these as components.
    /// </summary>
    public abstract class State : MonoBehaviour
    {
        private IStateMachine _stateMachine;

        internal virtual void Initialize(IStateMachine owner)
        {
            _stateMachine = owner;
        }

        /// <summary>
        /// Non-generic access to the owning state machine.
        /// </summary>
        protected IStateMachine ParentStateMachine => _stateMachine;

        /// <summary>
        /// Generic access to the owning state machine.
        /// Example: var fsm = GetParentStateMachine<CharacterStateMachine>();
        /// </summary>
        public TMachine GetParentStateMachine<TMachine>() where TMachine : class, IStateMachine
        {
            return _stateMachine as TMachine;
        }

        public virtual void Enter() { }

        /// <summary>
        /// Called from Unity Update().
        /// </summary>
        public virtual void UpdateTick() { }

        /// <summary>
        /// Called from Unity FixedUpdate().
        /// </summary>
        public virtual void FixedUpdateTick() { }

        public virtual void Exit() { }
    }
}
