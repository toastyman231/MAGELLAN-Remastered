using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public enum State { DONE, WAITING, TYPING };

    public Animator dialogueAnim;
    public TextMeshProUGUI boxText;
    public float textSpeed;
    public State textState;
    private bool boxOpen;
    private bool moreText;
    private string currentText;

    public static DialogueController instance;

    public readonly int Open = Animator.StringToHash("DialogueShow");
    public readonly int Close = Animator.StringToHash("DialogueHide");

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        dialogueAnim = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<Animator>();
        boxText = dialogueAnim.gameObject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        boxOpen = false;
        textSpeed = 0.01f;
        moreText = false;
        boxText.text = "";
        textState = State.DONE;
    }

    public void Update()
    {
        if (textState == State.WAITING && !moreText)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Done, no more");
                //textBoxAnimator.SetBool("Close", true);
                SetBoxState(false);
                textState = State.DONE;
            }
        }
        else if (textState == State.WAITING && moreText)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Done, more text");
                boxText.text = "";
                textState = State.DONE;
            }
        } else if (textState == State.TYPING)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Skip text");
                boxText.text = "";
                boxText.text = currentText;
                textState = State.WAITING;
            }
        }
    }

    public void AcceptInput(string text, bool more = false)
    {
        boxText.text = "";
        if (!boxOpen)
        {
            SetBoxState(true);
        }

        if(text != "" && textState == State.DONE)
        {
            //Debug.Log("About to type: " + text);
            moreText = more;
            currentText = text;
            instance.StartCoroutine(DisplayText(text));
        }
    }

    public IEnumerator DisplayText(string textToDisplay)
    {
        textState = State.TYPING;
        for (int i = 0; i < textToDisplay.Length; i++)
        {
            if(textState == State.TYPING)
            {
                char c = textToDisplay[i];
                boxText.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
        }

        textState = State.WAITING;
    }

    public void SetBoxState(bool newState)
    {
        boxOpen = newState;

        switch (boxOpen)
        {
            case true:
                //Open
                dialogueAnim.CrossFade(Open, 0.0f, 0);
                break;
            case false:
                //Close
                dialogueAnim.CrossFade(Close, 0.0f, 0);
                break;
        }
    }
}
