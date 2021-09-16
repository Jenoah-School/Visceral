using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnLoad : MonoBehaviour
{
    [SerializeField] private string objectToDestroyName = "";

    void Start()
    {
        GameObject objectToDestroy = GameObject.Find(objectToDestroyName);
        if (objectToDestroy == null) objectToDestroy = GameObject.Find(objectToDestroyName + " (Clone)");
        if (objectToDestroy != null) Destroy(objectToDestroy);
    }
}
