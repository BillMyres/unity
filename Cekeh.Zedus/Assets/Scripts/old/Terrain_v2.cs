using UnityEngine;
using System.Collections;

public class Terrain_v2 : MonoBehaviour {
    public GameObject Plane;

    void Start() {
    }

    void Update() {
        Vector3[] v = Plane.GetComponent<MeshFilter>().sharedMesh.vertices;

        for (int i = 0; i < v.Length; i++){
                float tX = (v[i].x + 344883),
                      tZ = (v[i].z + 324894);
                v[i].y += (Mathf.PerlinNoise(tX, tZ) * 2 - 1);
            
        }
        Plane.GetComponent<MeshFilter>().sharedMesh.vertices = v;
    }
}
