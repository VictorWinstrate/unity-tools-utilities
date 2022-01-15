using UnityEngine;

public static class ComponentExtensions
{
    public static bool TryToGetComponent<T>(GameObject gameObject, out T instance) where T : Component
    {
        instance = gameObject.GetComponent<T>();
        return instance != null;
    }

    public static bool TryToGetComponent<T>(Component component, out T instance) where T : Component
    {
        instance = component.GetComponent<T>();
        return instance != null;
    }
}