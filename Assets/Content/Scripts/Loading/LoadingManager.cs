using UnityEngine;
using System.Threading;
using System;

public class LoadingManager
{
    private static LoadingManager _instance;

    public static LoadingManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LoadingManager();
                _instance.cancelTokenSource = new CancellationTokenSource();
            }
            return _instance;
        }
    }
    public CancellationTokenSource cancelTokenSource;

}
