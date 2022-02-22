using System;
using System.Collections;
using UnityEngine;

class TaskManager : MonoBehaviour
{
	public class TaskState
	{
		public bool IsRunning { get; private set; }
		public bool IsPaused { get; private set; }

		public Action<bool> OnTaskFinished;

		bool _hasStopped;

		readonly IEnumerator _coroutine;

		public TaskState(IEnumerator coroutine) =>
			_coroutine = coroutine;

		public void Start()
		{
			IsRunning = true;
			_singleton.StartCoroutine(CallWrapper());
		}

		public void Pause() =>
			IsPaused = true;

		public void Unpause() =>
			IsPaused = false;

		public void Stop()
		{
			_hasStopped = true;
			IsRunning = false;
		}

		IEnumerator CallWrapper()
		{
			var coroutine = _coroutine;

			while (IsRunning)
			{
				if (IsPaused)
					yield return null;
				else
				{
					if (coroutine != null && coroutine.MoveNext())
						yield return coroutine.Current;
					else
						IsRunning = false;
				}
			}

			OnTaskFinished?.Invoke(_hasStopped);
		}
	}

	static TaskManager _singleton;

	void Awake()
	{
		if (_singleton == null)
			_singleton = this;

		DontDestroyOnLoad(this);
	}

	public static TaskState CreateTask(IEnumerator coroutine)
	{
		if (_singleton == null)
			_singleton = new GameObject("TaskManager").AddComponent<TaskManager>();

		return new TaskState(coroutine);
	}
}