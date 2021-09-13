using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnKeypress : MonoBehaviour
{
    [SerializeField] private List<KeyCode> keys = new List<KeyCode>();
    [SerializeField] private UnityEvent OnKeyPress;

    // Update is called once per frame
    void Update()
    {
        foreach(KeyCode key in keys)
        {
            if (Input.GetKeyDown(key))
            {
                OnKeyPress.Invoke();
            }
        }
    }
}
