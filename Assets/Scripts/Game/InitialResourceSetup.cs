using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InitialResourceSetup : MonoBehaviour
{
    public GameObject donePanel;
    public Animator scrollAnim;

    private readonly int Furl = Animator.StringToHash("ScrollFurl");

    public void CheckDone()
    {
        donePanel.SetActive(true);
    }

    public void NotDone()
    {
        donePanel.SetActive(false);
    }

    public void Done()
    {
        scrollAnim.CrossFade(Furl, 0.0f, 0);
        //Set sail
    }
}
