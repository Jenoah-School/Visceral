using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverTransform : MonoBehaviour
{
    [SerializeField] private Vector3 moveAxis = Vector3.up;
    [SerializeField] private float hoverSpeed = 2f;
    [SerializeField] private float hoverHeight = 5f;

    private Vector3 startPosition = Vector3.zero;
    private float lifeTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime += Time.deltaTime * hoverSpeed;
        transform.localPosition = startPosition + moveAxis * Mathf.Sin(lifeTime) * hoverHeight;
    }
}
