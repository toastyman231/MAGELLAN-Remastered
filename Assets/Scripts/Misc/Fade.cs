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
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        img = transform.GetChild(0).GetComponent<Image>();
        img.CrossFadeAlpha(0, 0.0f, false);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        img.CrossFadeAlpha(0, fadeTime, false);
    }
}
