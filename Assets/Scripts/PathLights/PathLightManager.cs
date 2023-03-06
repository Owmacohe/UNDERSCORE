using UnityEngine;

public class PathLightManager : MonoBehaviour
{
    [SerializeField] GameObject pathLight;
    [SerializeField] float segmentLength = 1;

    public void Generate(Vector3 a, Vector3 b)
    {
        Vector3 c = b - a;
        float segments = c.magnitude / segmentLength;
        Vector3 segment = c / segments;
        
        for (int i = 0; i < segments; i++)
        {
            GameObject temp = Instantiate(
                pathLight, 
                a + (segment * i) + (Vector3.up * 0.2f),
                Quaternion.Euler(90, 0, 0),
                transform);
            
            temp.transform.LookAt(b);
        }
    }
}