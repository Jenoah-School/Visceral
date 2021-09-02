using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMoveForward : MonoBehaviour
{
    [SerializeField] private Vector3 localMoveDirection = Vector3.forward;
    [SerializeField] private float moveSpeed = 4f;

    // Start is called before the first frame update
    void Start()
    {
        localMoveDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.TransformDirection(localMoveDirection) * moveSpeed * Time.deltaTime;
    }
}
