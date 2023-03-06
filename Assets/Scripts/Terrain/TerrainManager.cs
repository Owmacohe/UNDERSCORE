using System;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    [Header("FLoor")]
    [SerializeField] GameObject terrain;
    [SerializeField] float terrainSize = 5;
    [SerializeField] Vector2 generateSize = new Vector2(5, 5);

    [Header("Points Of Interest")]
    [SerializeField] bool generatePointsOfInterest;
    [SerializeField] float bufferDistance = 3;
    [SerializeField] PathChecker checker;

    List<Vector2> coordinates;
    List<Vector2> hasBeenGeneratedAround;

    GameObject player;
    GameObject[] allNPCs;
    List<GameObject> pointsOfInterest;

    TerrainController lastSpawnedTerrain;

    void Start()
    {
        coordinates = new List<Vector2>();
        hasBeenGeneratedAround = new List<Vector2>();
        
        allNPCs = GameObject.FindGameObjectsWithTag("NPC");
        player = GameObject.FindWithTag("Player");

        pointsOfInterest = new List<GameObject>();
        
        foreach (GameObject i in allNPCs)
            pointsOfInterest.Add(i);
        
        pointsOfInterest.Add(player);

        GenerateAround(0, 0);
    }

    void Update()
    {
        if (generatePointsOfInterest && lastSpawnedTerrain != null)
        {
            checker.CheckAndDelete(
                player.transform.position,
                lastSpawnedTerrain.spawnedNPC.transform.position,
                lastSpawnedTerrain);
            
            lastSpawnedTerrain = null;
        }
    }

    void Generate(int x, int y)
    {
        if (!coordinates.Contains(new Vector2(x, y)))
        {
            coordinates.Add(new Vector2(x, y));

            if (x == 0 && y == 0) return;
            
            TerrainController temp = Instantiate(
                terrain,
                new Vector3(x * terrainSize * 10, 0, y * terrainSize * 10),
                Quaternion.identity,
                transform).GetComponent<TerrainController>();

            temp.generatePointsOfInterest = generatePointsOfInterest;
            temp.pointsOfInterest = new List<GameObject>(pointsOfInterest);
            temp.bufferDistance = bufferDistance;

            lastSpawnedTerrain = temp;
        }
    }
    
    public void GenerateAround(Vector3 pos)
    {
        GenerateAround((int)(pos.x / (terrainSize * 10)), (int)(pos.z / (terrainSize * 10)));
    }

    void GenerateAround(int x, int y)
    {
        for (int i = x + -(int) (generateSize.x / 2); i <= x + (int) (generateSize.x / 2); i++)
        {
            for (int j = y + -(int) (generateSize.y / 2); j <= y + (int) (generateSize.y / 2); j++)
            {
                if (!hasBeenGeneratedAround.Contains(new Vector2(i, j)))
                {
                    Generate(i, j);
                    hasBeenGeneratedAround.Add(new Vector2(i, j));   
                }
            }
        }
    }
}