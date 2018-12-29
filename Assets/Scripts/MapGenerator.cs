using System;
using System.IO;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private int col, row;
    [SerializeField]

    private GameObject wall;
    [SerializeField]

    private GameObject floor;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject mummyAsset;

    [SerializeField]
    private GameObject triggerAsset;

    private WhiteMummy[] whiteMummies;

    [SerializeField]
    private int level;


    private Vector2Int playerPos;
    private Vector2Int[] mummiesPos;
    private Vector2Int goalPos;
    [SerializeField]
    private float size = Constant.Size;
    private MazeCell[,] maze;

    // Map Data: Store as (Type North, Type West, Type South, Type East)
    // As Type is the type of the wall, Type = 0 mean no wall
    private int[,,] mapData;

    // Start is called before the first frame update
    void Start()
    {
        ReadMetadata();
        GeneratePlayerAndEnemies();
        InitializeMaze();
    }

    private void GeneratePlayerAndEnemies()
    {
        player.transform.position = new Vector3(playerPos[1] * size * 10, 10, playerPos[0] * size * 10);
        WorldManager.player = player;

        // Let the mummy see the map!
        WorldManager.mapData = WhiteMummy.mapData = mapData;

        for (int i = 0; i < whiteMummies.Length; i++)
        {
            whiteMummies[i] = new WhiteMummy();
            whiteMummies[i].SetPosition(mummiesPos[i], Instantiate(mummyAsset, Vector3.zero, Quaternion.identity));
            WorldManager.whiteMummies.Add(whiteMummies[i]);
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
        for(int i = 0; i < nMummies; i++)
        {
            int[] mummyPos = ReadLineToInts(reader);
            mummiesPos[i].x = mummyPos[1];
            mummiesPos[i].y = mummyPos[0];
        }

        for(int i=0;i<row;i++)
            for(int j = 0; j < col; j++)
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
        foreach(string s in bits)
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
        for (int i=0;i<row;i++)
            for(int j = 0; j < col; j++)
            {
                maze[i, j] = new MazeCell();
                maze[i, j].floor = Instantiate(floor, new Vector3(size * j * 10, 0, size * i * 10), Quaternion.identity);
                maze[i, j].floor.name = "Floor[" + i + "," + j + "]";
                maze[i, j].floor.transform.localScale *= 2;

                if(mapData[i, j, 3] != 0)
                {
                    maze[i, j].eastWall = Instantiate(wall, new Vector3(size * (j * 10 + 5), 5, size *(i * 10)), Quaternion.identity);
                    maze[i, j].eastWall.name = "EastDoor[" + i + "," + j + "]";
                    maze[i, j].eastWall.transform.Rotate(Vector3.up, 90f);
                    maze[i, j].eastWall.transform.localScale *= 2;
                }

                if(mapData[i, j, 1] != 0)
                {
                    maze[i, j].westWall = Instantiate(wall, new Vector3(size * (j * 10 - 5), 5, size * (i * 10)), Quaternion.identity);
                    maze[i, j].westWall.name = "WestDoor[" + i + "," + j + "]";
                    maze[i, j].westWall.transform.Rotate(Vector3.up, 90f);
                    maze[i, j].westWall.transform.localScale *= 2;
                }

                if(mapData[i, j, 0] != 0)
                {
                    maze[i, j].northWall = Instantiate(wall, new Vector3(size * (j * 10), 5, size * (i * 10 + 5)), Quaternion.identity);
                    maze[i, j].northWall.name = "NorthDoor[" + i + "," + j + "]";
                    maze[i, j].northWall.transform.localScale *= 2;
                }

                if(mapData[i, j, 2] != 0)
                {
                    maze[i, j].southWall = Instantiate(wall, new Vector3(size * (j * 10), 5, size * (i * 10 - 5)), Quaternion.identity);
                    maze[i, j].southWall.name = "SouthDoor[" + i + "," + j + "]";
                    maze[i, j].southWall.transform.localScale *= 2;
                }
            }
    }
}
