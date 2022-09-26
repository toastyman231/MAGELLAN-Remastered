using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UI;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class OceanEvent : MonoBehaviour
{
    public enum Type { LANDMARK, ISLAND, COMBAT, QUEST, TRADER };
    public enum Actions { PILLAGE, TRADE, RECRUIT, QUEST, MINIGAME, LEAVE };

    private readonly int Enter = Animator.StringToHash("Enter");
    private readonly int Exit = Animator.StringToHash("Exit");

    public string eventName;
    public string eventDesc;

    public float difficulty;

    public Animator anim;

    public Type eventType;
    public Actions[] actionsList;

    public bool done;

    private void Awake()
    {
        difficulty = Random.Range(0f, 50f);
        anim = GetComponent<Animator>();
        done = false;

        if(actionsList.Length == 0 || actionsList == null)
        {
            if (eventType is Type.LANDMARK or Type.ISLAND)
            {
                actionsList = new[]
                {
                    Actions.TRADE, Actions.RECRUIT, Actions.PILLAGE, Actions.LEAVE
                };
            }
        }
    }

    private void ReduceCrewAndFood()
    {
        if (eventType == Type.LANDMARK || eventType == Type.ISLAND)
        {
            int foodNeeded = ResourceManager.instance.crew / 30;
            if (foodNeeded <= ResourceManager.instance.food) 
                ResourceManager.instance.food -= foodNeeded;
            else
            {

                ResourceManager.instance.crew -= (foodNeeded - ResourceManager.instance.food) * 30;
                ResourceManager.instance.food = 0;
            }
        }

        if (ResourceManager.instance.crew <= 0)
        {
            StartCoroutine(GameManager.instance.EndGame());
            GameManager.instance.gameOver = true;
        }
    }

    public void InitiateEvent()
    {
        //Ship.instance.shipState = Ship.State.DOCKING;
        ReduceCrewAndFood();
        if (!GameManager.instance.gameOver)
            anim.CrossFade(Enter, 0.0f, 0);
        //if (eventType != Type.ISLAND) difficulty += ResourceManager.instance.crew / 30 * 5;
    }

    public async void CloseEvent()
    {
        Ship.instance.shipState = Ship.State.SAILING;

        anim.CrossFade(Exit, 0.0f, 0);
        //Thread.Sleep(10000);
        await Task.Delay(10000);
        Destroy(gameObject);
    }

    public UnityAction GetAction(Actions action)
    {
        switch (action)
        {
            case Actions.LEAVE:
                return () =>
                {
                    done = true;
                    IslandUIController.Instance.LeaveIsland();
                };
            case Actions.QUEST:
                // Return proper function based on current quest
                return () => Debug.Log("Quest");
            case Actions.MINIGAME:
                // Return proper function based on which minigame should play
                // Successful minigames increases trust (difficulty)
                return () => Debug.Log("Minigame");
            case Actions.TRADE:
                // Return trade function
                // Trade increases trust (difficulty)
                // Trading spices (item) increases trust greatly
                return () =>
                {
                    StartCoroutine(Trade());
                };
            case Actions.RECRUIT:
                // Return recruit function
                return () =>
                {
                    IslandUIController.Instance.InvokeHideGUI();
                    StartCoroutine(Recruit());
                };
            case Actions.PILLAGE:
                // Return pillage function
                // TODO: Replace this with a pillage minigame later
                return () =>
                {
                    IslandUIController.Instance.InvokeHideGUI();
                    StartCoroutine(Pillage());
                };
            default:
                return () => Debug.Log("Invalid Action type");
        }
    }

    private IEnumerator Recruit()
    {
        if (ResourceManager.instance.crew >= ResourceManager.instance.crewMax)
        {
            DialogueController.instance.AcceptInput("Your ship cannot hold any more crew! Consider using or selling some of your items and food!");
            yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
            
            IslandUIController.Instance.InvokeDrawGUI(this);
            yield break;
        }
        
        DialogueController.instance.AcceptInput("You send the crew out to recruit new members from the islands' local population.", true);
        yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        
        // Play animation here?
        DialogueController.instance.AcceptInput("...", true);
        yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        DialogueController.instance.AcceptInput("...", true);
        yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        DialogueController.instance.AcceptInput("...", true);
        yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        
        // Higher difficulty value means better chance of succeeding (30 or lower can never win)
        int randomInt = Random.Range(30, GameManager.instance.maxDifficulty);
        if (difficulty >= randomInt)
        {
            DialogueController.instance.AcceptInput("A few of the natives were.... persuaded... to join your crew.", true);
            yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
            ResourceManager.instance.crew += Random.Range(5, 11);
            DialogueController.instance.AcceptInput("Your crew now has " + ResourceManager.instance.crew + " members!");
            yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        }
        else
        {
            DialogueController.instance.AcceptInput("The natives were not happy with your attempts to recruit them...", true);
            yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
            
            randomInt = Random.Range(30, GameManager.instance.maxDifficulty);
            if (difficulty >= randomInt)
            {
                DialogueController.instance.AcceptInput("Some careful negotiation prevents a battle from breaking out. Perhaps you should do something to gain the natives'" +
                                                        " trust...");
                yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
            }
            else
            {
                yield return StartCoroutine(Battle(difficulty, true));

                DialogueController.instance.AcceptInput("Perhaps you should try doing something to make the natives trust you more...");
                yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
            }
        }
        
        IslandUIController.Instance.InvokeDrawGUI(this);
    }

    private IEnumerator Pillage()
    {
        DialogueController.instance.AcceptInput("You ready your crew to take this island for all it's worth!", true);
        yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);

        yield return StartCoroutine(Battle(difficulty + ResourceManager.instance.crew / 30 * 5, true));

        int goldStolen = Random.Range(100, 301);
        int foodStolen = Random.Range(0, 4);
        DialogueController.instance.AcceptInput("You stole " + goldStolen + " gold and " + foodStolen + " food from the people of " + eventName + "!", true);
        yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        ResourceManager.instance.gold += goldStolen;
        ResourceManager.instance.food += foodStolen;
        
        DialogueController.instance.AcceptInput("The people of " + eventName + " will never trust you now...");
        yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        difficulty = 0;
        
        IslandUIController.Instance.InvokeDrawGUI(this);
    }

    private IEnumerator Battle(float battleDiff, bool more = false, int minDifficulty = 30, string enemy = "the natives")
    {
        DialogueController.instance.AcceptInput("A battle breaks out between your crew and " + enemy + "!", true);
        yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);

        int randomInt = Random.Range(minDifficulty, GameManager.instance.maxDifficulty);
        if (battleDiff >= randomInt)
        {
            int crewLost = Random.Range(5, 16);
            DialogueController.instance.AcceptInput("Your forces easily overwhelm " + enemy + "' forces! Your losses are minimal, but you still " +
                                                    "lost " + crewLost + " crew!", true);
            yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);

            ResourceManager.instance.crew -= crewLost;
            DialogueController.instance.AcceptInput("You now have " + ResourceManager.instance.crew + " crew!");
            yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        }
        else
        {
            int crewLost = Random.Range(20, 46);
            DialogueController.instance.AcceptInput("The battle is harder than you expected! You suffer great losses, losing " +
                                                    crewLost + " crew!", true);
            yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
            ResourceManager.instance.crew -= crewLost;
            DialogueController.instance.AcceptInput("You now have " + ResourceManager.instance.crew + " crew!", more);
            yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        }
    }

    private IEnumerator Trade()
    {
        if (difficulty <= 0)
        {
            DialogueController.instance.AcceptInput("The inhabitants of this island do not trust you enough to trade with you!");
            yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
            yield break;
        }

        int chance = Random.Range(0, 4);
        Item[] items = {
            new("1 Food", 100),
            new("40 Crew", 2000, 40),
            ResourceManager.instance.defaultItems[chance],
            new("Leave", 0)
        };
        
        IslandUIController.Instance.InvokeShowTrade(items);
    }

    public void RunSetup()
    {
        IslandUIController.Instance.SetupIsland(this);
    }
}
