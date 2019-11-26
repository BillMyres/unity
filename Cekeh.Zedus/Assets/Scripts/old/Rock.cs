using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {
    public static int mapSize = 100;
    public Material material;
    public int renderDistance = 9;
    GameObject Player, TILES;
    


    //MAP SETTINGS
    [Range(0.25f, 4f)]
    public static float amplitude = 5f, frequency = 0.3f, scale = 15f;//5, .45, 15 = old
    
    void Start() {
        TILES = GameObject.Find("TILES");
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    int xSq = 0,
        zSq = 0;
    void Update() {
        int sqDistance = (int)(Mathf.Sqrt(renderDistance));//root of renderDistance

        GenerateMap((Player.transform.position.x - 100) + (xSq * mapSize), (Player.transform.position.z - 100) + (zSq * mapSize));

        xSq++;
        if (xSq >= sqDistance) { zSq++; xSq = 0; }
        if (zSq >= sqDistance) { zSq = 0; xSq = 0; }
    }

    void GenerateMap(float xPos, float zPos) {
        int size = mapSize - 1;
        Mesh mesh = new Mesh();
        
        //START POSITION
        int xTemp = (int)(xPos / size);
        int zTemp = (int)(zPos / size);
        int xOffset = xTemp * size;
        int zOffset = zTemp * size;
        
        //IF EXIST, RETURN
        if (GameObject.Find("ROCK" + xOffset + ", " + zOffset)) { return; }

        //INIT FOR MESH
        Vector3[] verts = new Vector3[mapSize * mapSize];
        Vector3[] norms = new Vector3[mapSize * mapSize];
        Vector2[] uvs   = new Vector2[mapSize * mapSize];
        Color[] color = new Color[verts.Length];
        int[] triag = new int[(size * size) * 6];
        int triagCount = 0;

        //MAIN LOOP
        for (int x = 0; x < mapSize; x++) {
            for (int z = 0; z < mapSize; z++) {

                //INIT
                int n = (z * mapSize) + x;
                float amp = 1f, freq = 1f, y = 0;

                //GENERATION LOOP
                for (int i = 0; i < 4; i++) {
                    float tX = (x + 344883 + xOffset) / scale * freq,
                          tZ = (z + 324894 + zOffset) / scale * freq;
                    y += (Mathf.PerlinNoise(tX, tZ) * 2 - 1) * amp;
                    
                    amp *= amplitude;
                    freq *= frequency;
                }

                //SET HEIGHT OF MESH VERTICIE
                verts[n] = new Vector3(x + xOffset, y - 10, z + zOffset);//-10 becuase rock

                //SET TRIANGLES OF MESH
                if(x < size && z < size) { 
                    triag[triagCount + 0] = n;
                    triag[triagCount + 1] = n + mapSize;
                    triag[triagCount + 2] = n + (mapSize + 1);

                    triag[triagCount + 3] = n + (mapSize + 1);
                    triag[triagCount + 4] = n + 1;
                    triag[triagCount + 5] = n;

                    triagCount += 6;
                }

                //SET UVS
                uvs[n] = new Vector2(x, z);

                //SET NORMALS
                norms[n] = Vector3.up;

                //SET COLOR
                color[n] = new Color32(21, 155, 21, 55);

                
                
            }
        }

        //SET MESH
        mesh.vertices = verts;
        mesh.triangles = triag;
        mesh.uv = uvs;
        mesh.normals = norms;
        mesh.colors = color;

        //CREATE OBJECT
        GameObject obj = new GameObject("ROCK" + xOffset + ", " + zOffset);
        obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();
        obj.AddComponent<MeshCollider>();
        obj.AddComponent<TerrainUnloader>();
        obj.tag = "Terrain";

        //SET COMPONENTS
        obj.GetComponent<MeshFilter>().sharedMesh = mesh;
        obj.GetComponent<MeshCollider>().sharedMesh = mesh;
        obj.GetComponent<MeshRenderer>().sharedMaterial = material;
        obj.transform.parent = TILES.transform;
    }

    public static float getHeight(float x, float z) {
        
        float amp = 1f, freq = 1f, yPosition = 0;

        for (int i = 0; i < 4; i++) {
            float tX = (x + 344883) / scale * freq,
                    tZ = (z + 324894) / scale * freq;
            yPosition += (Mathf.PerlinNoise(tX, tZ) * 2 - 1) * amp;
                    
            amp *= amplitude;
            freq *= frequency;
        }

        return yPosition;
    }
}
