using UnityEngine;
using System.Collections;

public class TerrianGenIII : MonoBehaviour {

    Mesh MAP;
    Color[] colors;
    public Material MATT;
    int SIZE = 64;
    int[] random;

    void Start () {
        GenerateRandom();
        GeneratePlane();
	}
	
	// Update is called once per frame
	void Update () {
        if (MAP) {
            Graphics.DrawMesh(MAP, Vector3.zero, Quaternion.identity, MATT, 0);
        }

	}

    void GeneratePlane() {
        MAP = new Mesh();
        MAP.vertices = Verticies();
        MAP.triangles = Triangles();
        MAP.uv = UVs();
        MAP.RecalculateNormals();
    }

    Vector3[] Verticies() {
        Vector3[] v = new Vector3[SIZE * SIZE];
        for (int x = 0; x < SIZE; x++){
            for (int z = 0; z < SIZE; z++){
                int n = (z * (SIZE)) + x;
                v[n].x = x;
                v[n].z = z;
            }
        }

        return GenerateHeight(v);
    }

    int[] Triangles() {
        int[] t = new int[((SIZE - 1) * (SIZE - 1)) * 6];
        int count = 0;
        for (int x = 0; x < SIZE - 1; x++) {
            for (int z = 0; z < SIZE - 1; z++) {
                int n = ((z * (SIZE)) + x);
                t[count + 0] = n;
                t[count + 1] = n + SIZE;
                t[count + 2] = n + (SIZE + 1);

                t[count + 3] = n + (SIZE + 1);
                t[count + 4] = n + 1;
                t[count + 5] = n;
                
                count+=6;
            }
        }
        return t;
    }

    Vector2[] UVs() {
        Vector2[] u = new Vector2[MAP.vertices.Length];
        for (int i = 0; i < MAP.vertices.Length; i++) {
            u[i] = new Vector2(MAP.vertices[i].x, MAP.vertices[i].z);
        }
        return u;
    }

    Vector3[] GenerateHeight(Vector3[] v) {

        for (int x = 0; x < SIZE; x++) {
            for (int z = 0; z < SIZE; z++) {
                int n = ((z * (SIZE)) + x);
                v[n].y = random[n];
            }
        }

        return v;
    }

    void GenerateRandom() {
        random = new int[SIZE * SIZE];
        int shift = SIZE / 2;

        for (int x = shift; x > 0; x /= 2) {

        }
        
    }
}
