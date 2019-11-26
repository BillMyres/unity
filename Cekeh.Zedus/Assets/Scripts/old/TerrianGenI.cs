using UnityEngine;
using System.Collections;

public class TerrianGenI : MonoBehaviour {

    public GameObject CUBEobj;
    static GameObject cube;
    static Mesh map = new Mesh();
    static int startPOSx,
        startPOSz,
        mapSize,
        mapHeight;

    static void GenerateMap(int x, int z, int size, int height) {
        startPOSx = x;
        startPOSz = z;
        mapSize = size;
        mapHeight = height;

        map.vertices = getVerticies();
        map.triangles = getTriangles();
        map.uv = getUVs();
    }

    static Vector3[] getVerticies() {
        Vector3[] verticies = new Vector3[mapSize * mapSize];
        Debug.Log("LENGTH: [" + verticies.Length + "]");
        for (int x = 0; x < mapSize; x++) {
            for (int z = 0; z < mapSize; z++) {
                //Debug.Log(x + (z * mapSize));
                int seedX = x + startPOSx,
                    seedZ = z + startPOSz;
                setSeed(seedX, seedZ);
                //float y = getAverageHeight(seedX, seedZ);

                verticies[x + (z * mapSize)] = new Vector3(seedX, getAverageHeight(seedX, seedZ) * (Random.value + 1), seedZ);
                //GameObject.Instantiate(cube, new Vector3(x + startPOSx, 1 * Random.value, z + startPOSz), Quaternion.identity);
            }
        }
        return verticies;
    }

    static int[] getTriangles() {
        int[] triangles = new int[(mapSize - 1) * (mapSize - 1) * 2 * 3];
        //Debug.Log(triangles.Length);
        for (int x = 0; x < mapSize - 1; x++) {
            for (int z = 0; z < mapSize - 1; z++) {
                triangles[x + (z * (mapSize - 1))] = 0;
                triangles[x + (z * (mapSize - 1)) + mapSize] = 1;
                triangles[x + (z * (mapSize - 1)) + (mapSize - 1)] = 1;

                triangles[x + (z * (mapSize - 1))] = 0;
                triangles[x + (z * (mapSize - 1)) + (mapSize - 1)] = 1;
                triangles[x + (z * (mapSize - 1)) + 1] = 1;
            }
        }
        return null;
    }

    static Vector2[] getUVs() {
        Vector2[] uvs = new Vector2[map.vertices.Length];
        for (int i = 0; i < map.vertices.Length; i++) {
            uvs[i] = new Vector2(map.vertices[i].x, map.vertices[i].z);
        }
        return uvs;
    }

    static float getAverageHeight(int x, int z) {
        float y = (getHeight(x + 1, z)+
            getHeight(x - 1, z)+
            getHeight(x, z + 1)+
            getHeight(x, z - 1)) / 4;

        //Debug.Log(getHeight(x+1, z+1));
        

        return y;
    }

    static void setSeed(int x, int z) {
        Random.seed = x * 42589 + z * 35082;
    }

    static float getHeight(int x, int z) {
        setSeed(x, z);
        return 1 * Random.value;
    }

    void Start() {
        //Random.seed = 42;
        cube = CUBEobj;
        GenerateMap(0, 0, 4, 4);
        placeVerticies();
        Debug.Log(getAverageHeight(16, 5));
    }

    void Update() {
        if (map != null) {
            Debug.Log("DRAWING, at least trying too;");
            Graphics.DrawMesh(map, Vector3.zero, Quaternion.identity, null, 0);
        }
    }

    void placeVerticies() {
        foreach (Vector3 v in map.vertices) {
            GameObject.Instantiate(cube, new Vector3(v.x, v.y, v.z), Quaternion.identity);
        }
    }
}
