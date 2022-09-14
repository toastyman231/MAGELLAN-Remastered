using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public OceanEvent[] landmarks;
    public OceanEvent[] stopEvents;
    public OceanEvent currentEvent;
    
    public int[] stops;

    //public bool done;
    public bool gameStarted;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }
        
        gameStarted = false;
    }

    public IEnumerator StartGame()
    {
        if (gameStarted) yield break;
        OceanEvent landmark;
        OceanEvent stop;

        gameStarted = true;
        for (int i = 0; i < landmarks.Length; i++)
        {
            for(int j = 0; j < stops[i]; j++)
            {
                //Choose random stop from list (should override at certain story based stops)
                stop = Instantiate(stopEvents[Random.Range(0, stopEvents.Length)], new Vector3(0, -400, 0),
                    Quaternion.identity, transform).GetComponent<OceanEvent>();
                currentEvent = stop;

                //Move player back to make room for island to spawn, wait until player is done at the island
                yield return StartCoroutine(WaitAndShowEvent(stop));

                yield return new WaitUntil(() => stop.done);

                //Hide and despawn the island, set the player sailing again
                //stop.CloseEvent();

                yield return new WaitUntil(() =>
                    Vector3.Distance(Ship.instance.transform.position, Ship.instance.targets[0]) <= 0.3f);
                yield return new WaitForSeconds(3f);
            }

            //Same as above, but the island is a major landmark and each is visited in order
            landmark = Instantiate(landmarks[i], new Vector3(0, -400, 0),
                    Quaternion.identity, transform).GetComponent<OceanEvent>();
            currentEvent = landmark;
            yield return StartCoroutine(WaitAndShowEvent(landmark));

            yield return new WaitUntil(() => landmark.done);
            //landmark.CloseEvent();

            yield return new WaitUntil(() =>
                    Vector3.Distance(Ship.instance.transform.position, Ship.instance.targets[0]) <= 0.3f);
            yield return new WaitForSeconds(3f);
        }
    }

    public IEnumerator WaitAndShowEvent(OceanEvent island)
    {
        Ship.instance.shipState = Ship.State.DOCKING;
        yield return new WaitUntil(() => 
            Vector3.Distance(Ship.instance.transform.position, Ship.instance.targets[1]) <= 0.3f);
        
        island.InitiateEvent();
    }
    
    public void GameStarter()
    {
        //Evil function that only exists so i can delete an object
        StartCoroutine(StartGame());
    }
}

