using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Fade))]
public class FadeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Fade!"))
        {
            GameManager.instance.InvokeGameOver(true, 0.5f, true);
        }
    }
}
