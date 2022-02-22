using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Task
{
	public bool IsRunning => _task.IsRunning;
	public bool IsPaused => _task.IsPaused;

	/// <summary>
	/// Termination event, invoked when the coroutine completes its execution.
	/// The bool paramater is true if the coroutine was stopped
	/// when Stop() method is explicitly called.
	/// </summary>
	public event Action<bool> OnTaskFinished;

	readonly TaskManager.TaskState _task;

	public Task(
		IEnumerator coroutine,
		bool canAutoStart = true,
		Action<bool> taskFinishedCallback = null)
	{
		if (taskFinishedCallback != null)
			OnTaskFinished += taskFinishedCallback;

		_task = TaskManager.CreateTask(coroutine);
		_task.OnTaskFinished += HandleTaskFinished;

		if (canAutoStart)
			Start();
	}

	public void Start() =>
		_task.Start();

	public void Pause() =>
		_task.Pause();

	public void Unpause() =>
		_task.Unpause();

	public void Stop() =>
		_task.Stop();

	void HandleTaskFinished(bool isManuallyFinished) =>
		OnTaskFinished?.Invoke(isManuallyFinished);
}