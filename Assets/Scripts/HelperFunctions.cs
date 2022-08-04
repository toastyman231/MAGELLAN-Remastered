using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions
{
    public static bool ApproxEqual(float value1, float value2, float acceptableDifference)
    {
        return Mathf.Abs(value1 - value2) <= acceptableDifference;
    }
}
