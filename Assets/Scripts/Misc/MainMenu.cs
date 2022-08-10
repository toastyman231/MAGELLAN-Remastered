using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    public GameObject menu;
    public Transform camNewLocation;
    public PlayableDirector director;

    public void PlayGame()
    {
        menu.SetActive(false);
        Camera.main.transform.position = camNewLocation.position;
        Camera.main.transform.rotation = camNewLocation.rotation;
        director.Play();
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
