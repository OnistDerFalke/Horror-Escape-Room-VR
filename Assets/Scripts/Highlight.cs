using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [SerializeField]
    private List<Renderer> renderers;

    private List<Material> materials;
    private Color color;
    private bool doHightLight = true;
    private List<Color> primalColors;
    
    [SerializeField] private bool alternativeColor;
    
    private void Awake()
    {
        primalColors = new List<Color>();
        materials = new List<Material>();
        foreach (var renderer in renderers)
            materials.AddRange(new List<Material>(renderer.materials));
        foreach (var t in materials)
        {
            primalColors.Add(alternativeColor ? t.GetColor("_Color") : t.GetColor("_EmissionColor"));
        }
    }

    private void Update()
    {
        if (!doHightLight)
        {
            for (var i = 0; i < materials.Count; i++)
            {
                if (alternativeColor)
                {
                    materials[i].SetColor("_Color", primalColors[i]);
                }
                else
                {
                    materials[i].EnableKeyword("_EMISSION");
                    materials[i].SetColor("_EmissionColor", primalColors[i]);
                }
            }
            return;
        }

        if (alternativeColor)
        {
            var sinVal = Mathf.Sin(4f * Time.time);
            if (sinVal > 0.8f) sinVal = 0.8f;
            if (sinVal < 0.2f) sinVal = 0.2f;
            color = new Color(sinVal, sinVal, sinVal);
        }
        else
        {
            var sinVal = Mathf.Sin(2f * Time.time);
            if (sinVal > 0.8f) sinVal = 0.8f;
            if (sinVal < 0.2f) sinVal = 0.2f;
            color = new Color(sinVal, sinVal, sinVal);
        }
        ChangeHighlight();
    }

    private void ChangeHighlight()
    {
        foreach (var material in materials)
        {
            if (alternativeColor)
            {
                material.SetColor("_Color", color);
            }
            else
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", color);
            }
        }
    }

    public void SetHighlight(bool doSet)
    {
        doHightLight = doSet;
    }
}
