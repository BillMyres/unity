using UnityEngine;
using System.Collections;

public class TerrainTest : MonoBehaviour {

    Mesh map;
    public MeshFilter mFilter;
    public MeshRenderer mRenderer;

    [Range(1f, 4f)]
    public float miniMapScale = 2f;

    int printCount = 0;
    int rockCount = 0;
    int treeCount = 0;

    Vector3[] ROCKS;
    public static ArrayList rocksSpawned;
    public GameObject rock;

    Vector3[] TREES;
    public static ArrayList treesSpawned;
    public GameObject tree;
    
    public Texture heightMap;

    public static float PLAYERHEIGHT;
    public static int spawnDistance = 500;

    public float amplitude = 3.5f, frequency = 0.25f, scale = 3f;
    public Vector2 OFFSET;
    public static float xOffset, zOffset;
    public static int width = 100, height = 100;

    Vector3[] colliderVerticies;

    public float[] COLORHEIGHTS;
    
	void Start () {
        //rocks
        rocksSpawned = new ArrayList();
        ROCKS = new Vector3[40];

        //trees
        treesSpawned = new ArrayList();
        TREES = new Vector3[40];

        COLORHEIGHTS = new float[5];
        COLORHEIGHTS[0] = 4.28f;
        COLORHEIGHTS[1] = 9.69f;
        COLORHEIGHTS[2] = 14.1f;
        COLORHEIGHTS[3] = 18.8f;
        COLORHEIGHTS[4] = 30.2f;
        if (!map) {
            map = createPlane();
        }

    }
	
	void Update () {
        OFFSET = new Vector2(xOffset, zOffset);
        mRenderer.material.SetTextureOffset("_MainTex", new Vector2(xOffset * 0.06f, zOffset * 0.06f));
        map = createPlane();
        mFilter.sharedMesh = map;

        PLAYERHEIGHT = getHeight(width / 2, height / 2);

        //check if rocks spawned
        foreach(Vector3 v in ROCKS) {
            if(!rocksSpawned.Contains(v) && Vector3.Distance(v, new Vector3(xOffset + (width / 2), PLAYERHEIGHT + 1f, zOffset + (height / 2))) < spawnDistance)
            {
                rocksSpawned.Add(v);
                //print(v);
                Instantiate(rock, v, Quaternion.identity);
                
            }
        }
        foreach (Vector3 v in TREES) {
            if (!treesSpawned.Contains(v) && Vector3.Distance(v, new Vector3(xOffset + (width / 2), PLAYERHEIGHT + 1f, zOffset + (height / 2))) < spawnDistance) {
                treesSpawned.Add(v);
                //print(v);
                Instantiate(tree, v, Quaternion.identity);
            }
        }
    }

    void OnGUI() {
        if (heightMap) { GUI.DrawTexture(new Rect(10, 10, heightMap.width * miniMapScale, heightMap.height * miniMapScale), heightMap); }
    }
    /////////////////////

    Mesh createPlane(){
        Mesh plane = new Mesh();
        int count = 0;
        

        Vector3[] verticies = new Vector3[width * height];
        int[] triangles = new int[((width - 1) * (height - 1)) * 6];
        Vector2[] uvs = new Vector2[width * height];
        Vector3[] normals = new Vector3[width * height];

        //Texture / Mesh Colors / Material Colors
        Texture2D colors = new Texture2D(width, height);
        Color[] MapColors = new Color[verticies.Length];

        for (int x = 0; x < width; x++){
            for (int z = 0; z < height; z++){
                //init
                int n = (z * width) + x;
                float amp = 1f, freq = 1f, yPosition = 0;
                                
                //octaves
                for (int i = 0; i < 4; i++) {
                    float tX = (x + 344883 + xOffset) / scale * freq, //ADD X AND Z POS
                          tZ = (z + 324894 + zOffset) / scale * freq;
                    yPosition += (Mathf.PerlinNoise(tX, tZ) * 2 - 1) * amp;
                    
                    amp *= amplitude;
                    freq *= frequency;
                }

                //COLORS
                if (yPosition <= COLORHEIGHTS[0]) {//WATER
                    colors.SetPixel(x, z, Color.blue);
                    MapColors[n] = Color32.Lerp(new Color32(243, 213, 177, 1), new Color32(16, 55, 114, 1), (yPosition * -1) / 20);
                }
                if (yPosition > COLORHEIGHTS[0]) {//WATER TO LAND
                    colors.SetPixel(x, z, Color.green);
                    if (yPosition < 6.5f) { colors.SetPixel(x, z, Color.gray); }
                    
                    MapColors[n] = Color32.Lerp(new Color32(243, 213, 177, 1), new Color32(73, 173, 54, 1), (yPosition - COLORHEIGHTS[0]) / (COLORHEIGHTS[1] - COLORHEIGHTS[0]));// /14.1
                }
                if (yPosition > COLORHEIGHTS[1]) {//LAND
                    colors.SetPixel(x, z, Color.green);
                    MapColors[n] = Color32.Lerp(new Color32(73, 173, 54, 1), new Color32(67, 129, 43, 1), (yPosition - COLORHEIGHTS[1]) / (COLORHEIGHTS[2] - COLORHEIGHTS[1]));// /14.1
                }
                if (yPosition > COLORHEIGHTS[2]) {//LAND TO MOUNTAIN
                    MapColors[n] = Color32.Lerp(new Color32(67, 129, 43, 1), new Color32(139, 97,  39, 1), (yPosition - COLORHEIGHTS[2]) / (COLORHEIGHTS[3] - COLORHEIGHTS[2]));// /14.1
                }
                if (yPosition > COLORHEIGHTS[3]) {//MOUNTAIN
                    colors.SetPixel(x, z, Color.white);
                    MapColors[n] = Color32.Lerp(new Color32(139, 97, 39, 1), new Color32(255, 255, 255, 1), (yPosition - COLORHEIGHTS[3]) / (COLORHEIGHTS[4] - COLORHEIGHTS[3]));// /14.1
                }
                if (x == width / 2 && z == height / 2) {
                    Color c = Color.yellow;

                    colors.SetPixel(x, z, c);

                    colors.SetPixel(x+1, z, c);
                    colors.SetPixel(x-1, z, c);
                    colors.SetPixel(x, z+1, c);
                    colors.SetPixel(x, z-1, c);
                }

                //set verticie(x, z)
                verticies[n] = new Vector3(x + xOffset, yPosition, z + zOffset);

                if(x < width - 1 && z < height - 1) { 
                    //set triangle(1-6)
                    triangles[count + 0] = n;
                    triangles[count + 1] = n + width;
                    triangles[count + 2] = n + (width + 1);

                    triangles[count + 3] = n + (width + 1);
                    triangles[count + 4] = n + 1;
                    triangles[count + 5] = n;

                    count += 6;
                }

                //set uvs(v2)
                uvs[n] = new Vector2(x, z);

                //set normals
                normals[n] = Vector3.up;

                //rocks
                if (yPosition > 7 && yPosition < 14) {
                    Random.seed = (x + 34057 + (int)(xOffset)) * (z + 34535 + (int)(zOffset));
                    float r = Random.value;

                    if (r > .999f) {
                        colors.SetPixel(x, z, Color.magenta);

                        ROCKS[rockCount] = new Vector3((int)(x + xOffset), (int)(yPosition), (int)(z + zOffset));
                    }
                    rockCount++;
                    if (rockCount > 19) {
                        rockCount = 0;
                    }

                    if (r > .555f && r < .560f) {
                        colors.SetPixel(x, z, Color.red);

                        TREES[treeCount] = new Vector3((int)(x + xOffset), (int)(yPosition), (int)(z + zOffset));
                    }
                    treeCount++;
                    if (treeCount > 19) {
                        treeCount = 0;
                    }
                }
            }
        }
        

        colors.Apply();

        plane.vertices = verticies;
        plane.triangles = triangles;
        plane.uv = uvs;
        plane.normals = normals;
        plane.colors = MapColors;
        heightMap = colors;
        colors.wrapMode = TextureWrapMode.Clamp;

        return plane;
    }

    float getHeight(int x, int z) {

        return map.vertices[(z * width) + x].y;
    }
}
