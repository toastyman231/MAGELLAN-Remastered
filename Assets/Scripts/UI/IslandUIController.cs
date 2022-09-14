using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class IslandUIController : MonoBehaviour
    {
        public static IslandUIController Instance;
        
        [SerializeField] private GameObject islandUI;
        [SerializeField] private Animator islandUIAnim;

        [SerializeField] private GameObject buttonPrefab;

        private readonly int Unfurl = Animator.StringToHash("WideUnfurl");
        private readonly int Furl = Animator.StringToHash("WideFurl");

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        
        public void SetupIsland(OceanEvent island)
        {
            //Show UI background
            islandUI.SetActive(true);
            islandUIAnim.CrossFade(Unfurl, 0.0f, 0);
            
            //Populate name and buttons
            GameObject uiElements = islandUI.transform.GetChild(0).GetChild(0).gameObject;

            uiElements.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = island.eventName.ToUpper();
            
            int i = 0;
            // Remove old buttons
            foreach (Transform child in uiElements.transform)
            {
                if (i != 0) { Destroy(child.gameObject); }

                i++;
            }
            
            // Create new buttons
            foreach (OceanEvent.Actions action in island.actionsList)
            {
                Button actionButton = Instantiate(buttonPrefab, uiElements.transform).GetComponent<Button>();
                actionButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = action.ToString();
                actionButton.onClick.AddListener(island.GetAction(action));
            }

            StartCoroutine(IntroDialogue(island));
        }

        public void LeaveIsland()
        {
            // Put closing dialogue here, before animation plays
            islandUIAnim.CrossFade(Furl, 0.0f, 0);
        }

        private IEnumerator IntroDialogue(OceanEvent island)
        {
            DialogueController.instance.AcceptInput("Welcome to " + island.eventName + "! " + island.eventDesc);
            yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        }
    }
}