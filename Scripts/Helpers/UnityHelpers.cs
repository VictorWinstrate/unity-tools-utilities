using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class UnityHelpers
{
	public static List<T> GetAllComponentsInScene<T>(bool includeInactive)
	{
		var allItems = new List<T>();

		// Iterate over all root objects so we can use
		// GetComponentsInChildren and include inactive objects.
		var activeScene = SceneManager.GetActiveScene();
		var rootObjects = activeScene.GetRootGameObjects();

		foreach (var rootObject in rootObjects)
		{
			var childItems = rootObject.GetComponentsInChildren<T>(includeInactive);
			allItems.AddRange(childItems);
		}

		return allItems;
	}
}