using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginningCutsceneSetup : MonoBehaviour
{
    public GameObject setupMenu;

    private void OnEnable()
    {
        //setupMenu.SetActive(true);

        StartCoroutine(OpeningDialogue());
        //setupMenu.SetActive(true);
    }

    private IEnumerator OpeningDialogue()
    {
        DialogueController.instance.AcceptInput("Welcome to MAGELLAN: Remastered. You are Ferdinand Magellan," +
            " the Captain General of the Armada de Moluccas. Click to Continue.", true);
        yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        DialogueController.instance.AcceptInput("In the year 1519 Magellan and his crew set out to find a sea" +
            " route to the Moluccas, islands that deal in valuable spices.", true);
        yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        DialogueController.instance.AcceptInput("After much opposition, Magellan has finally set off on his journey!", true);
        yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        DialogueController.instance.AcceptInput("You have 3 main resources to manage: Crew, Food, and Gold." +
            " Higher crew will consume more food, but make most tasks easier. Every 30 crewmembers will consume" +
            " 1 food each time you dock.", true);
        yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        DialogueController.instance.AcceptInput("Gold can be used to buy food at certain islands, but earning more will usually" +
            " result in losing crew members, since you must pillage to earn gold!", true);
        yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        DialogueController.instance.AcceptInput("If you find yourself desperately in need of crew, you could try recruiting" +
            " more, either with gold or by converting an island to Christianity and having them join you.", true);
        yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        DialogueController.instance.AcceptInput("However, beware: Doing this on islands that don't trust you may result in" +
            " them kicking you out by force! This may result in losing crew members.", true);
        yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        DialogueController.instance.AcceptInput("One last thing: The more crew you take, the less food you'll have room for, but you'll " +
                                                "need more food to sustain that many crew.", true);
        yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        DialogueController.instance.AcceptInput("You can hold more crew than you have food to sustain, but you can't take more food than you have " +
                                                "room for. Good luck!");
        yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);

        yield return new WaitForSeconds(0.1f);
        setupMenu.SetActive(true);
        Destroy(gameObject);
    }
}
