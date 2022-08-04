using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShaderSaver))]
public class ShaderSaverEditor : Editor
{
    string text = "Preset1";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ShaderSaver myTarget = (ShaderSaver)target;

        text = EditorGUILayout.TextField("Preset Name: ", text);

        if (GUILayout.Button("Save Wave"))
        {
            WaveSettings settings = CreateInstance<WaveSettings>();

            settings._TimeScale = myTarget.mat.GetFloat("_TimeScale");
            settings._TimeScale2 = myTarget.mat.GetFloat("_TimeScale2");
            settings._TimeScale3 = myTarget.mat.GetFloat("_TimeScale3");
            settings._TimeScale4 = myTarget.mat.GetFloat("_TimeScale4");

            settings._Direction = myTarget.mat.GetVector("_Direction");
            settings._Direction2 = myTarget.mat.GetVector("_Direction2");
            settings._Direction3 = myTarget.mat.GetVector("_Direction3");
            settings._Direction4 = myTarget.mat.GetVector("_Direction4");

            settings._Amplitude = myTarget.mat.GetFloat("_Amplitude");
            settings._Amplitude2 = myTarget.mat.GetFloat("_Amplitude2");
            settings._Amplitude3 = myTarget.mat.GetFloat("_Amplitude3");
            settings._Amplitude4 = myTarget.mat.GetFloat("_Amplitude4");

            AssetDatabase.CreateAsset(settings, "Assets/Resources/Scriptable Objects/" + text + ".asset");
        }

        if (GUILayout.Button("Load Wave"))
        {
            if(AssetDatabase.FindAssets(text, new[] { "Assets/Resources/Scriptable Objects" }).Length > 0)
            {
                WaveSettings settings = AssetDatabase.LoadAssetAtPath<WaveSettings>
                    ("Assets/Resources/Scriptable Objects/" + text + ".asset");

                WaveManager.SetCurrentWave(text);

                /*myTarget.mat.SetFloat("_TimeScale", settings._TimeScale);
                myTarget.mat.SetFloat("_TimeScale2", settings._TimeScale2);
                myTarget.mat.SetFloat("_TimeScale3", settings._TimeScale3);
                myTarget.mat.SetFloat("_TimeScale4", settings._TimeScale4);

                myTarget.mat.SetVector("_Direction", settings._Direction);
                myTarget.mat.SetVector("_Direction2", settings._Direction2);
                myTarget.mat.SetVector("_Direction3", settings._Direction3);
                myTarget.mat.SetVector("_Direction4", settings._Direction4);

                myTarget.mat.SetFloat("_Amplitude", settings._Amplitude);
                myTarget.mat.SetFloat("_Amplitude2", settings._Amplitude2);
                myTarget.mat.SetFloat("_Amplitude3", settings._Amplitude3);
                myTarget.mat.SetFloat("_Amplitude4", settings._Amplitude4);*/
            } else
            {
                Debug.LogWarning("File was not found!");
            }
        }
    }
}
