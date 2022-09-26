using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image img;
    public float fadeTime;

    // Start is called before the first frame update
    void Start()
    {
        img = transform.GetChild(0).GetComponent<Image>();
        img.CrossFadeAlpha(1, 0.0f, false);
        img.CrossFadeAlpha(0, fadeTime, false);
        //UpdateRaycastTarget();
    }

    private void OnEnable()
    {
        GameManager.GameOverEventHandler += FadeInOut;
    }

    private void OnDisable()
    {
        GameManager.GameOverEventHandler -= FadeInOut;
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        img = transform.GetChild(0).GetComponent<Image>();
        img.CrossFadeAlpha(0, 0.0f, false);
#endif
    }

    private void FadeInOut(object sender, FadeArgs args)
    {
        int start = args.fadeIn ? 1 : 0;
        img.CrossFadeAlpha(start, args.time, false);
        if (args.gameOver) transform.GetChild(1).gameObject.SetActive(true);
        //UpdateRaycastTarget();
    }

    // Update is called once per frame
    void Update()
    {
        //img.CrossFadeAlpha(0, fadeTime, false);
        //UpdateRaycastTarget();
    }

    void UpdateRaycastTarget()
    {
        img.raycastTarget = img.gameObject.GetComponent<CanvasRenderer>().GetAlpha() > 0f;
    }
}

public class FadeArgs : EventArgs
{
    public bool fadeIn;
    public bool gameOver;
    public float time;

    public FadeArgs(bool fade, float fadeTime, bool over)
    {
        fadeIn = fade;
        time = fadeTime;
        gameOver = over;
    }
}
