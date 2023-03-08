using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainController : MonoBehaviour
{
    [SerializeField] GameObject sign, NPC;
    [SerializeField] Vector2 signNumRange = new Vector2(60, 60);

    [HideInInspector] public List<GameObject> pointsOfInterest;
    [HideInInspector] public float bufferDistance;
    [HideInInspector] public GameObject spawnedNPC;
    [HideInInspector] public Transform lastExtraNpcTransform;
    
    Transform terrainTransform, playerTransform;

    public void Generate(bool generatePointsOfInterest, bool extraNPC, NPCController.NPCInformation extraInfo)
    {
        terrainTransform = transform.GetChild(0).transform;
        float half = (terrainTransform.localScale.x / 2) * 10;

        Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;

        if (generatePointsOfInterest)
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
            {
                GameObject temp = GenerateSign(half);
                Vector3 pos = temp.transform.position;

                if (temp != null &&
                    (Vector3.Distance(spawnedNPC.transform.position, pos) <= 5 ||
                    Vector3.Distance(playerPos, pos) <= 10))
                    Destroy(temp);
            }
        }

        if (extraNPC)
        {
            if (playerTransform == null) playerTransform = GameObject.FindWithTag("Player").transform;
            
            GameObject temp = Instantiate(
                NPC,
                new Vector3(
                    playerTransform.position.x + Random.Range(-half, half),
                    0,
                    playerTransform.position.z + Random.Range(0, half)),
                Quaternion.Euler(0, 180, 0),
                transform);

            lastExtraNpcTransform = temp.transform;

            if (extraInfo != null)
            {
                NPCController controller = temp.GetComponentInChildren<NPCController>();
                controller.info = extraInfo;
            }
        }
    }

    GameObject GenerateSign(float max)
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
                return null;
            }
            
            pointsOfInterest.Add(temp);
        }

        return temp;
    }
}