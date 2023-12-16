using System;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [SerializeField]
    private List<Renderer> renderers;

    private List<Material> materials;
    private Color color;
    private bool doHightLight = true;
    private void Awake()
    {
        materials = new List<Material>();
        foreach (var renderer in renderers)
            materials.AddRange(new List<Material>(renderer.materials));
    }

    private void Update()
    {
        if (!doHightLight) return;
        var sinVal = Mathf.Sin(2f*Time.time);
        if (sinVal > 0.8f) sinVal = 0.8f;
        if (sinVal < 0.2f) sinVal = 0.2f;
        color = new Color(sinVal, sinVal, sinVal);
        ChangeHighlight();
    }

    private void ChangeHighlight()
    {
        foreach (var material in materials)
        {
            material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", color);
        }
    }

    public void SetHighlight(bool doSet)
    {
        doHightLight = doSet;
    }
}
