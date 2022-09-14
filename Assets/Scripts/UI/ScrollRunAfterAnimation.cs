using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollRunAfterAnimation : MonoBehaviour
{
    public void RunAfterAnimation()
    {
        GameManager.instance.currentEvent.CloseEvent();
    }
}
