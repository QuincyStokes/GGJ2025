
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;


public class DialogueManager : MonoBehaviour
{

    public static DialogueManager Instance;

    [Header("Dialogue Settings")]
    public float charsPerSecond;
    public List<DialogueData> dialogueDatas;
    public float dialogueDuration;

    [Header("UI")]
    //FIRST SPEAKER (laika)
    public TMP_Text dialogueOneUI;
    public Image speakerImage;
    public Image textboxImage;

    public TMP_Text dialogueSoloUI;
    public Image dialogueSoloImageUI;

    //SECOND SPEAKER (will change)
    

    private bool isDialogueDisplaying;
    public bool IsDialoguePlaying => isDialogueDisplaying;

    private bool isCutscening;
    public bool IsCutscening => isCutscening;



    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        dialogueOneUI.enabled = false;
        speakerImage.enabled = false;
        textboxImage.enabled = false;

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            ClearDialogue();
            CutsceneManager.Instance.ExitCutscene(0);
        }
    }


    public void StartDialogue(string dialogue, Sprite speaker=null, float duration=2, int who=1, bool clearOnEnd = true)
    {
        if(!isDialogueDisplaying)
        {
            StartCoroutine(DisplayDialogue(dialogue, speaker, duration, who, clearOnEnd));
        }
    }

    /// <summary>
    /// This function handles the displaying of dialogue, can display on either dialogue area for conversation
    /// only issue with it is the dialogue clears after it's done, maybe we want it to stay there
    /// </summary>
    /// <param name="dialogue">the string to speak</param>
    /// <param name="speaker">the string speaker</param>
    /// <param name="duration">the time for the dialogue to stay on the screen</param>
    /// <param name="who">1 == laika, 2 == other gamer, DEFAULT IS LAIKA</param>
    /// <returns>nothing, just wait</returns>
    private IEnumerator DisplayDialogue(string dialogue, Sprite speaker, float duration, int who=1, bool clearOnEnd=true)
    {   
        
        //if laika is speaking, set the first textbox dialogue
        if(who == 1)
        {
            

            dialogueOneUI.enabled = true;
            dialogueOneUI.text = "";

            speakerImage.sprite = speaker;
            speakerImage.enabled = true;
            textboxImage.enabled = true;
            //dialogueSpeakerOneUI.text = speaker; CHANGE TO IMAGE
            
            
            foreach (char letter in dialogue)
            {
                dialogueOneUI.text += letter;
                yield return new WaitForSeconds(charsPerSecond);
            }
            yield return new WaitForSeconds(duration);

            if(clearOnEnd)
            {
                dialogueOneUI.text = "";
                dialogueOneUI.enabled= false;

                speakerImage.enabled = false;
                speakerImage.sprite = null;
                textboxImage.enabled = false;
            }
        }
        else
        {
            isDialogueDisplaying = true;
            dialogueSoloUI.text = "";
            dialogueSoloUI.enabled = true;
            dialogueSoloImageUI.sprite =speaker;
            dialogueSoloImageUI.enabled = true;

            foreach (char letter in dialogue)
            {
                dialogueSoloUI.text += letter;
                yield return new WaitForSeconds(charsPerSecond);
            }
            yield return new WaitForSeconds(duration);

            if(clearOnEnd)
            {
                dialogueSoloUI.text = "";
                dialogueSoloUI.enabled= false;

                dialogueSoloImageUI.enabled = false;
                dialogueSoloImageUI.sprite = null;
            }
        }
       
           

        // isDialogueDisplaying = false;
    
    
        // isDialogueDisplaying = true;

        // dialogueTwoUI.enabled = true;
        // dialogueTwoUI.text = "";

        // dialogueSpeakerTwoUI.enabled = true;
        // dialogueSpeakerTwoUI.text = speaker;
        
        // foreach (char letter in dialogue)
        // {
        //     dialogueTwoUI.text += letter;
        //     yield return new WaitForSeconds(charsPerSecond);
        // }
        // yield return new WaitForSeconds(duration);

        // if(clearOnEnd)
        // {
        //     dialogueTwoUI.text = "";
        //     dialogueTwoUI.enabled= false;

        //     dialogueSpeakerTwoUI.enabled = false;
        //     dialogueSpeakerTwoUI.text = "";
        // }
        // isDialogueDisplaying = false;
        
       
    }

    public void ClearDialogue()
    {
        StopAllCoroutines();
        dialogueOneUI.text = "";
        speakerImage.sprite = null;
        speakerImage.enabled = false;
        textboxImage.enabled = false;
    }

    public void BeginCutsceneDialogue(int day)
    {
        //day is the index in the list of dialogue that we should play from
        //just loop through the list and start DisplayDialogue?

        //STEPS TO DOING THAT
        //set dialogue box 1 to firstName, and dialogue box 2 to secondName and then call DisplayDialogue for name 1
        //WHEN THAT IS DONE, (this is the hard part), call displaydialogue for name 2, and then repeat.
        //is this just one massive coroutine?
        StartCoroutine(PlayEntireCutsceneDialogue(day));
    }

    private IEnumerator PlayEntireCutsceneDialogue(int day)
    {
        isCutscening = true;
        
        //loops for the number of laika's lines + number of other person's lines
        print($"day is {day}");
        for(int i = 0; i < Math.Max(dialogueDatas[day].laikaDialogue.Count, dialogueDatas[day].otherDialogue.Count); i++)
        {
            //now we want to play the dialogue back and forth.. 
            //first play laikas dialogue
            if(dialogueDatas[day].laikaDialogue[i] != null)
            {
                StartCoroutine(DisplayDialogue(dialogueDatas[day].laikaDialogue[i], dialogueDatas[day].speakerIcon, dialogueDuration, 1, false));
                //HARD PART, we need to wait for this to end before playing the other person's. Can we calculate how long it'll take?
                yield return new WaitForSeconds(dialogueDatas[day].laikaDialogue[i].Length * charsPerSecond + dialogueDuration);
            }
            //...now do it again for other person?
            if(dialogueDatas[day].otherDialogue[i] != null)
            {
                StartCoroutine(DisplayDialogue(dialogueDatas[day].otherDialogue[i], dialogueDatas[day].speakerIconOther, dialogueDuration, 2, false));
            
                yield return new WaitForSeconds(dialogueDatas[day].otherDialogue[i].Length * charsPerSecond + dialogueDuration);
            //winning hopefully?
            }
        }
        //need to clear all the dialgogue now WAIT I HAVE A FUNCTION FOR THIS
        ClearDialogue();
        isCutscening = false;
    }

}
