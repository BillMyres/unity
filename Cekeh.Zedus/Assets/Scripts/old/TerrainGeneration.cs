using UnityEngine;
using System.Collections;

public class TerrainGeneration : MonoBehaviour {

    public Vector2 PlayerChunk;
    public GameObject Player;

    public Mesh[] loadedChunk;
    public int chunkSize = 100;
    public Material terrainMaterial;

    Texture map;
    [Range(4.28f, 30.2f)]
    public float[] mapHeights = new float[5];

    public static float amplitude = 3.5f, frequency = 0.25f, scale = 4f;//scale was 3, amp was 3.5f
    
    void Start () {
        loadedChunk = new Mesh[36];
        PlayerChunk = new Vector2(Player.transform.position.x / (chunkSize - 1), Player.transform.position.z / (chunkSize - 1));
        
        for (int x = 0; x < 6; x++) {
            for (int z = 0; z < 6; z++) {
                int n = (z * 6) + x;
                if (!loadedChunk[n]) {
                    loadedChunk[n] = loadChunk((PlayerChunk - new Vector2(3, 3)) + new Vector2(x, z));
                }
            }
        }
        
	}
	
	void Update () {
        if (PlayerChunk != new Vector2((int)(Player.transform.position.x / (chunkSize - 1)), (int)(Player.transform.position.z / (chunkSize - 1)))) {
            PlayerChunk = new Vector2((int)(Player.transform.position.x / (chunkSize - 1)), (int)(Player.transform.position.z / (chunkSize - 1)));
            
            
            for (int x = 0; x < 6; x++) {
                for (int z = 0; z < 6; z++) {
                    int n = (z * 6) + x;
                    if (!loadedChunk[n]) {
                        loadedChunk[n] = loadChunk((PlayerChunk - new Vector2(3, 3)) + new Vector2(x, z));
                    }
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.R)) {
            for (int x = 0; x < 6; x++) {
                for (int z = 0; z < 6; z++) {
                    int n = (z * 6) + x;
                    if (!loadedChunk[n]) {
                        loadedChunk[n] = loadChunk((PlayerChunk - new Vector2(3, 3)) + new Vector2(x, z));
                    }
                }
            }
        }

        for (int i = 0; i < loadedChunk.Length; i++) {
            if (loadedChunk[i]) {
                GameObject obj = new GameObject(loadedChunk[i].vertices[0].x + ", " + loadedChunk[i].vertices[0].z);
                obj.AddComponent<MeshFilter>();
                obj.AddComponent<MeshRenderer>();
                obj.AddComponent<TerrainUnloader>();
                obj.tag = ("Terrain");

                obj.GetComponent<MeshFilter>().sharedMesh = loadedChunk[i];
                obj.GetComponent<MeshRenderer>().sharedMaterial = terrainMaterial;
                loadedChunk[i] = null;
            }
        }
        
	}

    Mesh loadChunk(Vector2 pos){
        Mesh terrain = new Mesh();

        int xOffset = (int)(pos.x) * (chunkSize - 1), zOffset = (int)(pos.y) * (chunkSize - 1);
        if (GameObject.Find(xOffset + ", " + zOffset)) {
            return null;
        }
        
        Vector3[] verticies = new Vector3[chunkSize * chunkSize];
        int[] triangles = new int[((chunkSize - 1) * (chunkSize - 1)) * 6];
        Vector2[] uvs = new Vector2[chunkSize * chunkSize];
        Vector3[] normals = new Vector3[chunkSize * chunkSize];

        Color[] terrainColors = new Color[verticies.Length];
        Texture2D mapColors = new Texture2D(chunkSize, chunkSize);

        int triangleCount = 0;

        for (int x = 0; x < chunkSize; x++){
            for (int z = 0; z < chunkSize; z++){
                int n = (z * chunkSize) + x;
                float amp = 1f, freq = 1f, yPosition = 0;

                for (int i = 0; i < 4; i++) {
                    float tX = (x + 344883 + xOffset) / scale * freq,
                          tZ = (z + 324894 + zOffset) / scale * freq;
                    yPosition += (Mathf.PerlinNoise(tX, tZ) * 2 - 1) * amp;
                    
                    amp *= amplitude;
                    freq *= frequency;
                }
                //SET FLAT SPOTS
                if (yPosition > 6.5f && yPosition < 11) { yPosition = 6.5f; }
                if (yPosition > 11) { yPosition = yPosition - 4.5f; }

                if (yPosition > 15 && yPosition < 18) { yPosition = 15; }
                if (yPosition > 18) { yPosition = yPosition - 3; }

                if (yPosition > 25 && yPosition < 30) { yPosition = 25; }
                if (yPosition > 30) { yPosition = yPosition - 5; }

                //COLORS
                if (yPosition <= mapHeights[0]) {//WATER
                    mapColors.SetPixel(x, z, Color.blue);
                    terrainColors[n] = Color32.Lerp(new Color32(243, 213, 177, 1), new Color32(16, 55, 114, 1), (yPosition * -1) / 20);
                }
                if (yPosition > mapHeights[0]) {//WATER TO LAND
                    mapColors.SetPixel(x, z, Color.green);
                    if (yPosition < 6.5f) { mapColors.SetPixel(x, z, Color.gray); }

                    terrainColors[n] = Color32.Lerp(new Color32(243, 213, 177, 1), new Color32(73, 173, 54, 1), (yPosition - mapHeights[0]) / (mapHeights[1] - mapHeights[0]));// /14.1
                }
                if (yPosition > mapHeights[1]) {//LAND
                    mapColors.SetPixel(x, z, Color.green);
                    terrainColors[n] = Color32.Lerp(new Color32(73, 173, 54, 1), new Color32(67, 129, 43, 1), (yPosition - mapHeights[1]) / (mapHeights[2] - mapHeights[1]));// /14.1
                }
                if (yPosition > mapHeights[2]) {//LAND TO MOUNTAIN
                    terrainColors[n] = Color32.Lerp(new Color32(67, 129, 43, 1), new Color32(139, 97,  39, 1), (yPosition - mapHeights[2]) / (mapHeights[3] - mapHeights[2]));// /14.1
                }
                if (yPosition > mapHeights[3]) {//MOUNTAIN
                    mapColors.SetPixel(x, z, Color.white);
                    terrainColors[n] = Color32.Lerp(new Color32(139, 97, 39, 1), new Color32(72, 72, 72, 1), (yPosition - mapHeights[3]) / (mapHeights[4] - mapHeights[3]));// /14.1
                }

                //HEIGHT
                verticies[n] = new Vector3(x + xOffset, yPosition, z + zOffset);

                //TRIANGLES
                if(x < chunkSize - 1 && z < chunkSize - 1) { 
                    triangles[triangleCount + 0] = n;
                    triangles[triangleCount + 1] = n + chunkSize;
                    triangles[triangleCount + 2] = n + (chunkSize + 1);

                    triangles[triangleCount + 3] = n + (chunkSize + 1);
                    triangles[triangleCount + 4] = n + 1;
                    triangles[triangleCount + 5] = n;

                    triangleCount += 6;
                }

                //set uvs(v2)
                uvs[n] = new Vector2(x, z);

                //set normals
                normals[n] = Vector3.up;
            }
        }

        mapColors.Apply();

        terrain.vertices = verticies;
        terrain.triangles = triangles;
        terrain.uv = uvs;
        terrain.normals = normals;
        terrain.colors = terrainColors;
        map = mapColors;
        mapColors.wrapMode = TextureWrapMode.Clamp;
        
        return terrain;
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
        // SET FLAT SPOTS
        if (yPosition > 6.5f && yPosition < 11) { yPosition = 6.5f; }
        if (yPosition > 11) { yPosition = yPosition - 4.5f; }

        if (yPosition > 15 && yPosition < 18) { yPosition = 15; }
        if (yPosition > 18) { yPosition = yPosition - 3; }

        if (yPosition > 25 && yPosition < 30) { yPosition = 25; }
        if (yPosition > 30) { yPosition = yPosition - 5; }

        return yPosition;
    }
}
