using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class MainMenu : MonoBehaviour
{
    public GameObject menu;
    public Transform camNewLocation;
    public PlayableDirector newGameCutscene;
    public PlayableDirector continueCutscene;
    public CinemachineVirtualCamera vcam;
    public OceanGeometry geometry;

    public void NewGame()
    {
        menu.SetActive(false);
        Camera.main.transform.position = camNewLocation.position;
        Camera.main.transform.rotation = camNewLocation.rotation;
        newGameCutscene.Play();
    }

    public void ContinueGame()
    {
        //Check save data here
        menu.SetActive(false);
        //Camera.main.transform.position = camNewLocation.position;
        //Camera.main.transform.rotation = camNewLocation.rotation;
        Ship.instance.shipState = Ship.State.SAILING;
        vcam.Priority += 5;
        continueCutscene.Play();
        //geometry.offsetFromCamera = new Vector3(0, 0, -350);
        StartCoroutine(geometry.LerpOverTime(geometry.offsetFromCamera, new Vector3(0, 0, -350), 4, 0.3f));
        StartCoroutine(geometry.LerpOverTime(geometry.lengthScale, 100f, 4, 0.3f));
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
