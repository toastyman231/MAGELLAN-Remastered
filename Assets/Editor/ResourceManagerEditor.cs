using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ResourceManager))]
public class ResourceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        EditorGUILayout.LabelField("DO NOT USE OUTSIDE PLAY MODE");
        
        if (GUILayout.Button("Add 10 Crew"))
        {
            ResourceManager.instance.crew += 10;
        }
        if (GUILayout.Button("Subtract 10 Crew"))
        {
            ResourceManager.instance.crew -= 10;
        }
        
        if (GUILayout.Button("Add 10 food"))
        {
            ResourceManager.instance.food += 10;
        }
        if (GUILayout.Button("Subtract 10 food"))
        {
            ResourceManager.instance.food -= 10;
        }
        
        if (GUILayout.Button("Add 10 Gold"))
        {
            ResourceManager.instance.gold += 10;
        }
        if (GUILayout.Button("Subtract 10 Gold"))
        {
            ResourceManager.instance.gold -= 10;
        }
    }
}
