using UnityEngine;

public static class GameObjectExtensions
{
    public static void SetLayerRecursively(this GameObject gameObject, LayerMask layerMask)
    {
        gameObject.layer = layerMask;

        foreach (Transform child in gameObject.transform)
            child.gameObject.SetLayerRecursively(layerMask);
    }

    public static void TryToDestroy(this GameObject gameObject)
    {
        if (gameObject == null)
            return;

        if (Application.isPlaying)
            Object.Destroy(gameObject);
        else
            Object.DestroyImmediate(gameObject);
    }

    public static string GetFullPath(this GameObject gameObject) =>
        gameObject.transform.GetFullPath();
}