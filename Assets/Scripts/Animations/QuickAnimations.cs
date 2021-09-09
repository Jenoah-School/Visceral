using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Lean.Pool;
using UnityEngine.UI;

public class QuickAnimations : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MeshRenderer[] meshRenderers;
    [SerializeField] private Collider[] colliders;
    [SerializeField] private GameObject rootObject = null;

    [Header("Toggles")]
    [SerializeField] private bool disableCollisionOnGrow = true;
    [SerializeField] private bool destroyAfterFade = true;

    [Header("Targets")]
    [SerializeField] private float growTarget = 2.5f;
    [SerializeField] private float squishTarget = 0.1f;
    [SerializeField] private Color colorTarget = Color.white;

    private List<Material> materials = new List<Material>();
    private Image image = null;
    private Color startColor;

    private void Start()
    {
        if (meshRenderers.Length > 0)
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                for (int j = 0; j < meshRenderers[i].materials.Length; j++)
                {
                    materials.Add(meshRenderers[i].materials[j]);
                }
            }
        }
        if (gameObject.TryGetComponent(out image)) startColor = image.color;
    }

    public void Squish(float speed)
    {
        transform.DOPunchScale(-Vector3.one * squishTarget, speed, 10, 0.25f);
    }

    public void SetImageColor(float speed)
    {
        if (image != null)
        {
            image.DOColor(colorTarget, speed);
        }
    }

    public void ResetImageColor(float speed)
    {
        if (image != null)
        {
            image.DOColor(startColor, speed);
        }
    }

    public void Grow(float speed)
    {
        transform.DOScale(growTarget, speed);
        if (disableCollisionOnGrow)
        {
            foreach(Collider collider in colliders)
            {
                collider.enabled = false;
            }
        }
    }

    private void OnDisable()
    {
        ResetAlphaImmediate();
    }

    private void OnDestroy()
    {
        ResetAlphaImmediate();
    }

    public void ResetAlphaImmediate()
    {
        foreach (Material material in materials)
        {
            Color tempColor = material.color;
            tempColor.a = 1f;
            material.color = tempColor;
        }
    }

    public void FadeOut(float speed)
    {
        foreach(Material material in materials)
        {
            material.DOFade(0f, speed);
        }

        if (destroyAfterFade)
        {
            if (LeanPool.Links.ContainsKey(gameObject))
            {
            LeanPool.Despawn(gameObject, speed + 0.1f);
            }
            else
            {
                Destroy(gameObject, speed + 0.1f);
            }
        }
    }
}
