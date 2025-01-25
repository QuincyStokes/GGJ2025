using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public static DialogueManager Instance;

    [Header("Dialogue Settings")]
    public float charsPerSecond;

    [Header("UI")]
    public TMP_Text dialogueUI;
    public TMP_Text dialogueSpeakerUI;
    private bool isDialogueDisplaying;

    

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
        dialogueUI.enabled = false;
        dialogueSpeakerUI.enabled = false;
    }


    public void StartDialogue()
    {
        //set dialogue to some default?
    }

    public void StartDialogue(string dialogue, string speaker, float duration=2)
    {
        if(!isDialogueDisplaying)
        {
            StartCoroutine(DisplayDialogue(dialogue, speaker, duration));
        }
    }

    private IEnumerator DisplayDialogue(string dialogue, string speaker, float duration)
    {
        isDialogueDisplaying = true;

        dialogueUI.enabled = true;
        dialogueUI.text = "";

        dialogueSpeakerUI.enabled = true;
        dialogueSpeakerUI.text = speaker;
        
        foreach (char letter in dialogue)
        {
            dialogueUI.text += letter;
            yield return new WaitForSeconds(charsPerSecond);
        }
        yield return new WaitForSeconds(duration);

        dialogueUI.text = "";
        dialogueUI.enabled= false;

        dialogueSpeakerUI.enabled = false;
        dialogueSpeakerUI.text = "";

        isDialogueDisplaying = false;
    }

}
