using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveSettings", menuName = "ScriptableObjects/WaveSettings", order = 1)]
public class WaveSettings : ScriptableObject
{
    public float _TimeScale;
    public float _TimeScale2;
    public float _TimeScale3;
    public float _TimeScale4;

    public Vector3 _Direction;
    public Vector3 _Direction2;
    public Vector3 _Direction3;
    public Vector3 _Direction4;

    public float _Amplitude;
    public float _Amplitude2;
    public float _Amplitude3;
    public float _Amplitude4;
}
