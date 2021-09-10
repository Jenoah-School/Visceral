using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string checkString = "";
    [Header("Events")]
    [SerializeField] private UnityEvent TriggerEnter;
    [SerializeField] private UnityEvent TriggerStay;
    [SerializeField] private UnityEvent TriggerExit;

    void OnTriggerEnter(Collider other)
    {
        if(string.IsNullOrEmpty(checkString) || other.transform.CompareTag(checkString))
            TriggerEnter.Invoke();
    }

    void OnTriggerStay(Collider other)
    {
        if (string.IsNullOrEmpty(checkString) || other.transform.CompareTag(checkString))
            TriggerStay.Invoke();
    }

    void OnTriggerExit(Collider other)
    {
        if (string.IsNullOrEmpty(checkString) || other.transform.CompareTag(checkString))
            TriggerExit.Invoke();
    }
}
