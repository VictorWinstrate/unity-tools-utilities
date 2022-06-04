using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrefabPool
{
	class Pool
	{
		public int InstanceLimit;

		public List<GameObject> ActiveInstances = new List<GameObject>();
		public List<GameObject> InactiveInstances = new List<GameObject>();

		public Dictionary<GameObject, DateTime> ActiveInstanceToAgeMap = new Dictionary<GameObject, DateTime>();

		public Pool(int instanceLimit) => InstanceLimit = instanceLimit;

		public GameObject GetOldestActiveInstance()
		{
			GameObject oldestInstance = null;
			DateTime? oldestInstanceAge = null;

			foreach (var activeInstance in ActiveInstances)
			{
				if (!oldestInstanceAge.HasValue || ActiveInstanceToAgeMap[activeInstance] < oldestInstanceAge.Value)
				{
					oldestInstance = activeInstance;
					oldestInstanceAge = ActiveInstanceToAgeMap[activeInstance];
				}
			}

			return oldestInstance;
		}

		const int _defaultInstanceLimit = 1;

		static readonly Dictionary<GameObject, Pool> _prefabToPoolMap = new Dictionary<GameObject, Pool>();
		static readonly Dictionary<GameObject, Pool> _instanceToPoolMap = new Dictionary<GameObject, Pool>();

		public static void SetInstanceLimit<T>(T prefab, int instanceLimit) where T : Component =>
			SetInstanceLimit(prefab.gameObject, instanceLimit);

		public static void SetInstanceLimit(GameObject prefab, int instanceLimit)
		{
			if (_prefabToPoolMap.ContainsKey(prefab))
				_prefabToPoolMap[prefab].InstanceLimit = Mathf.Max(_prefabToPoolMap[prefab].InstanceLimit, instanceLimit);
			else
				_prefabToPoolMap.Add(prefab, new Pool(instanceLimit));
		}

		public static T Spawn<T>(T prefab, Transform parent, bool resetTransform = false) where T : Component
		{
			var instanceObject = Spawn(prefab.gameObject, parent, resetTransform);
			return instanceObject.GetComponent<T>();
		}

		public static GameObject Spawn(GameObject prefab, Transform parent, bool resetTransform = false)
		{
			if (!_prefabToPoolMap.ContainsKey(prefab))
				_prefabToPoolMap.Add(prefab, new Pool(_defaultInstanceLimit));

			var pool = _prefabToPoolMap[prefab];
			GameObject instance;

			CheckForDestroyedInstances(prefab, pool);

			if (pool.InactiveInstances.Count > 0)
			{
				//  Use available instance.
				instance = pool.ActiveInstances[0];
				pool.InactiveInstances.RemoveAt(0);
			}
			else if (pool.InstanceLimit > 0 && pool.ActiveInstances.Count >= pool.InstanceLimit)
			{
				//  Pool is at size limit! Reuse oldest active instance.
				instance = pool.GetOldestActiveInstance();
				instance.SetActive(false);

				pool.ActiveInstances.Remove(instance);
				pool.ActiveInstanceToAgeMap.Remove(instance);
			}
			else
			{
				//  Create new instance.
				instance = UnityEngine.Object.Instantiate(prefab);
				_instanceToPoolMap.Add(instance, pool);

				int number = pool.ActiveInstances.Count + pool.InactiveInstances.Count + 1;
				instance.name = $"{prefab.name}_{number:N3}";
			}

			//  Track spawned instance.
			pool.ActiveInstances.Add(instance);
			pool.ActiveInstanceToAgeMap.Add(instance, DateTime.Now);

			//  Activate and set transform details.
			instance.transform.SetParent(parent);
			instance.SetActive(true);

			if (resetTransform)
				instance.transform.ResetLocalTransform();

			return instance;
		}

		public static void Recycle<T>(T instance) where T : Component =>
			Recycle(instance.gameObject);

		public static void Recycle(GameObject instance)
		{
			if (!_instanceToPoolMap.ContainsKey(instance))
			{
				Debug.LogErrorFormat("Tried to recycle an untracked prefab instance ({0})!", instance.name);
				return;
			}

			//  Deactivate instance.
			instance.SetActive(false);

			//  Remove it from pool's ActiveInstance collection.
			var pool = _instanceToPoolMap[instance];

			//  Ignore failure to remove in cases when instance exists at start and gets recycled later, or is recycled twice.
			pool.ActiveInstances.Remove(instance);
			pool.ActiveInstanceToAgeMap.Remove(instance);

			//  Add it to pool's InactiveInstance collection.
			if (!pool.InactiveInstances.Contains(instance))
				pool.InactiveInstances.Add(instance);
		}

		static void CheckForDestroyedInstances(GameObject prefab, Pool pool)
		{
			//  Only do expensive Destroy method if a destroyed instance is found.
			foreach (var instance in pool.ActiveInstances)
			{
				if (instance == null)
				{
					ClearDestroyedInstances(prefab, pool);
					return;
				}
			}

			foreach (var instance in pool.InactiveInstances)
			{
				if (instance == null)
				{
					ClearDestroyedInstances(prefab, pool);
					return;
				}
			}
		}

		static void ClearDestroyedInstances(GameObject prefab, Pool pool)
		{
			int removedInstanceCount = pool.ActiveInstances.RemoveAll(i => i == null) + pool.InactiveInstances.RemoveAll(i => i == null);

			Debug.LogWarningFormat(prefab, "Found {0} destroyed instance(s) of {1} prefab in pool! Pooled instances should not be destroyed!", removedInstanceCount, prefab.name);

			foreach (var instance in pool.ActiveInstanceToAgeMap.Where(i => i.Key == null).ToList())
				pool.ActiveInstanceToAgeMap.Remove(instance.Key);

			foreach (var instance in _instanceToPoolMap.Where(i => i.Key == null).ToList())
				_instanceToPoolMap.Remove(instance.Key);
		}
	}
}