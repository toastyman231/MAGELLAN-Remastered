using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private static WaveSettings currentWave;
    private static Material waves;

    private void Start()
    {
        currentWave = ScriptableObject.CreateInstance<WaveSettings>();
        SetCurrentWave("BiggerWaves");
    }

    public static bool SetCurrentWave(string newWave)
    {
        WaveSettings newWaveSettings = Resources.Load<WaveSettings>("Scriptable Objects/" + newWave);

        if(waves == null)
        {
            waves = GameObject.FindGameObjectWithTag("Water").GetComponent<Renderer>().sharedMaterial;
        }

        if(newWaveSettings != null)
        {
            //Debug.Log("Set wave");
            currentWave = newWaveSettings;
            ReloadWaves();
            return true;
        } else
        {
            return false;
        }
    }

    private static void ReloadWaves()
    {
        waves.SetFloat("_TimeScale", currentWave._TimeScale);
        waves.SetFloat("_TimeScale2", currentWave._TimeScale2);
        waves.SetFloat("_TimeScale3", currentWave._TimeScale3);
        waves.SetFloat("_TimeScale4", currentWave._TimeScale4);

        waves.SetVector("_Direction", currentWave._Direction);
        waves.SetVector("_Direction2", currentWave._Direction2);
        waves.SetVector("_Direction3", currentWave._Direction3);
        waves.SetVector("_Direction4", currentWave._Direction4);

        waves.SetFloat("_Amplitude", currentWave._Amplitude);
        waves.SetFloat("_Amplitude2", currentWave._Amplitude2);
        waves.SetFloat("_Amplitude3", currentWave._Amplitude3);
        waves.SetFloat("_Amplitude4", currentWave._Amplitude4);
    }

    public static float GetWaveHeight(float _x, float _z)
    {
        float freq1 = Mathf.Sqrt((currentWave._Direction.magnitude * 9) * (float) System.Math.Tanh(9 * currentWave._Direction.magnitude));
        float theta1 = ((currentWave._Direction.x * _x) + (currentWave._Direction.z * _z)) - freq1 * currentWave._TimeScale - 0;
        float y1 = Mathf.Cos(theta1) * currentWave._Amplitude;

        float freq2 = Mathf.Sqrt((currentWave._Direction2.magnitude * 9) * (float)System.Math.Tanh(9 * currentWave._Direction2.magnitude));
        float theta2 = ((currentWave._Direction2.x * _x) + (currentWave._Direction2.z * _z)) - freq2 * currentWave._TimeScale2 - 0;
        float y2 = Mathf.Cos(theta2) * currentWave._Amplitude2;

        float freq3 = Mathf.Sqrt((currentWave._Direction3.magnitude * 9) * (float)System.Math.Tanh(9 * currentWave._Direction3.magnitude));
        float theta3 = ((currentWave._Direction3.x * _x) + (currentWave._Direction3.z * _z)) - freq3 * currentWave._TimeScale3 - 0;
        float y3 = Mathf.Cos(theta3) * currentWave._Amplitude3;

        float freq4 = Mathf.Sqrt((currentWave._Direction4.magnitude * 9) * (float)System.Math.Tanh(9 * currentWave._Direction4.magnitude));
        float theta4 = ((currentWave._Direction4.x * _x) + (currentWave._Direction4.z * _z)) - freq4 * currentWave._TimeScale4 - 0;
        float y4 = Mathf.Cos(theta4) * currentWave._Amplitude4;

        return y1 + y2 + y3 + y4 + -127.23f;
    }
}
