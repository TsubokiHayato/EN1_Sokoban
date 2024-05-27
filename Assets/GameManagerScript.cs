using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

//Object Instantiate(
//    Object original,
//    Vector3 pos,
//    Quaternion rotation
//);

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject boxPrefab;

    int[,] map;

    GameObject[,] field;




    void PrintArray()
    {
        string debugText = "";

        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                debugText += map[y,x].ToString() + ",";

            }
        }
        Debug.Log(debugText);
    }

    Vector2Int GetPlayerIndex()
    {

        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y, x] == null) { continue; }
                if (field[y, x].tag == "Player") { return new Vector2Int(x, y); }

            }
        }
        return new Vector2Int(-1, -1);
    }

    bool MoveNumber(Vector2Int moveFrom, Vector2Int moveTo)
    {
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }


        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;

            bool success = MoveNumber(moveTo, moveTo + velocity);
            if (!success)
            {
                return false;
            }
        }

        field[moveTo.y, moveTo.x].transform.position =
            new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;


        return true;


    }
    // Start is called before the first frame update
    void Start()
    {

        map = new int[,] {
            {0,2,0,0,0},
            {0,2,1,0,0},
            {0,2,0,0,0},
        };

        field = new GameObject
            [
            map.GetLength(0),
            map.GetLength(1)
            ];



        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y,x] == 1)
                {
                    field[y, x] = Instantiate(
                            playerPrefab,
                            new Vector3(x, map.GetLength(0) - y, 0),
                            Quaternion.identity
                            );
                }
                if(map[y,x] == 2)
                {
                    field[y, x] = Instantiate(
                        boxPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity
                        );
                }

            }

        }





        //PrintArray();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {

            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber(playerIndex, playerIndex+new Vector2Int(1,0));

            Debug.Log(playerIndex.x);
            Debug.Log(playerIndex.y);
        }


        if (Input.GetKeyDown(KeyCode.A))
        {

            Vector2Int playerIndex = GetPlayerIndex();


            MoveNumber( playerIndex, playerIndex + new Vector2Int(-1, 0));

            Debug.Log(playerIndex.x);
            Debug.Log(playerIndex.y);
        }

    }
}
