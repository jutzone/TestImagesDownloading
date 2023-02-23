using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class TaskManager : MonoBehaviour
{
    CancellationTokenSource _cts;

    SynchronizationContext context = SynchronizationContext.Current;
    // private static void DumbTask(int subTaskCount, int sleepTime, SynchronizationContext context, CancellationToken token)
    // {
    //     _cts.Token
    //     Debug.Log($"Task started at {Thread.CurrentThread.ManagedThreadId}");
    //     for (int i = 0; i < subTaskCount; i++)
    //     {
    //         // Cancellation
    //         token.ThrowIfCancellationRequested();

    //         // Work
    //         Thread.Sleep(sleepTime);

    //         // Notification
    //         context.Post(_ => GetCustomWindow(false).Progress = (float)i / subTaskCount, null);
    //     }
    //     Debug.Log($"Task done at {Thread.CurrentThread.ManagedThreadId}");
    //     context.Post(_ => GetCustomWindow(true).OnTaskFinishedOrCanceled(), null);
    // }

    // private void OnTaskFinishedOrCanceled()
    // {
    //     _taskRunning = false;
    //     _cts.Dispose();
    //     _cts = null;
    // }
}
