using UnityEngine;

/// <summary>
/// Generic Singleton pattern for MonoBehaviour classes.
/// Usage: public class MyClass : Singleton<MyClass> { }
/// </summary>
/// <typeparam name="T">The type of the singleton class</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Optionally override Awake to ensure only one instance exists
    /// </summary>
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Debug.LogWarning($"[Singleton] Duplicate instance of {typeof(T)} found. Destroying duplicate.");
            Destroy(gameObject);
        }
    }
}