
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;
    //this is an interesting one, essentially needs references to everything that updates constantly, and disable them?
    [Header("References")]
    public PlayerMovement playerMovement;
    public PlayerAttack playerAttack;
    public Image cutsceneImage;
    public Image cutsceneBlackImage;
    public float fadeTime;
    public Sprite placeholderSprite;
    private bool inCutscene;
    public float cutsceneDuration;
    public List<Sprite> dayEndCutscenes;
    public PlayerOxygen playerOxygen;

    public TMP_Text finText;
    public List<AudioClip> cutsceneSongs;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        inCutscene = false;
        cutsceneImage.enabled = false;
        cutsceneBlackImage.enabled = false;
        //EnterCutscene(placeholderSprite);
        finText.enabled = false;
        
    }
   
    public void EnterCutscene(int cutsceneNumber)
    {
        if(!inCutscene)
        {
            cutsceneImage.sprite = dayEndCutscenes[cutsceneNumber];
            PauseGame();
            StartCoroutine(FadeInCutscene(cutsceneNumber));
            
        }
    }

    private IEnumerator FadeInCutscene(int day=0)
    {   
        inCutscene = true;
        cutsceneImage.color = new Color(1f, 1f, 1f, 0);
        cutsceneImage.enabled = true;

        for (int i = 0; i < EnemySpawnManager.Instance.spawnedEnemies.Count; i++){
            Enemy e = EnemySpawnManager.Instance.spawnedEnemies[0];
            Destroy(e.gameObject);
            EnemySpawnManager.Instance.spawnedEnemies.RemoveAt(0);
        }

        float elapsed = 0f;
        Jukebox.Instance.FadeOutMusic(fadeTime);
        while(elapsed <= fadeTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeTime; //this is percentage done
            cutsceneImage.color = new Color(1f, 1f, 1f, t);
            yield return null;
        }
        Jukebox.Instance.FadeInMusic(cutsceneSongs[day], fadeTime/2);
        cutsceneImage.color = new Color(1f, 1f, 1f, 1f);
        print("starting cutscenefadeouttimer");
        StartCoroutine(CutsceneFadeOutTimer(day));

    }


    private IEnumerator CutsceneFadeOutTimer(int day)
    {
        //DURING THIS TIME, DIALOGUE SHOULD PLAY
        DialogueManager.Instance.BeginCutsceneDialogue(day);
        //can we do a while loop here and just wait for isDialoguing to be false? this is yucky and dangerous i think...
        while(DialogueManager.Instance.IsCutscening == true)
        {
            yield return null;
        }
        
        print("calling exitcutscene");
        ExitCutscene(day);
    }

    public void ExitCutscene(int day)
    {
        print("starting fadeoutcutscene");
        StartCoroutine(FadeOutCutscene(day));

        //empties the SpawnManager list of enemies and recreates new ones. 
        // foreach (Enemy e in EnemySpawnManager.Instance.spawnedEnemies)
        // {
        //     EnemySpawnManager.Instance.spawnedEnemies.Remove(e);
        //     Destroy(e.gameObject);
            
        // }
        
    }

    private IEnumerator FadeOutCutscene(int day)
    {
        //maybe we do a cool fade to black, then take it out?
        //first fade to black
        float elapsed = 0f;  
        cutsceneBlackImage.color = new Color(0f, 0f, 0f, 0f);
        print("Enabling black image");
        cutsceneBlackImage.enabled = true;
        //STEPS
        //fade in the black screen, which is on top of the cutscene iamge
        //turn off the cutscene image
        //fade out the black iamge
        if(day != 3)
        {
            Jukebox.Instance.FadeOutMusic(fadeTime);
        }
        
        while(elapsed <= fadeTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeTime; //this is percentage done
            cutsceneBlackImage.color = new Color(0f, 0f, 0f, t);
            yield return null;
        } 
        cutsceneBlackImage.color = new Color(0f, 0f, 0f, 1f);
        //black is now fully faded in, disable the regular cutscene image
        cutsceneImage.enabled = false;
        DialogueManager.Instance.ClearDialogue();
        //small delay before going back to game to let the emotions of reid's amazing writing sink in
        yield return new WaitForSeconds(1.5f);
        //now fade out the black image
        //HERE WE CHECK IF WE SHOULD END THE GAME INSTEAD
        Debug.Log($"DAY CHECK | {day} ");
        if(day >= 3)
        {
            GameDone();
            //IF WERE HERE, WE HAVE JUST DISPLAYED THE FINAL CUTSCENE
            yield break;
        }
        else
        {
            Jukebox.Instance.FadeInMusic(OrbitManager.Instance.gameplayMusic, fadeTime);

            

            EnemySpawnManager.Instance.SpawnEnemies();

            ResumeGame();


            elapsed = 0;
            while(elapsed <= fadeTime/2)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeTime; //this is percentage done
                cutsceneBlackImage.color = new Color(0f, 0f, 0f, 1-t);
                yield return null;
            } 
            //its fully faded out, turn the black screen off, revealing the game again.
            cutsceneBlackImage.enabled = false;
            inCutscene = false;

            
        }
        
        
    }

    private void PauseGame()
    {
        //everything we need to do to stop the game from functioning, do here
        OrbitManager.Instance.PauseOrbit();
        DialogueManager.Instance.ClearDialogue();
        playerMovement.StopMovement();
        playerMovement.enabled = false;
        playerAttack.enabled = false;
        playerOxygen.PauseOxygen();
        //here i think we would also use gridmanager to delete all current grids, then restart?
    }

    private void ResumeGame()
    {
        //literally just undo everything we did in pause
        OrbitManager.Instance.ResumeOrbit();
        playerMovement.enabled = true;
        playerAttack.enabled = true;
        playerOxygen.ResumeOxygen();

    }

    private void GameDone()
    {
        finText.enabled = true;
        StartCoroutine(GameDoneCountdown());

    }

    private IEnumerator GameDoneCountdown()
    {
        yield return new WaitForSeconds(5f);
        Application.Quit();
    }

}
