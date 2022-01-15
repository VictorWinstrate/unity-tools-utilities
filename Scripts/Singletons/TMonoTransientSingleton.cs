using UnityEngine;

/// <summary>
/// An impermanent Singleton that inherits from Mono. It may only exist in a certain scene, but there should only ever be one instance.
/// Consumers should check for null before using Instance as the Instance property doesn't create an instance of itself.
/// </summary>
public abstract class TMonoTransientSingleton<T> : MonoBehaviour where T : TMonoTransientSingleton<T>
{
    public static T Instance { get; private set; }

    const string _exceptionMessageFormat =
        "Error! More than one instance of a {0} transient singleton exists";

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            throw new System.Exception(
                string.Format(_exceptionMessageFormat, typeof(T)));
        }

        Instance = (T) this;
    }

    protected virtual void OnDestroy() => Instance = null;
}
