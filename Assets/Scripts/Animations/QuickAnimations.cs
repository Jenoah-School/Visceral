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
    [SerializeField] private float shrinkTarget = 0.5f;
    [SerializeField] private float squishTarget = 0.1f;
    [SerializeField] private float shakeStrength = .5f;
    [SerializeField] private Color colorTarget = Color.white;

    private List<Material> materials = new List<Material>();
    private List<Color> defaultMaterialColor = new List<Color>();
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
    public void SetMaterialColor(float speed)
    {
        foreach (Material material in materials)
        {
            material.DOColor(colorTarget, speed);
        }
    }

    public void SetMaterialEmission(float speed)
    {
        foreach (Material material in materials)
        {
            StartCoroutine(LerpMaterialEmission(material, colorTarget, speed));
        }
    }

    public void ResetMaterialColor(float speed)
    {
        for (int i = 0; i < materials.Count; i++)
        {
            materials[i].DOColor(defaultMaterialColor[i], speed);
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

    public void Shrink(float speed)
    {
        transform.DOScale(shrinkTarget, speed);
        if (disableCollisionOnGrow)
        {
            foreach (Collider collider in colliders)
            {
                collider.enabled = false;
            }
        }
    }

    public void Shake(float duration)
    {
        transform.DOShakePosition(duration, shakeStrength);
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
            if (material.HasProperty("_Color"))
            {
                Color tempColor = material.color;
                tempColor.a = 1f;
                material.color = tempColor;
            }
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
            LeanPool.Despawn(rootObject, speed + 0.1f);
            }
            else
            {
                Destroy(rootObject, speed + 0.1f);
            }
        }
    }

    IEnumerator LerpMaterialEmission(Material materialToFade, Color targetColor, float speed)
    {
        Color currentColor = materialToFade.GetColor("_EmissionColor");
        float elapsedTime = 0f;

        while (elapsedTime < speed)
        {
            elapsedTime += Time.deltaTime;
            materialToFade.SetColor("_EmissionColor", Color.Lerp(currentColor, targetColor, elapsedTime / speed));
            yield return new WaitForEndOfFrame();
        }

        materialToFade.SetColor("_EmissionColor", targetColor);
    }
}
