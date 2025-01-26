
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    //this is an interesting one, essentially needs references to everything that updates constantly, and disable them?
    [Header("References")]
    public PlayerMovement playerMovement;
    public Image cutsceneImage;
    public Image cutsceneBlackImage;
    public float fadeTime;
    public Sprite placeholderSprite;
    private bool inCutscene;
    public float cutsceneDuration;

    void Start()
    {
        inCutscene = false;
        cutsceneImage.enabled = false;
        cutsceneBlackImage.enabled = false;
        EnterCutscene(placeholderSprite);
        
    }
    public void EnterCutscene(Sprite cutsceneSprite)
    {
        if(!inCutscene)
        {
            cutsceneImage.sprite = cutsceneSprite;
            PauseGame();
            StartCoroutine(FadeInCutscene());
        }
        
    }

    private IEnumerator FadeInCutscene()
    {   
        inCutscene = true;
        cutsceneImage.color = new Color(1f, 1f, 1f, 0);
        cutsceneImage.enabled = true;

        float elapsed = 0f;
        while(elapsed <= fadeTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeTime; //this is percentage done
            cutsceneImage.color = new Color(1f, 1f, 1f, t);
            yield return null;
        }
        cutsceneImage.color = new Color(1f, 1f, 1f, 1f);
        print("starting cutscenefadeouttimer");
        StartCoroutine(CutsceneFadeOutTimer());

    }


    private IEnumerator CutsceneFadeOutTimer()
    {
        yield return new WaitForSeconds(cutsceneDuration);
        print("calling exitcutscene");
        ExitCutscene();
    }

    public void ExitCutscene()
    {
        print("starting fadeoutcutscene");
        StartCoroutine(FadeOutCutscene());
        
    }

    private IEnumerator FadeOutCutscene()
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

        //small delay before going back to game to let the emotions of reid's amazing writing sink in
        yield return new WaitForSeconds(1.5f);
        //now fade out the black image

        elapsed = 0;
        while(elapsed <= fadeTime)
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

    private void PauseGame()
    {
        //everything we need to do to stop the game from functioning, do here
        OrbitManager.Instance.PauseOrbit();
    }

    private void ResumeGame()
    {
        //literally just undo everything we did in pause
        OrbitManager.Instance.ResumeOrbit();
    }

}
