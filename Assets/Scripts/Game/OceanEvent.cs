using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class OceanEvent : MonoBehaviour
{
    public enum Type { LANDMARK, ISLAND, COMBAT, QUEST, TRADER };
    public enum Actions { PILLAGE, TRADE, CONVERT, QUEST, MINIGAME, LEAVE };

    private readonly int Enter = Animator.StringToHash("Enter");
    private readonly int Exit = Animator.StringToHash("Exit");

    private Animator anim;

    public Type eventType;
    public Actions[] actionsList;

    public bool done;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        done = false;

        if(actionsList.Length == 0 || actionsList == null)
        {
            actionsList = new Actions[1];
            actionsList[0] = Actions.LEAVE;
        }
    }

    public void InitiateEvent()
    {
        //Ship.instance.shipState = Ship.State.DOCKING;

        anim.CrossFade(Enter, 0.0f, 0);
    }

    public async void CloseEvent()
    {
        Ship.instance.shipState = Ship.State.SAILING;

        anim.CrossFade(Exit, 0.0f, 0);
        //Thread.Sleep(10000);
        await Task.Delay(10000);
        Destroy(gameObject);
    }
}
