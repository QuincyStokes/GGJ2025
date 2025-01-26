using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DialogueData", menuName = "ScriptableObject/DIalogueData")]
public class DialogueData : ScriptableObject
{
    //each script will be a day's cutscene worth of dialogue?
    [SerializeField] public Sprite speakerIcon;
    [SerializeField] public Sprite speakerIconOther;
    [SerializeField]public List<string> laikaDialogue;
    [SerializeField]public List<string> otherDialogue;
}
