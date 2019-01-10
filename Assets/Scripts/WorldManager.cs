using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static bool firstRun = true;
    public static Vector2Int playerPos;
    public static List<WhiteMummy> whiteMummies = new List<WhiteMummy>();
    public static List<RedMummy> redMummies = new List<RedMummy>();
    public static List<Scorpion> scorpions = new List<Scorpion>();
    public static bool movingSomething = false;
    public static int[,,] mapData;
    public static GameObject endGameTrigger;
    private static MapGenerator mg;
    public static GameObject thePlayer;

    Quaternion rotation;

    // Map generator fields
    [SerializeField]
    private float size = Constant.Size;

    [SerializeField]
    private GameObject wall;

    [SerializeField]
    private GameObject floor;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject mummyAsset;

    [SerializeField]
    private GameObject redMummyAsset;

    [SerializeField]
    private GameObject scorpionAsset;

    [SerializeField]
    private GameObject triggerAsset;


    public void Start()
    {
        if (firstRun)
        {
            mg = gameObject.AddComponent<MapGenerator>();
            mg.SetUp(wall, floor, player, mummyAsset, redMummyAsset, scorpionAsset, triggerAsset);
            firstRun = false;
            GoToLevel(1);
        }
    }

    public static void RemoveMummy()
    {
        movingSomething = false;
        if (whiteMummies.Count + redMummies.Count + scorpions.Count < 2)
            return;
        for(int i = 0;i < whiteMummies.Count; i++)
        {
            if(i < whiteMummies.Count - 1)
            for (int j = i + 1; j < whiteMummies.Count; j++)
                if (whiteMummies[i].GetPosition() == whiteMummies[j].GetPosition())
                {
                    GameObject @object = whiteMummies[i].GetGameObject();
                    whiteMummies.RemoveAt(i);
                    Destroy(@object);
                    return;
                }

            for(int j=0;j<redMummies.Count;j++)
                if(whiteMummies[i].GetPosition() == redMummies[j].GetPosition())
                {
                    GameObject @object = whiteMummies[i].GetGameObject();
                    whiteMummies.RemoveAt(i);
                    Destroy(@object);
                    return;
                }

            for (int j = 0; j < scorpions.Count; j++)
                if (whiteMummies[i].GetPosition() == scorpions[j].GetPosition())
                {
                    GameObject @object = scorpions[j].GetGameObject();
                    scorpions.RemoveAt(j);
                    Destroy(@object);
                    return;
                }
        }

        for(int i = 0; i < redMummies.Count; i++)
        {
            if(i < redMummies.Count - 1)
                for(int j=i+1;j<redMummies.Count;j++)
                if(redMummies[i].GetPosition() == redMummies[j].GetPosition())
                    {
                        GameObject @object = redMummies[i].GetGameObject();
                        redMummies.RemoveAt(i);
                        Destroy(@object);
                        return;
                    }
            for(int j=0;j<scorpions.Count;j++)
                if(redMummies[i].GetPosition() == scorpions[j].GetPosition())
                {
                    GameObject @object = scorpions[j].GetGameObject();
                    scorpions.RemoveAt(j);
                    Destroy(@object);
                    return;
                }
        }

        for(int i = 0; i < scorpions.Count - 1; i++)
        {
            for(int j=i+1;j<scorpions.Count;j++)
                if(scorpions[i].GetPosition() == scorpions[j].GetPosition())
                {
                    GameObject @object = scorpions[j].GetGameObject();
                    scorpions.RemoveAt(j);
                    Destroy(@object);
                    return;
                }
        }
    }

    public void Update()
    {
        if (movingSomething)
            return;

        if (Input.GetAxis("Vertical") > 0 && playerCanMoveForward())
        {
            player.GetComponent<MovementComponent>().MoveForward();
            movingSomething = true;
        }
        else if (Input.GetAxis("Vertical") < 0 && playerCanMoveBackward())
        {
            player.GetComponent<MovementComponent>().MoveBackward();
            movingSomething = true;
        }
        else if (Input.GetAxis("Horizontal") > 0 && playerCanMoveRight())
        {
            player.GetComponent<MovementComponent>().MoveRight();
            movingSomething = true;
        }
        else if (Input.GetAxis("Horizontal") < 0 && playerCanMoveLeft())
        {
            player.GetComponent<MovementComponent>().MoveLeft();
            movingSomething = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
            MummyMove();
        // After player moved, the mummy move
    }

    public static void MummyMove()
    {
        foreach (WhiteMummy mummy in whiteMummies)
        {
            Debug.Log(playerPos);
            mummy.Move(playerPos);
        }
        foreach (Scorpion s in scorpions)
            s.Move(playerPos);
        foreach (RedMummy m in redMummies)
            m.Move(playerPos);
    }

    private bool playerCanMoveForward()
    {
        if (mapData[playerPos.y, playerPos.x, 0] == 1)
            return false;
        return true;
    }
    private bool playerCanMoveBackward()
    {
        // This check to make sure index is >= 0
        if (playerPos.y == 0)
            return false;
        if (mapData[playerPos.y - 1, playerPos.x, 0] == 1)
            return false;
        return true;
    }
    private bool playerCanMoveLeft()
    {
        // This check to make sure index is >= 0
        if (playerPos.x == 0)
            return false;
        if (mapData[playerPos.y, playerPos.x - 1, 3] == 1)
            return false;
        return true;
    }
    private bool playerCanMoveRight()
    {
        if (mapData[playerPos.y, playerPos.x, 3] == 1)
            return false;
        return true;
    }

    public static void GoToLevel(int level)
    {
        //EventSystem.current.SetSelectedGameObject(null);
        foreach (WhiteMummy mummy in whiteMummies)
            Destroy(mummy.GetGameObject());
        whiteMummies.Clear();

        if(thePlayer != null)
            thePlayer.GetComponent<MovementComponent>().StopMoving();
        endGameTrigger = null;
        mg.SetLevel(level);
    }

    public class MapGenerator : MonoBehaviour
    {
        private int col, row;
        //[SerializeField]

        private GameObject wall;
        //[SerializeField]

        private GameObject floor;

        //[SerializeField]
        private GameObject player;

        //[SerializeField]
        private GameObject mummyAsset;
        //[SerializeField]

        private GameObject redMummyAsset;
        //[SerializeField]

        private GameObject scorpionAsset;

        //[SerializeField]
        private GameObject triggerAsset;

        private WhiteMummy[] whiteMummies;
        private Scorpion[] scorpions;
        private RedMummy[] redMummies;
        [SerializeField]
        private int level;


        private Vector2Int playerPos;
        private Vector2Int[] mummiesPos;
        private Vector2Int[] redMummiesPos;
        private Vector2Int[] scorpionsPos;
        private Vector2Int goalPos;
        [SerializeField]
        private float size = Constant.Size;
        private MazeCell[,] maze;

        // Map Data: Store as (Type North, Type West, Type South, Type East)
        // As Type is the type of the wall, Type = 0 mean no wall
        private int[,,] mapData;

        private List<GameObject> GeneratedGameObjects = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            //ReadMetadata();
            //GeneratePlayerAndEnemies();
            //InitializeMaze();
        }

        public void SetUp(GameObject wall, GameObject floor, GameObject player, GameObject mummyAsset, GameObject redMummyAsset, GameObject scorpionAsset, GameObject triggerAsset)
        {
            this.wall = wall;
            this.floor = floor;
            this.player = player;
            this.mummyAsset = mummyAsset;
            this.triggerAsset = triggerAsset;
            this.redMummyAsset = redMummyAsset;
            this.scorpionAsset = scorpionAsset;
        }

        public void SetLevel(int index)
        {
            // Clear the old map assets
            for (int i = 0; i < GeneratedGameObjects.Count; i++)
                Destroy(GeneratedGameObjects[i]);

            level = index;
            ReadMetadata();
            GeneratePlayerAndEnemies();
            InitializeMaze();
        }

        private void GeneratePlayerAndEnemies()
        {
            if(thePlayer == null)
                player.transform.position = new Vector3(playerPos[1] * size * 10, 10, playerPos[0] * size * 10);
            if (thePlayer == null)
                thePlayer = player;
            WorldManager.playerPos.x = playerPos[1];
            WorldManager.playerPos.y = playerPos[0];
            WorldManager.thePlayer.transform.position = new Vector3(playerPos[1] * size * 10, 10, playerPos[0] * size * 10);

            // Let the mummy see the map!
            WorldManager.mapData = Scorpion.mapData = RedMummy.mapData = WhiteMummy.mapData = mapData;

            for (int i = 0; i < whiteMummies.Length; i++)
            {
                whiteMummies[i] = new WhiteMummy();
                GameObject gm = Instantiate(mummyAsset, Vector3.zero, Quaternion.identity);
                whiteMummies[i].SetPosition(mummiesPos[i], gm);
                WorldManager.whiteMummies.Add(whiteMummies[i]);
            }

            for (int i = 0; i < redMummies.Length; i++)
            {
                redMummies[i] = new RedMummy();
                GameObject gm = Instantiate(redMummyAsset, Vector3.zero, Quaternion.identity);
                redMummies[i].SetPosition(redMummiesPos[i], gm);
                WorldManager.redMummies.Add(redMummies[i]);
            }

            for (int i = 0; i < scorpions.Length; i++)
            {
                scorpions[i] = new Scorpion();
                GameObject gm = Instantiate(scorpionAsset, Vector3.zero, Quaternion.identity);
                scorpions[i].SetPosition(scorpionsPos[i], gm);
                WorldManager.scorpions.Add(scorpions[i]);
            }

            //player.GetComponent<PlayerCollider>().mummies = whiteMummies;
        }


        private void ReadMetadata()
        {
            string fileName = "level" + level.ToString("D3") + ".txt";
            string path = "Assets/ScenesData/" + fileName;

            StreamReader reader = File.OpenText(path);

            int[] rowData = ReadLineToInts(reader);
            row = rowData[0];
            col = rowData[1];
            mapData = new int[row, col, 4];

            int[] escapePos = ReadLineToInts(reader);
            int[] tempPlayerPos = ReadLineToInts(reader);
            playerPos.x = tempPlayerPos[1];
            playerPos.y = tempPlayerPos[0];

            int nMummies = ReadLineToInts(reader)[0];
            whiteMummies = new WhiteMummy[nMummies];
            mummiesPos = new Vector2Int[nMummies];
            for (int i = 0; i < nMummies; i++)
            {
                int[] mummyPos = ReadLineToInts(reader);
                mummiesPos[i].x = mummyPos[1];
                mummiesPos[i].y = mummyPos[0];
            }

            int nRedMummies = ReadLineToInts(reader)[0];
            redMummies = new RedMummy[nRedMummies];
            redMummiesPos = new Vector2Int[nRedMummies];
            for (int i = 0; i < nRedMummies; i++)
            {
                int[] mummyPos = ReadLineToInts(reader);
                redMummiesPos[i].x = mummyPos[1];
                redMummiesPos[i].y = mummyPos[0];
            }

            int nScorpions = ReadLineToInts(reader)[0];
            scorpions = new Scorpion[nScorpions];
            scorpionsPos = new Vector2Int[nScorpions];
            for (int i = 0; i < nScorpions; i++)
            {
                int[] scorpionPos = ReadLineToInts(reader);
                scorpionsPos[i].x = scorpionPos[1];
                scorpionsPos[i].y = scorpionPos[0];
            }

            for (int i = 0; i < row; i++)
                for (int j = 0; j < col; j++)
                {
                    rowData = ReadLineToInts(reader);
                    mapData[i, j, 0] = rowData[0];
                    mapData[i, j, 3] = rowData[1];

                    if (i == 0)
                        mapData[i, j, 2] = 1;
                    if (i == row - 1)
                        mapData[i, j, 0] = 1;
                    if (j == 0)
                        mapData[i, j, 1] = 1;
                    if (j == col - 1)
                        mapData[i, j, 3] = 1;
                }

            mapData[escapePos[0], escapePos[1], escapePos[2]] = 0;
            goalPos.x = escapePos[1];
            goalPos.y = escapePos[0];
        }

        private string ReadLine(StreamReader reader)
        {
            string ans;
            do
            {
                ans = reader.ReadLine();
            } while (ans[0] == '%');
            return ans;
        }

        private int[] ReadLineToInts(StreamReader reader)
        {
            string text = ReadLine(reader);
            string[] bits = text.Split(' ');
            int[] ans = new int[bits.Length];
            int index = 0;
            foreach (string s in bits)
            {
                ans[index] = int.Parse(s);
                index++;
            }
            return ans;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void InitializeMaze()
        {
            maze = new MazeCell[row, col];

            WorldManager.endGameTrigger = Instantiate(triggerAsset, new Vector3(size * goalPos.x * 10, 0, size * goalPos.y * 10), Quaternion.identity);
            WorldManager.endGameTrigger.transform.localScale *= 2;
            GeneratedGameObjects.Add(endGameTrigger);

            for (int i = 0; i < row; i++)
                for (int j = 0; j < col; j++)
                {
                    maze[i, j] = new MazeCell();
                    maze[i, j].floor = Instantiate(floor, new Vector3(size * j * 10, 0, size * i * 10), Quaternion.identity);
                    maze[i, j].floor.name = "Floor[" + i + "," + j + "]";
                    maze[i, j].floor.transform.localScale *= 2;
                    GeneratedGameObjects.Add(maze[i, j].floor);

                    if (mapData[i, j, 3] != 0)
                    {
                        maze[i, j].eastWall = Instantiate(wall, new Vector3(size * (j * 10 + 5), 5, size * (i * 10)), Quaternion.identity);
                        maze[i, j].eastWall.name = "EastDoor[" + i + "," + j + "]";
                        maze[i, j].eastWall.transform.Rotate(Vector3.up, 90f);
                        maze[i, j].eastWall.transform.localScale *= 2;
                        GeneratedGameObjects.Add(maze[i, j].eastWall);
                    }

                    if (mapData[i, j, 1] != 0)
                    {
                        maze[i, j].westWall = Instantiate(wall, new Vector3(size * (j * 10 - 5), 5, size * (i * 10)), Quaternion.identity);
                        maze[i, j].westWall.name = "WestDoor[" + i + "," + j + "]";
                        maze[i, j].westWall.transform.Rotate(Vector3.up, 90f);
                        maze[i, j].westWall.transform.localScale *= 2;
                        GeneratedGameObjects.Add(maze[i, j].westWall);
                    }

                    if (mapData[i, j, 0] != 0)
                    {
                        maze[i, j].northWall = Instantiate(wall, new Vector3(size * (j * 10), 5, size * (i * 10 + 5)), Quaternion.identity);
                        maze[i, j].northWall.name = "NorthDoor[" + i + "," + j + "]";
                        maze[i, j].northWall.transform.localScale *= 2;
                        GeneratedGameObjects.Add(maze[i, j].northWall);
                    }

                    if (mapData[i, j, 2] != 0)
                    {
                        maze[i, j].southWall = Instantiate(wall, new Vector3(size * (j * 10), 5, size * (i * 10 - 5)), Quaternion.identity);
                        maze[i, j].southWall.name = "SouthDoor[" + i + "," + j + "]";
                        maze[i, j].southWall.transform.localScale *= 2;
                        GeneratedGameObjects.Add(maze[i, j].southWall);
                    }
                }
        }
    }
}
