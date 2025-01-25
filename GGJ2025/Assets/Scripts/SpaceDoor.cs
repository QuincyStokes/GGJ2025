using UnityEngine;
using UnityEngine.UI;

public class SpaceDoor : MonoBehaviour
{

    //store position of room to be created on entry, should always be the same?

    [Header("Image Component")]
    public SpriteRenderer image;
    [HideInInspector]public int x;
    [HideInInspector]public int y;
    private bool hasBeenOpened;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //create new room
            if(!hasBeenOpened)
            {
                GridManager.Instance.GenerateNewRoom(x, y);
            }
            //in the future, this would trigger some animation
            image.enabled = false;

        }
    }

    void OnTriggerExit2D()
    {
        //in the future, would trigger some animation
        image.enabled = true;
    }
}
