using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainController : MonoBehaviour
{
    [SerializeField] GameObject sign, NPC;
    [SerializeField] Vector2 signNumRange = new Vector2(10, 20);

    [HideInInspector] public bool generatePointsOfInterest;
    [HideInInspector] public List<GameObject> pointsOfInterest;
    [HideInInspector] public float bufferDistance;
    [HideInInspector] public GameObject spawnedNPC;

    Transform terrainTransform;

    public void Start()
    {
        terrainTransform = transform.GetChild(0).transform;
        float half = (terrainTransform.localScale.x / 2) * 10;

        if (true)
        {
            spawnedNPC = Instantiate(
                NPC, 
                new Vector3(
                    transform.position.x + Random.Range(-half, half),
                    0,
                    transform.position.z + Random.Range(-half, half)),
                Quaternion.Euler(0, 180, 0),
                transform);
        
            pointsOfInterest.Add(spawnedNPC);
        
            for (int i = 0; i < Random.Range(signNumRange.x, signNumRange.y); i++)
                GenerateSign(half);   
        }
    }

    void GenerateSign(float max)
    {
        GameObject temp = Instantiate(
            sign, 
            new Vector3(
                transform.position.x + Random.Range(-max, max),
                0,
                transform.position.z + Random.Range(-max, max)),
            Quaternion.identity,
            transform);

        if (pointsOfInterest != null) pointsOfInterest = new List<GameObject>();

        for (int i = 0; i < pointsOfInterest.Count; i++)
        {
            if (Vector3.Distance(temp.transform.position, pointsOfInterest[i].transform.position) <= bufferDistance)
            {
                Destroy(temp);   
            }
            else
            {
                pointsOfInterest.Add(temp);
            }
        }
    }
}