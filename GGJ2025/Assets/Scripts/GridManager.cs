using System.Collections;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{   

    public static GridManager Instance;
    [Header("Generation Settings")]
    //ideally these are both even numbers
    public int roomSizeX;
    public int roomSizeY;
    public int outerTileThickness;
    public int maxRoomsX;
    public int maxRoomsY;
    private bool isCameraMoving;
    



    [Header("Tilemap References")]
    public Tilemap backgroundTM;
    public Tilemap obstacleTM;

    [Header("Space Tile References")]
    public Tile[] spaceWallTiles;
    public Tile[] spaceGroundTiles;


    [Header("Ship Tile References")]
    public Tile[] shipWallTiles;
    public Tile[] shipGroundTiles;

    [Header("Specific Tiles")]
    public Tile OUTERTile;

    [Header("Game Objects")]
    public GameObject door;

    [Header("Obstacle Spawning")]
    public float obstacleSpawnChancePerTile;
    public GameObject[] obstacles;

    [Header("Camera Settings")]
    public float cameraMoveDuration;
    [HideInInspector]public CinemachineVirtualCamera cam;
    
    [HideInInspector]public int currentCamX;
    [HideInInspector]public int currentCamY;


    private int[,] rooms;

    void InitializeRoomsArray()
    {
        rooms = new int[maxRoomsX, maxRoomsY];
        for(int i = 0; i < maxRoomsX; i++)
        {
            for(int j = 0; j < maxRoomsY; j++)
            {
                rooms[i, j] = 0;
            }
        }

    }

    
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
        

        if(cam == null)
        {
            cam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        }
        InitializeRoomsArray();
        MoveCameraPos(0, 0);
        GenerateNewRoom(0, 0);
        DontDestroyOnLoad(this.gameObject);

    }   


    public void GenerateNewRoom(int roomX, int roomY)
    {
        int startPosX = roomX*roomSizeX;
        int startPosY = roomY*roomSizeY;

        //place wall and background tiles
        for(int i = startPosX ; i < startPosX+roomSizeX; i++)
        {
            for(int j = startPosY; j < startPosY+roomSizeY; j++)
            {
                //place background tiles as a grid
                
                //not even gonna worry about the outer tiles rn, should probably just not have em
                if(i == startPosX || i == startPosX+roomSizeX-1 || j == startPosY || j == startPosY+roomSizeY-1)
                {
                    PlaceTile(i, j, spaceWallTiles[Random.Range(0, spaceWallTiles.Count())], obstacleTM);
                }
                else
                {
                    //place background (floor) tiles
                    PlaceTile(i, j, spaceGroundTiles[Random.Range(0, spaceGroundTiles.Count())], backgroundTM);

                    //here is a good place to spawn in random gameobjects since we have the position stored
                    //this just checks if we are away from the walls
                    if(i > startPosX+2 && i < startPosX+roomSizeX-3 && j > startPosY+2 && j < startPosY+roomSizeY-3)
                    {
                        if(Random.Range(0, 100) < obstacleSpawnChancePerTile)
                        {
                            Instantiate(obstacles[Random.Range(0, obstacles.Count())], new Vector3(i, j, -1), Quaternion.identity);
                        }
                    }
                    
                }
            }
        }

        //placing doors
        if(roomX > 0 )
        {
            if(rooms[roomX-1,roomY] == 0) // LEFT
            {
                Debug.Log($"placing LEFT door with NEXT room xy: {roomX-1}, {roomY}, and OLD room xy {roomX}, {roomY}");
                PlaceDoor(startPosX, startPosY +roomSizeY/2, roomX-1, roomY, roomX, roomY);
            }
            else
            {
                KillTile(startPosX, startPosY+roomSizeY/2, obstacleTM);
                KillTile(startPosX, startPosY+roomSizeY/2-1, obstacleTM);
                KillTile(startPosX-1, startPosY+roomSizeY/2, obstacleTM);
                KillTile(startPosX-1, startPosY+roomSizeY/2-1, obstacleTM);
            }
        }
        if(roomX < maxRoomsX-1) //RIGHT
        {
            if(rooms[roomX+1, roomY] == 0) 
            {
                Debug.Log($"placing RIGHT door with NEXT room xy: {roomX+1}, {roomY}, and OLD room xy {roomX}, {roomY}");
                PlaceDoor(startPosX + roomSizeX, startPosY +roomSizeY/2, roomX+1, roomY, roomX, roomY);
            }
            else
            {
                KillTile(startPosX+roomSizeX-1, startPosY+roomSizeY/2, obstacleTM);
                KillTile(startPosX+roomSizeX-1, startPosY+roomSizeY/2-1, obstacleTM);
                KillTile(startPosX+roomSizeX, startPosY+roomSizeY/2, obstacleTM);
                KillTile(startPosX+roomSizeX, startPosY+roomSizeY/2-1, obstacleTM);
            }
        }
        if(roomY > 0 ) //DOWN
        {
            if(rooms[roomX, roomY-1] == 0)
            {
                Debug.Log($"placing DOWN door with NEXT room xy: {roomX}, {roomY-1}, and OLD room xy {roomX}, {roomY}");
                PlaceDoor(startPosX +roomSizeX/2, startPosY, roomX, roomY-1, roomX, roomY);
            }
            else
            {
                KillTile(startPosX+roomSizeX/2, startPosY, obstacleTM);
                KillTile(startPosX+roomSizeX/2-1, startPosY, obstacleTM);
                KillTile(startPosX+roomSizeX/2, startPosY-1, obstacleTM);
                KillTile(startPosX+roomSizeX/2-1, startPosY-1, obstacleTM);
            }
        }
        if(roomY < maxRoomsY-1)//UP
        {
            if(rooms[roomX, roomY+1] == 0 ) 
            {
                Debug.Log($"placing UP door with NEXT room xy: {roomX}, {roomY+1}, and OLD room xy {roomX}, {roomY}");
                PlaceDoor(startPosX +roomSizeX/2, startPosY +roomSizeY, roomX, roomY+1, roomX, roomY);
            }  
            else
            {
                KillTile(startPosX+roomSizeX/2, startPosY +roomSizeY, obstacleTM);
                KillTile(startPosX+roomSizeX/2-1, startPosY +roomSizeY, obstacleTM);
                KillTile(startPosX+roomSizeX/2, startPosY-1 +roomSizeY, obstacleTM);
                KillTile(startPosX+roomSizeX/2-1, startPosY-1 +roomSizeY, obstacleTM);
            }
        }
        rooms[roomX, roomY] = 1;
        

        //spawn enemies in that room
        if(roomX != 0 && roomY != 0)
        {
            //EnemySpawnManager.Instance.SpawnEnemies();
        }
        

    }
    public void PlaceDoor(int x, int y, int nextRoomX, int nextRoomY, int oldRoomX, int oldRoomY)
    {
        GameObject newDoor = Instantiate(door, new Vector3(x, y, 0), Quaternion.identity);
        SpaceDoor sd = newDoor.GetComponent<SpaceDoor>();
        sd.newRoomX = nextRoomX;
        sd.newRoomY = nextRoomY;
        sd.oldRoomX = oldRoomX;
        sd.oldRoomY = oldRoomY;
        KillTile(x, y, obstacleTM);
        KillTile(x-1, y, obstacleTM);
        KillTile(x, y-1, obstacleTM);
        KillTile(x-1, y-1, obstacleTM);
    }

    public void PlaceTile( int x, int y, Tile tile, Tilemap tilemap)
    {
        tilemap.SetTile(new Vector3Int(x, y, 0), tile);
    }

    public void KillTile(int x, int y, Tilemap tilemap)
    {
        tilemap.SetTile(new Vector3Int(x, y, 0), null);
    }

    //placing gameobjects on the tilemap should be easy, (0,0) is pretty much 0,0, BUT center of origin tile is (.5, .5)

    public void PlaceGameObject(int x, int y, GameObject go)
    {
        Instantiate(go, new Vector3(x+.5f, y+.5f, 0), Quaternion.identity);
    }


    //MoveCameraPos sets the position of a camera to a Room X and Y
    public void MoveCameraPos(int x, int y)
    {
        print("moving camera position to " + x + ", " +  y);
        currentCamX = x;
        currentCamY = y;
        StartCoroutine(MoveCameraSmooth(x, y));
        cam.transform.position = new Vector3(x*roomSizeX + roomSizeX/2, y*roomSizeY + roomSizeY/2, -10);
    }

    private IEnumerator MoveCameraSmooth(int x, int y)
    {
        isCameraMoving = true;
        Vector3 startPos = cam.transform.position;
        Vector3 endPos = new Vector3(x*roomSizeX + roomSizeX/2, y*roomSizeY + roomSizeY/2, -10);

        float elapsedTime = 0f;
        while(elapsedTime < cameraMoveDuration)
        {
            elapsedTime += Time.deltaTime;

            //t is the percentage of the way to the new location, goes from 0-1
            float t = Mathf.Clamp01(elapsedTime / cameraMoveDuration); 

            //apply the position change using Lerp, makes it smooooth
            cam.transform.position = Vector3.Lerp(startPos, endPos, t);
            
            //this basically tells the loop to wait to continue until the next frame
            yield return null;
        }

        cam.transform.position = endPos;
        isCameraMoving = false;

    }


}
