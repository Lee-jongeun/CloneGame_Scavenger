using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable] public class Count {
        public int minimum;
        public int maximum;

        public Count(int inputMax, int inputMin) {
            minimum = inputMin;
            maximum = inputMax;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] wallTiles;
    public GameObject[] floorTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] enemyTiles;
    public GameObject[] foodTiles;

    private Transform boardHolder;
    private List<Vector3> gridPostions = new List<Vector3>();

    void initalizeList() {
        gridPostions.Clear();

        for (int x = 1; x < columns - 1;  x++) {
            for (int y = 1; y < rows - 1; y++) {
                gridPostions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetUp() {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x <= columns; x++) {
            for (int y = -1; y <= rows; y++) {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition() {
        int randomIndex = Random.Range(0, gridPostions.Count);
        Vector3 randomPostion = gridPostions[randomIndex];
        gridPostions.RemoveAt(randomIndex);
        return randomPostion;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum) {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++) {
            Vector3 randomPostion = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate (tileChoice, randomPostion, Quaternion.identity);
        }
    }

    public void SetUpScene(int level) {
        BoardSetUp();
        initalizeList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}
