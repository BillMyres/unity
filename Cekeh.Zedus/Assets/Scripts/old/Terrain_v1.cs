using UnityEngine;
using System.Collections;

public class Terrain_v1 : MonoBehaviour {

    Mesh map;

    public float xPosition, zPosition, playerHeight;
    public GameObject player;
    public GameObject mapObject;
    public Material   material;

    Vector3 lastPosition;

    MeshFilter   mapFilter;
    MeshRenderer mapRenderer;
    MeshCollider mapCollider;

    float[] positionHeight;//log all positions

    Color[] mapColors;

    int     mapWidth = 20, mapHeight = 20;
    float   mapScale = 3f, mapAmplitude = 3.5f, mapFrequency = 0.25f;
    int octaves = 4;
    int waveSpeed = 10, waveCount = 0;

    void Start() {
        lastPosition = new Vector3(100, 0, 100);
        Init();
        mapFilter = mapObject.AddComponent<MeshFilter>();
        mapRenderer = mapObject.AddComponent<MeshRenderer>();
        mapCollider = mapObject.AddComponent<MeshCollider>();
        //mapFilter.mesh = GenerateMap();
    }

    void Init() {
        xPosition = player.transform.position.x - (mapWidth / 2);
        zPosition = player.transform.position.z - (mapHeight / 2);

        if (mapScale < 0) { mapScale = 0.0001f; }
        if (waveCount > waveSpeed) {
            waveCount = 0;
        }
        waveCount++;
    }

    void Update() {
        //set every frame
        Init();
        mapFilter.mesh = GenerateMap();
        mapRenderer.material = material;
        mapCollider.sharedMesh = mapFilter.mesh;

        player.transform.position = new Vector3(player.transform.position.x, playerHeight, player.transform.position.z);
    }

    Mesh GenerateMap() {
        map = new Mesh();
        map.vertices = Verticies();
        map.triangles = Triangles();
        map.uv = UVs();

        map.colors = mapColors;
        map.RecalculateNormals();

        return map;
    }

    Vector3[] Verticies() {
        Vector3[] v = new Vector3[mapWidth * mapHeight];
        mapColors = new Color[v.Length];
        float[] wave = new float[v.Length];

        

        for (int x = 0; x < mapWidth; x++) {
            for (int z = 0; z < mapHeight; z++) {
                float amplitude = 1, frequency = 1, yPosition = 0;

                for (int i = 0; i < octaves; i++) {
                    float tempX = (x + xPosition + 344883) / mapScale * frequency,
                          tempZ = (z + zPosition + 324894) / mapScale * frequency;
                    yPosition += (Mathf.PerlinNoise(tempX, tempZ) * 2 - 1) * amplitude;
                    amplitude *= mapAmplitude;
                    frequency *= mapFrequency;
                }
                int n = (z * (mapWidth)) + x;

                v[n].x = x + xPosition;
                v[n].z = z + zPosition;
                v[n].y = yPosition;

                //set player Height
                if (x == (mapWidth / 2) && z == (mapHeight / 2)) {
                    if ((yPosition + 0.7f) - playerHeight > .25f) {
                        print((yPosition + 0.7f) - playerHeight);
                    }
                    playerHeight = yPosition + 0.7f;
                    lastPosition = player.transform.position;
                }

                //COLORS
                if (yPosition < 0f) {
                    mapColors[n] = Color.blue;
                }else if(yPosition >= 0 && yPosition < 10f){
                    mapColors[n] = Color32.Lerp(Color.green, Color.gray, yPosition / 10f);
                }else if(yPosition >= 10f){
                    mapColors[n] = Color32.Lerp(Color.gray, Color.white, yPosition / 21f);
                }

                //WATER
                if (wave[n] == 0) {
                    wave[n] = v[n].y * Random.value / 50;
                }
                if (waveCount > waveSpeed) {
                    wave[n] = v[n].y * Random.value / 50;
                }
                if (yPosition < 0f) {
                    v[n].y = wave[n];
                }
            }
        }

        return v;
    }

    int[] Triangles() {
        int[] t = new int[((mapWidth - 1) * (mapHeight - 1)) * 6];
        int count = 0;

        for (int x = 0; x < mapWidth - 1; x++){
            for (int z = 0; z < mapHeight - 1; z++){
                int n = ((z * (mapWidth)) + x);

                t[count + 0] = n;
                t[count + 1] = n + mapWidth;
                t[count + 2] = n + (mapWidth + 1);

                t[count + 3] = n + (mapWidth + 1);
                t[count + 4] = n + 1;
                t[count + 5] = n;

                count += 6;
            }
        }
        return t;
    }

    Vector2[] UVs() {
        Vector2[] u = new Vector2[map.vertices.Length];

        for (int i = 0; i < map.vertices.Length; i++){
            u[i] = new Vector2(map.vertices[i].x, map.vertices[i].z);
        }
        return u;
    }

}
