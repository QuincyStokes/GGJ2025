using UnityEngine;
using UnityEngine.UI;

public class SpaceDoor : MonoBehaviour
{

    //store position of room to be created on entry, should always be the same?

    [Header("Image Component")]
    public SpriteRenderer image;
    [HideInInspector]public int newRoomX; //x of the room that this door created
    [HideInInspector]public int newRoomY; //y of the room that this door created

    [HideInInspector]public int oldRoomX; //x of the room that was already there
    [HideInInspector]public int oldRoomY; //y of the room that aas already there
    private bool hasBeenOpened = false;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //create new room
            if(!hasBeenOpened)
            {
                GridManager.Instance.GenerateNewRoom(newRoomX, newRoomY);
                hasBeenOpened = true;
            }
           
            //in the future, this would trigger some animation
            image.enabled = false;
            
            //need to move the camera to the appropriate position
            //if our current room is not the new room, then we must be moving to it..?
            //                      0 !=  1  &&   0 !=  0
            if(GridManager.Instance.currentCamX != newRoomX || GridManager.Instance.currentCamY != newRoomY)
            {
                GridManager.Instance.MoveCameraPos(newRoomX, newRoomY);
            }
            else
            {
                GridManager.Instance.MoveCameraPos(oldRoomX, oldRoomY);
            }

        }
    }

    void OnTriggerExit2D()
    {
        //in the future, would trigger some animation
        image.enabled = true;
    }
}
