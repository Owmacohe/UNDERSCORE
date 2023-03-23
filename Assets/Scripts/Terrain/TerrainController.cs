using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainController : MonoBehaviour
{
    [SerializeField] GameObject NPC;
    [SerializeField] GameObject snow;

    [HideInInspector] public List<GameObject> pointsOfInterest;
    [HideInInspector] public float bufferDistance;
    [HideInInspector] public GameObject spawnedNPC;
    [HideInInspector] public Transform lastExtraNpcTransform;
    
    Transform terrainTransform, playerTransform;

    public void Generate(
        bool generatePointsOfInterest,
        bool generateNPCs,
        float bufferFromCentre,
        GameObject POI,
        Vector2 POIRange,
        bool NPCFollow,
        bool extraNPC,
        NPCController.NPCInformation extraInfo,
        bool generateSnow = false)
    {
        terrainTransform = transform.GetChild(0).transform;
        float half = (terrainTransform.localScale.x / 2) * 10;

        Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;

        if (generatePointsOfInterest)
        {
            if (generateNPCs)
            {
                Vector3 NPCpos = new Vector3(
                    transform.position.x + Random.Range(-half, half),
                    0,
                    transform.position.z + Random.Range(-half, half));

                if (Vector3.Distance(NPCpos, Vector3.zero) > bufferFromCentre)
                {
                    spawnedNPC = Instantiate(NPC, NPCpos, Quaternion.Euler(0, 180, 0), transform);
                    spawnedNPC.GetComponent<NPCController>().followPlayer = NPCFollow;
                
                    pointsOfInterest.Add(spawnedNPC);
                }   
            }

            for (int i = 0; i < Random.Range(POIRange.x, POIRange.y); i++)
            {
                GameObject temp = GenerateSign(POI, half);
                Vector3 pos = temp.transform.position;

                if (temp != null &&
                    (((spawnedNPC != null && Vector3.Distance(spawnedNPC.transform.position, pos) <= 5) ||
                    Vector3.Distance(playerPos, pos) <= 10) ||
                    Vector3.Distance(pos, Vector3.zero) <= bufferFromCentre))
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
                controller.followPlayer = NPCFollow;
            }
        }

        if (generateSnow)
        {
            Instantiate(snow, transform);
        }
    }

    GameObject GenerateSign(GameObject POI, float max)
    {
        GameObject temp = Instantiate(
            POI, 
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