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

        public static EventHandler<IslandArgs> CreateGUIEvent;
        public static EventHandler HideGUIEvent;

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

            CreateGUIEvent += DrawGUI;
            HideGUIEvent += HideGUI;
        }

        public void InvokeDrawGUI(OceanEvent island)
        {
            CreateGUIEvent?.Invoke(this, new IslandArgs(island));
        }

        public void InvokeHideGUI()
        {
            HideGUIEvent?.Invoke(this, EventArgs.Empty);
        }

        private void DrawGUI(object sender, IslandArgs args)
        {
            //Show UI background
            islandUI.SetActive(true);
            islandUIAnim.CrossFade(Unfurl, 0.0f, 0);
            
            //Populate name and buttons
            GameObject uiElements = islandUI.transform.GetChild(0).GetChild(0).gameObject;

            uiElements.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = args.island.eventName.ToUpper();
            
            int i = 0;
            // Remove old buttons
            foreach (Transform child in uiElements.transform)
            {
                if (i != 0) { Destroy(child.gameObject); }

                i++;
            }
            
            // Create new buttons
            foreach (OceanEvent.Actions action in args.island.actionsList)
            {
                Button actionButton = Instantiate(buttonPrefab, uiElements.transform).GetComponent<Button>();
                actionButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = action.ToString();
                actionButton.onClick.AddListener(args.island.GetAction(action));
            }
        }

        private void HideGUI(object sender, EventArgs args)
        {
            islandUIAnim.CrossFade(Furl, 0.0f, 0);
        }
        
        public void SetupIsland(OceanEvent island)
        {
            InvokeDrawGUI(island);

            StartCoroutine(IntroDialogue(island));
        }

        public void LeaveIsland()
        {
            // Put closing dialogue here, before animation plays
            InvokeHideGUI();
        }

        private IEnumerator IntroDialogue(OceanEvent island)
        {
            DialogueController.instance.AcceptInput("Welcome to " + island.eventName + "! " + island.eventDesc);
            yield return new WaitUntil(() => DialogueController.instance.textState == DialogueController.State.DONE);
        }
    }

    public class IslandArgs : EventArgs
    {
        public OceanEvent island;

        public IslandArgs(OceanEvent isl)
        {
            island = isl;
        }
    }
}