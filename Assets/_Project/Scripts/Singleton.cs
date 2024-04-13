using UnityEngine;

/// <summary>
/// Ensures only one instance of a given MonoBehavior is present.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T>: MonoBehaviour where T : Singleton<T>
{
    private static T _instance;
    
    public static T Instance
    {
        get
        {
            if (_instance != null) return _instance;
            
            _instance = FindObjectOfType<T>(true);
            if (_instance != null) return _instance;
            Debug.LogWarning($"An instance of {typeof(T)} is needed but there is none. Attempting to create one!");
            _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
            return _instance;
        }
    }

    public static bool Exists => _instance != null;

    protected void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this)
        {
            Debug.LogWarning($"An instance of {typeof(T)} already exists. Destroying this extra!", gameObject);
            Destroy(this);
        }
    }
}