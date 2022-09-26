using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public OceanEvent[] landmarks;
    public OceanEvent[] stopEvents;
    public OceanEvent currentEvent;

    // How many stops will proceed each landmark? Each element corresponds to an element in landmarks.
    public int[] stops;
    public int progress;

    public bool gameOver;
    public bool gameStarted;

    public static EventHandler<FadeArgs> GameOverEventHandler;

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
        gameOver = false;
        _maxDifficulty = 100;
        //TODO: Load these from player save
        _currentStop = 0;
        _currentLandmark = 0;
    }

    public void InvokeGameOver(bool fadeIn, float time, bool gameOver)
    {
        GameOverEventHandler?.Invoke(this, new FadeArgs(fadeIn, time, gameOver));
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
        
        DialogueController.instance.AcceptInput("Nearly 3 years after their departure, Magellan's crew have finally made it back to Spain!", true);
        yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        DialogueController.instance.AcceptInput("As their numbers dwindled, they abandoned one ship, and another was captured by the Portuguese, leaving " +
                                                "only one ship remaining out of the original 3 by the time the crew docks at Seville, Spain.", true);
        yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        StartCoroutine(EndGame());
    }

    private IEnumerator WaitAndShowEvent(OceanEvent island)
    {
        Ship.instance.shipState = Ship.State.DOCKING;
        yield return new WaitUntil(() => 
            Vector3.Distance(Ship.instance.transform.position, Ship.instance.targets[1]) <= 0.3f);
        
        island.InitiateEvent();
    }

    public IEnumerator EndGame()
    {
        if (ResourceManager.instance.crew >= ResourceManager.instance.startingCrew)
        {
            DialogueController.instance.AcceptInput("Out of the " + ResourceManager.instance.startingCrew + " crew members that left Spain, " +
                                                    "you made it back with " + ResourceManager.instance.crew + "!");
            yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        }
        else
        {
            DialogueController.instance.AcceptInput("Out of the " + ResourceManager.instance.startingCrew + " crew members that left Spain, " +
                                                    (ResourceManager.instance.crew <= 0 ? 
                                                        "none have made it back." : 
                                                        "only " + ResourceManager.instance.crew + " have returned."), true);
            yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        }

        int score = ResourceManager.instance.crew + ResourceManager.instance.gold; // TODO: Divide by time taken and adjust score categories

        switch (score)
        {
            case >= 1000:
                DialogueController.instance.AcceptInput("Your score was " + score + "! You stand at the top of the Spanish Naval world!", true);
                yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
            
                DialogueController.instance.AcceptInput("Your final rank is CAPTAIN GENERAL.");
                yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
                break;
            case >= 600:
                DialogueController.instance.AcceptInput("Your score was " + score + "! " +
                                                        "You did pretty well for yourself, but there's still more you could get out of this world.", true);
                yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
            
                DialogueController.instance.AcceptInput("Your final rank is ADMIRAL.");
                yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
                break;
            case >= 400:
                DialogueController.instance.AcceptInput("Your score was " + score + "! Your name will certainly go down in history, but will you be famous" +
                                                        " or infamous?", true);
                yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
            
                DialogueController.instance.AcceptInput("Your final rank is CAPTAIN.");
                yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
                break;
            case >= 200:
                DialogueController.instance.AcceptInput("Your score was " + score + "! Perhaps you could have tried a bit harder...", true);
                yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
            
                DialogueController.instance.AcceptInput("Your final rank is FRIGATE LIEUTENANT.");
                yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
                break;
            default:
                DialogueController.instance.AcceptInput("Your score was " + score + "! Surely there was more you could have done?", true);
                yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
            
                DialogueController.instance.AcceptInput("Your final rank is SEAMAN.");
                yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
                break;
        }
        
        instance.InvokeGameOver(true, 0.5f, true);
    }
    
    public void GameStarter()
    {
        //Evil function that only exists so i can delete an object
        StartCoroutine(StartGame());
    }
}

