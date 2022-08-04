using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShaderSaver : MonoBehaviour
{
    public Material mat;
    [HideInInspector]
    public string presetName;

    public void OnValidate()
    {
        mat = GetComponent<Renderer>().sharedMaterial;
    }
}

