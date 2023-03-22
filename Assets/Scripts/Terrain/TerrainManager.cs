using System;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    [Header("Floor")]
    [SerializeField] GameObject terrain;
    [SerializeField] Material material;
    [SerializeField] float terrainSize = 5;
    [SerializeField] Vector2 generateSize = new Vector2(8, 8);
    [SerializeField] Material secondaryLayer;

    [Header("Points Of Interest")]
    [SerializeField] GameObject pointOfInterest;
    [SerializeField] Vector2 pointOfInterestRange = new Vector2(60, 60);
    [SerializeField] bool generatePointsOfInterest;
    [SerializeField] float bufferDistanceFromCentre;
    [SerializeField] float bufferDistance = 3;
    [SerializeField] PathChecker checker;
    [SerializeField] bool generateSnow;
    [SerializeField] bool NPCFollow;

    List<Vector2> coordinates;
    List<Vector2> hasBeenGeneratedAround;

    GameObject player;
    GameObject[] allNPCs;
    List<GameObject> pointsOfInterest;

    TerrainController lastSpawnedTerrain;

    [HideInInspector] public Vector2 currentTerrain;
    [HideInInspector] public bool generateExtraNPC;
    [HideInInspector] public NPCController.NPCInformation extraNPCInfo;
    [HideInInspector] public Transform lastExtraNpcTransform;

    void Start()
    {
        Destroy(transform.GetChild(0).gameObject);
        
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
            
            TerrainController temp = Instantiate(
                terrain,
                new Vector3(x * terrainSize * 10, 0, y * terrainSize * 10),
                Quaternion.identity,
                transform).GetComponent<TerrainController>();

            if (material != null) temp.GetComponentInChildren<MeshRenderer>().material = material;

            temp.pointsOfInterest = new List<GameObject>(pointsOfInterest);
            temp.bufferDistance = bufferDistance;

            temp.Generate(
                generatePointsOfInterest,
                bufferDistanceFromCentre,
                pointOfInterest,
                pointOfInterestRange,
                NPCFollow,
                generateExtraNPC,
                extraNPCInfo,
                generateSnow);
            generateExtraNPC = false;

            lastSpawnedTerrain = temp;
            if (temp.lastExtraNpcTransform != null) lastExtraNpcTransform = temp.lastExtraNpcTransform;

            if (secondaryLayer != null)
            {
                Instantiate(
                    terrain,
                    new Vector3(x * terrainSize * 10, -3, y * terrainSize * 10),
                    Quaternion.identity,
                    transform)
                    .GetComponentInChildren<MeshRenderer>().material = secondaryLayer;
            }
        }
    }
    
    public void GenerateAround(Vector3 pos)
    {
        GenerateAround((int)(pos.x / (terrainSize * 10)), (int)(pos.z / (terrainSize * 10)));
    }

    void GenerateAround(int x, int y)
    {
        currentTerrain = new Vector2(x, y);
        
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