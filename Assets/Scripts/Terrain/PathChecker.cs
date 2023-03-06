using System;
using System.Collections.Generic;
using UnityEngine;

public class PathChecker : MonoBehaviour
{
    [SerializeField] float minDistance = 6;
    [SerializeField] int checks = 5;
    [SerializeField] LayerMask signLayer;
    [SerializeField] PathLightManager plm;

    GameObject[] NPCs;

    int start = 0;
    int end = 1;
    List<GameObject> path = new List<GameObject>();

    void Start()
    {
        Invoke(nameof(CheckAndDelete), 0.01f);
    }

    public void GenerateNextPathLights()
    {
        plm.Generate(path[start++].transform.position, path[end++].transform.position);
    }

    void CheckAndDelete()
    {
        NPCs = GameObject.FindGameObjectsWithTag("NPC");

        GameObject player = GameObject.FindWithTag("Player");
        path.Add(player);
        
        Vector3 previous = player.transform.position;
        GameObject next = FindClosestNPC(previous);

        while (next != null)
        {
            Vector3 p = previous;
            Vector3 n = next.transform.position;

            CheckAndDelete(p, n);

            Vector3 temp = new Vector3(n.x, n.y, n.z);
            next = FindClosestNPC(previous);
            previous = temp;
        }
        
        GenerateNextPathLights();
    }

    public void CheckAndDelete(Vector3 a, Vector3 b, TerrainController terrain = null)
    {
        for (int i = 0; i < checks; i++)
        {
            if (Physics.SphereCast(
                    a, 
                    minDistance,
                    (b - a).normalized,
                    out var hit,
                    Single.PositiveInfinity,
                    signLayer))
            {
                GameObject temp = hit.transform.parent.gameObject;

                if (terrain == null || terrain.pointsOfInterest.Contains(temp)) Destroy(temp);
            }   
        }
    }

    GameObject FindClosestNPC(Vector3 pos)
    {
        float closestDistance = Single.PositiveInfinity;
        GameObject closest = null;

        foreach (GameObject i in NPCs)
        {
            if (!path.Contains(i))
            {
                float distance = Vector3.Distance(i.transform.position, pos);
                
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = i;
                }
            }
        }
        
        if (closest != null) path.Add(closest);

        return closest;
    }
}