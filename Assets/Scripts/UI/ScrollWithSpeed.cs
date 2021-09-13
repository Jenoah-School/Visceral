using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollWithSpeed : MonoBehaviour
{
    [SerializeField] private Vector2 scrollSpeed = new Vector2(0f, 5f);

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition += scrollSpeed * Time.deltaTime;
    }
}
