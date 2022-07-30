using UnityEngine;

/// <summary>
/// Singleton inherits from Mono 
/// </summary>
public abstract class TMonoSingleton<T> : MonoBehaviour where T : Component
{
	public static bool IsExisting => _instance != null;

	static T _instance;

	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<T>();

				if (_instance == null)
				{
					var obj = new GameObject(typeof(T).Name);
					_instance = obj.AddComponent<T>();
				}
			}

			return _instance;
		}
	}

	protected virtual void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this);
			return;
		}

		_instance = this as T;
	}
}