using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollRunAfterAnimation : MonoBehaviour
{
    public void RunAfterAnimation()
    {
        if (GameManager.instance.currentEvent.done)
            GameManager.instance.currentEvent.CloseEvent();
    }
}
