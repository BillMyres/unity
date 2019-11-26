using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {
    public static int mapSize = 100;
    public Material material;
    public int renderDistance = 9;
    GameObject Player;

    //DECORATIONS
    public GameObject ROCKS, TREES, rock, tree, rock2;
    ArrayList rockList, treeList;

    GameObject TILES;

    //MAP SETTINGS
    public static float a_amplitude = 6f,  a_frequency = 0.25f, a_scale = 20f;
    public static float b_amplitude = 5f,  b_frequency = 0.30f, b_scale = 15f;
    public static float c_amplitude = 4f, c_frequency = 0.50f, c_scale = 3f;

    //MAP TEXTURE
    [Range(0.1f, 5f)]
    public float mapScale = 1;
    //A* 
    
    public Texture mapTex;
    void Start() {
        TILES = GameObject.Find("TILES");
        Player = GameObject.FindGameObjectWithTag("Player");
        rockList = new ArrayList();
        treeList = new ArrayList();
    }

    void OnGUI() {
        int size = mapSize - 1,
            x = (int)(Player.transform.position.x / size) * size,
            z = (int)(Player.transform.position.z / size) * size;

        
        GameObject obj = GameObject.Find("MAP" + x + ", " + z);
        Texture t = new Texture();
        if (obj) { 
            t = obj.GetComponent<miniMap>().texture;
        }
        if (t) { GUI.DrawTexture(new Rect(10, 10, t.width, t.height), t); }
        
        int tx = (int)(Player.transform.position.x / size) * size,
            tz = (int)(Player.transform.position.z / size) * size;
        int mapX = (int)(Player.transform.position.x) - tx,
            mapY = (int)(Player.transform.position.z) - tz;

        GUI.DrawTexture(new Rect(10 + mapX - 8, 110 - (mapY) - 8, 16, 16), mapTex);
    }

    int xSq = 0,
        zSq = 0;
    void Update() {
        int sqDistance = (int)(Mathf.Sqrt(renderDistance));//root of renderDistance

        GenerateMap((Player.transform.position.x - 175) + (xSq * mapSize), (Player.transform.position.z - 100) + (zSq * mapSize));

        xSq++;
        if (xSq >= sqDistance) { zSq++; xSq = 0; }
        if (zSq >= sqDistance) { zSq = 0; xSq = 0; }


        if (Input.GetKeyUp(KeyCode.R)) {
            Player.transform.position = new Vector3((int)(Player.transform.position.x), (int)(Player.transform.position.y), (int)(Player.transform.position.z));
        }
        if (rockList.Count > 0) { 
            foreach (Vector3 v in rockList) {
                GameObject r;

                Random.seed = (int)((v.x * 99) + (v.z * 999));
                float val = Random.value;
                if (val > .4999f) {
                    r = Instantiate(rock, v, Quaternion.identity) as GameObject;
                }else{
                    r = Instantiate(rock2, v, Quaternion.identity) as GameObject;
                }
                
                r.transform.parent = ROCKS.transform;
                float rand = Mathf.PerlinNoise(v.x * 2340870, v.z * 432904);
                
                rand *= 0.1f;
                //if (rand < 0) { rand = 0.01f; }
                if (r.name == "Rock_1(Clone)") {
                    r.transform.localScale *= (int)(Random.Range(1f, 10f));
                }
                r.transform.localScale *= (int)(Random.Range(1f, 10f));

            }
            rockList = new ArrayList();
        }
        if (treeList.Count > 0) {
            foreach (Vector3 v in treeList) {
                GameObject t = Instantiate(tree, v, Quaternion.identity) as GameObject;
                t.transform.parent = TREES.transform;
            }
            treeList = new ArrayList();
        }
        
    }

    void GenerateMap(float xPos, float zPos) {
        int size = mapSize - 1;
        Mesh mesh = new Mesh();
        //TEST
        Mesh rmesh = new Mesh();

        //START POSITION
        int xTemp = (int)(xPos / size);
        int zTemp = (int)(zPos / size);
        int xOffset = xTemp * size;
        int zOffset = zTemp * size;

        //TEXTURE MAP
        Texture2D mapTexture = new Texture2D(mapSize, mapSize);

        //IF EXIST, RETURN
        if (GameObject.Find("MAP" + xOffset + ", " + zOffset)) { return; }

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

                float a_amp = 1f, a_freq = 1f, a_y = 0;
                float b_amp = 1f, b_freq = 1f, b_y = 0;
                float c_amp = 1f, c_freq = 1f, c_y = 0;

                //GENERATION LOOP
                for (int i = 0; i < 4; i++) {
                    float a_x = (x + 344883 + xOffset) / a_scale * a_freq,
                          a_z = (z + 324894 + zOffset) / a_scale * a_freq;
                    a_y += (Mathf.PerlinNoise(a_x, a_z) * 2 - 1) * a_amp;

                    //TEST
                    float b_x = (x + 344883 + xOffset) / b_scale * b_freq,
                          b_z = (z + 324894 + zOffset) / b_scale * b_freq;
                    b_y += (Mathf.PerlinNoise(b_x, b_z) * 2 - 1) * b_amp;

                    float c_x = (x + xOffset) / c_scale * c_freq,
                          c_z = (z + zOffset) / c_scale * c_freq;
                    c_y += (Mathf.PerlinNoise(c_x, c_z) * 2 - 1) * c_amp;

                    a_amp *= a_amplitude;
                    a_freq *= a_frequency;

                    b_amp *= b_amplitude;
                    b_freq *= b_frequency;

                    c_amp *= c_amplitude;
                    c_freq *= c_frequency;

                    //leave

                }

                c_y += a_y;

                //SET FLAT SPOTS
                if (a_y > 6.5f && a_y < 11) { a_y = 6.5f; }//y -= 1 * (18 - y); old = 6.5f
                if (a_y > 11) { a_y = a_y - 4.5f; }

                if (a_y > 15 && a_y < 18) { a_y = 15; }
                if (a_y > 18) { a_y = a_y - 3; }

                if (a_y > 25 && a_y < 30) { a_y = 25; }
                if (a_y > 30) { a_y = a_y - 5; }

                if (b_y > a_y) {
                    a_y = b_y;
                }

                

                //COLORS
                if (a_y <= 5.75) {
                    mapTexture.SetPixel(x, z, new Color32(0, 0, 255, 255));
                    color[n] = new Color32(0, 0, 255, 1);
                }else if (a_y > 5.75f && a_y <= 6.5f) {
                    mapTexture.SetPixel(x, z, new Color32(129, 191, 120, 255));
                    color[n] = new Color32(129, 191, 120, 1);
                }else if (a_y == 15) {
                    mapTexture.SetPixel(x, z, new Color32(72, 133, 64, 255));
                    color[n] = new Color32(72, 133, 64, 1);
                }else if (a_y == 25) {
                    mapTexture.SetPixel(x, z, new Color32(117, 117, 48, 255));
                    color[n] = new Color32(117, 117, 48, 1);
                }
                if (a_y > 6.5f && a_y < 15) {
                    mapTexture.SetPixel(x, z, new Color32(33, 84, 29, 255));
                    color[n] = new Color32(33, 84, 29, 1);
                } else if (a_y > 15 && a_y < 25) {
                    mapTexture.SetPixel(x, z, new Color32(64, 64, 25, 255));
                    color[n] = new Color32(64, 64, 25, 1);
                } else if (a_y > 25) {
                    mapTexture.SetPixel(x, z, new Color32(48, 48, 18, 255));
                    color[n] = new Color32(48, 48, 18, 1);
                }

                //TEST
                if (a_y > 20f && c_y > a_y) {
                    if (c_y - a_y > .5f) { c_y = a_y - 0.1f; }
                    a_y = c_y;
                    
                    mapTexture.SetPixel(x, z, new Color32(51, 100, 51, 255));
                    color[n] = new Color32(75, 100, 75, 1);
                }
                //SET HEIGHT OF MESH VERTICIE
                verts[n] = new Vector3(x + xOffset, a_y, z + zOffset);

                //SET TRIANGLES OF MESH
                if (x < size && z < size) { 
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

                

                //DECORATIONS
                if (a_y >= 6 && a_y != c_y) {//ONLY ABOVE WATER
                    Random.seed = (x + 34057 + xOffset) * (z + 34535 + zOffset);
                    float r = Random.value;
                    Vector3 pos = new Vector3(x + xOffset, a_y, z + zOffset);

                    //CREATE ROCK
                    if (r > .999f) {
                        mapTexture.SetPixel(x, z, new Color32(255, 255, 255, 255));
                        rockList.Add(pos);
                    }

                    //CREATE TREE
                    if (r > .555 && r < .556) {
                        mapTexture.SetPixel(x, z, new Color32(255, 255, 255, 255));
                        treeList.Add(pos);
                    }
                }
            }
        }
        //APPLY TEXTURE
        mapTexture.Apply();
        //textureMapList.Add(mapTexture);
        mapTexture.name = xOffset + ", " + zOffset;

        //SET MESH
        mesh.vertices = verts;
        mesh.triangles = triag;
        mesh.uv = uvs;
        mesh.normals = norms;
        mesh.colors = color;

        //CREATE OBJECT
        GameObject obj = new GameObject("MAP" + xOffset + ", " + zOffset);
        obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();
        obj.AddComponent<MeshCollider>();
        obj.AddComponent<TerrainUnloader>();
        obj.AddComponent<miniMap>();
        obj.tag = "Terrain";

        //SET COMPONENTS
        obj.GetComponent<MeshFilter>().sharedMesh = mesh;
        obj.GetComponent<MeshCollider>().sharedMesh = mesh;
        obj.GetComponent<MeshRenderer>().sharedMaterial = material;
        obj.GetComponent<miniMap>().texture = mapTexture;
        obj.transform.parent = TILES.transform;
        
    }

    public static float getHeight(float x, float z) {
        
        float a_amp = 1f, a_freq = 1f, a_y = 0;

        //TEST
        float b_amp = 1f, b_freq = 1f, b_y = 0;

        float c_amp = 1f, c_freq = 1f, c_y = 0;

        //GENERATION LOOP
        for (int i = 0; i < 4; i++) {
            float a_x = (x + 344883) / a_scale * a_freq,
                  a_z = (z + 324894) / a_scale * a_freq;
            a_y += (Mathf.PerlinNoise(a_x, a_z) * 2 - 1) * a_amp;

            //TEST
            float b_x = (x + 344883) / b_scale * b_freq,
                  b_z = (z + 324894) / b_scale * b_freq;
            b_y += (Mathf.PerlinNoise(b_x, b_z) * 2 - 1) * b_amp;

            float c_x = (x) / c_scale * c_freq,
                  c_z = (z) / c_scale * c_freq;
            c_y += (Mathf.PerlinNoise(c_x, c_z) * 2 - 1) * c_amp;

            b_amp *= b_amplitude;
            b_freq *= b_frequency;

            //leave
            a_amp *= a_amplitude;
            a_freq *= a_frequency;

            c_amp *= c_amplitude;
            c_freq *= c_frequency;
        }
        // SET FLAT SPOTS
        if (a_y > 6.5f && a_y < 11) { a_y = 6.5f; }
        if (a_y > 11) { a_y = a_y - 4.5f; }

        if (a_y > 15 && a_y < 18) { a_y = 15; }
        if (a_y > 18) { a_y = a_y - 3; }

        if (a_y > 25 && a_y < 30) { a_y = 25; }
        if (a_y > 30) { a_y = a_y - 5; }

        if (a_y < 5.75f) { a_y = 5.65f; }

        if (b_y > a_y) {
            a_y = b_y;
        }
        c_y += a_y;
        if (a_y > 20f && c_y > a_y) {
            if (c_y - a_y > .5f) { c_y = a_y - 0.1f; }
            a_y = c_y;
        }

        return a_y;
    }
}
