using System;
using System.Collections.Generic;
using UnityEngine;

public class JobManager : MonoBehaviour
{
    private Queue<Action> _plainQueue = new Queue<Action>();
    
    public void AddAction(Action action)
    {
        _plainQueue.Enqueue(action);
    }

    private void Update()
    {
        if (_plainQueue.Count <= 0) return;
        foreach (var action in _plainQueue)
        {
            action();
        }
        _plainQueue.Clear();
    }
}