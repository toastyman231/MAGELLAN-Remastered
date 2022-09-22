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

    // How many stops will proceed each landmark? Each element corresponds to an element in landmarks.
    public int[] stops;
    public int progress;

    //public bool done;
    public bool gameStarted;

    private int _maxDifficulty;
    private int _currentStop;
    private int _currentLandmark;
    public int maxDifficulty
    {
        get => _maxDifficulty + (progress * 5) + 1;
        set => _maxDifficulty = value;
    }

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
        _maxDifficulty = 100;
        //TODO: Load these from player save
        _currentStop = 0;
        _currentLandmark = 0;
    }

    public IEnumerator StartGame()
    {
        if (gameStarted) yield break;
        OceanEvent landmark;
        OceanEvent stop;

        gameStarted = true;
        for (int i = _currentLandmark; i < landmarks.Length; i++)
        {
            for(int j = _currentStop; j < stops[i]; j++)
            {
                //Choose random stop from list (should override at certain story based stops)
                stop = Instantiate(stopEvents[Random.Range(0, stopEvents.Length)], new Vector3(0, -400, 0),
                    Quaternion.identity, transform).GetComponent<OceanEvent>();
                currentEvent = stop;
                _currentStop = j;
                progress = _currentLandmark + _currentStop;

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
            _currentLandmark = i;
            progress = _currentLandmark + _currentStop;
            
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

