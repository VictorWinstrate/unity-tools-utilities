using System.Text;
using UnityEngine;

public static class TransformExtensions
{
	public static Transform FindChildRecursively(this Transform parent, string name)
	{
		var child = parent.Find(name);
		if (child != null)
			return child;

		foreach (Transform t in parent)
		{
			child = FindChildRecursively(t, name);
			if (child != null)
				return child;
		}

		return null;
	}

	/// <summary>
	/// Resets local transform to (0, 0, 0) in position
	/// and transform and (1, 1, 1) in scale.
	/// </summary>
	public static void ResetLocalTransform(this Transform transform)
	{
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
	}

	public static void ResetWorldTransform(this Transform transform)
	{
		transform.SetPositionAndRotation(
			Vector3.zero,
			Quaternion.identity);
		transform.localScale = new Vector3(
			1f / transform.lossyScale.x,
			1f / transform.lossyScale.y,
			1f / transform.lossyScale.z);
	}

	public static void MatchTransform(this Transform transform, Transform transformToMirror)
	{
		transform.SetPositionAndRotation(
			transformToMirror.position,
			transformToMirror.rotation);
		transform.localScale = transformToMirror.localScale;
	}

	public static void DestroyChildren(this Transform transform)
	{
		for (int i = transform.childCount - 1; i >= 0; i--)
			transform.GetChild(i).gameObject.TryToDestroy();
	}

	public static string GetFullPath(this Transform transform, char separator = '/')
	{
		if (transform == null)
			return string.Empty;

		if (transform.parent == null)
			return transform.name;

		var stringBuilder = new StringBuilder()
			.Append(transform.parent.GetFullPath())
			.Append(separator)
			.Append(transform.name);
		return stringBuilder.ToString();
	}
}