using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightBehavior : MonoBehaviour
{
    //we assign all the renderers here through the inspector

    [SerializeField] private List<Renderer> renderers;
    private List<Material> materials;

    [Header("Color Settings")]
    [SerializeField] private Color color;
    [SerializeField][Range(0,1)] private float _colorStrength;

   

    //Gets all the materials from each renderer
    private void Awake()
    {
        materials = new List<Material>();
        foreach (var renderer in renderers)
        {
            //A single child-object might have mutliple materials on it
            //that is why we need to all materials with "s"
            materials.AddRange(new List<Material>(renderer.materials));
        }
    }

    public void ToggleHighlight(bool val)
    {
        if (val)
        {
            foreach (var material in materials)
            {
                //We need to enable the EMISSION
                material.EnableKeyword("_EMISSION");
                //before we can set the color
                material.SetColor("_EmissionColor", color * _colorStrength);
            }
        }
        else
        {
            foreach (var material in materials)
            {
                //we can just disable the EMISSION
                //if we don't use emission color anywhere else
                material.DisableKeyword("_EMISSION");
            }
        }
    }
}
