using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenu : MonoBehaviour
{
    public GameObject titleMenu;
    public Light sun;
    public float sunStep;
    public Transform camEnd;

    public void PlayGame()
    {
        titleMenu.SetActive(false);

        //Animate opening cutscene here
        //TODO: replace with actual cutscene and intro
        Camera.main.transform.position = camEnd.position;
        Camera.main.transform.rotation = camEnd.rotation;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        sun.transform.Rotate(new Vector3(sunStep, 0, 0));
    }
}
