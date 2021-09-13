using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventAfterTime : MonoBehaviour
{
    [SerializeField] private float waitTime = 5f;
    [SerializeField] private bool autoStart = false;
    [SerializeField] private UnityEvent timedEvent;

    private void Start()
    {
        if (autoStart) StartTimedEvent();
    }

    public void StartTimedEvent()
    {
        CancelInvoke();
        Invoke("StartEventImmediate", waitTime);
    }

    public void StartEventImmediate()
    {
        CancelInvoke();
        timedEvent.Invoke();
    }
}
