/// <summary>
/// Generic non-monobehaviour Singleton, instantiated the first time the Instance property is accessed.
/// </summary>
public class TSingleton<T> where T : new()
{
    static T _instance;
    static readonly object _lock = new object();

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new T();
                }
            }

            return _instance;
        }
    }

    protected TSingleton() { }
}