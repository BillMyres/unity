using UnityEngine;
using System.Collections;

public class GenerateTerrian : MonoBehaviour {

    Mesh terrain, bufferedTerrain;
    Mesh[] chunks;
    
    public MeshFilter mapFilter;
    public GameObject Land;
    public Material material;
    Color[] colors;

    public GameObject player;
    bool spawned = false;
    public static float playerHeight = -1000f;
    int waveSpeed = 10, waveCount = 0;

    [Range(16, 64)]
    public int width = 16, height = 16;
    int uWidth, uHeight;

    [Range(0f, 10f)]
    public float scale = 3f, amplitude = 3.5f, freqency = 0.25f;
    float uScale, uAmplitude, uFreqency;

    int octaves = 4;

    public static float xPosition = 100f, zPosition = 100f, xOld, zOld;
    public float minHeight = -1, maxHeight = 1;

	void Start () {

        //Init
        chunks = new Mesh[100];

        xOld = xPosition;
        zOld = zPosition;

        minHeight = float.MaxValue;
        maxHeight = float.MinValue;

        uWidth = width;
        uHeight = height;
        uScale = scale;
        uAmplitude = amplitude;
        uFreqency = freqency;
        
        terrain = CalculateMesh();
        MeshCollider meshc = Land.AddComponent(typeof(MeshCollider)) as MeshCollider;
    }
	
	// Update is called once per frame
	void Update () {
        xPosition = player.transform.position.x - (width / 2);
        zPosition = player.transform.position.z - (height / 2);
        player.transform.position = new Vector3(player.transform.position.x, playerHeight, player.transform.position.z);

        terrain = CalculateMesh();
        if (waveCount > waveSpeed) {
            waveCount = 0;
        }
        waveCount++;

        //Draw Mesh
        

        Graphics.DrawMesh(terrain, Vector3.zero, Quaternion.identity, material, 0);






        
    }

    //Generate Buffered Mesh
    Mesh CalculateMesh() {

        bufferedTerrain = new Mesh();
        bufferedTerrain.vertices = getVerticies();
        bufferedTerrain.triangles = getTriangles();
        bufferedTerrain.uv = getUVs();

        bufferedTerrain.colors = colors;
        bufferedTerrain.RecalculateNormals();

        //mapCollider.sharedMesh = bufferedTerrain;
        
        return bufferedTerrain;

    }

    //Generate new Verticies
    Vector3[] getVerticies() {

        Vector3[] v = new Vector3[width * height];
        float waveHeight = 0;
        //Reset colors
        colors = new Color[v.Length];

        float[] wave = new float[v.Length];

        //Prevent scale error
        if (scale < 0) { scale = 0.0001f; }

        //For each verticie
        for (int x = 0; x < width; x++){
            for (int z = 0; z < height; z++){
                //Init
                float amp = 1;
                float freq = 1;
                float yPos = 0;

                //for each octave
                for (int i = 0; i < octaves; i++) {
                    float sampleX = (x + xPosition + 344883) / scale * freq;
                    float sampleZ = (z + zPosition + 324894) / scale * freq;

                    yPos += (Mathf.PerlinNoise(sampleX, sampleZ) * 2 - 1) * amp;

                    amp *= amplitude;
                    freq *= freqency;
                }

                int n = (z * (width)) + x;

                v[n].x = x + xPosition;
                v[n].z = z + zPosition;
                v[n].y = yPos;

                if (yPos < minHeight) {
                    minHeight = yPos;
                } else if (yPos > maxHeight) {
                    maxHeight = yPos;
                }

                if (yPos < 0f) {
                    colors[n] = Color.blue;
                }else if(yPos >= 0 && yPos < 10f){
                    colors[n] = Color32.Lerp(Color.green, Color.gray, yPos / 10f);
                    //colors[n] = Color.green;
                }else if(yPos >= 10f){
                    colors[n] = Color32.Lerp(Color.gray, Color.white, yPos / 21f);
                    //colors[n] = Color.gray;
                }

                if (x == 8 + xPosition && z == 8 + zPosition && spawned == false) {
                    //player.transform.position = new Vector3(x + xPosition, yPos, z + zPosition);
                    spawned = true;
                    playerHeight = yPos;
                }
                if (x == 8 && z == 8 && spawned == true) {
                    playerHeight = yPos;
                }


                if (wave[n] == 0) {
                    wave[n] = v[n].y * Random.value / 50;
                }
                if (waveCount > waveSpeed) {
                    wave[n] = v[n].y * Random.value / 50;
                }
                if (yPos < 0f) {
                    v[n].y = wave[n];
                }
            }
        }

        return v;
    }

    //Generate Triangles
    int[] getTriangles() {

        int[] t = new int[((width - 1) * (height - 1)) * 6];

        int count = 0;

        for (int x = 0; x < width - 1; x++){

            for (int z = 0; z < height - 1; z++){

                int n = ((z * (width)) + x);

                t[count + 0] = n;
                t[count + 1] = n + width;
                t[count + 2] = n + (width + 1);

                t[count + 3] = n + (width + 1);
                t[count + 4] = n + 1;
                t[count + 5] = n;

                count += 6;
            }
        }

        return t;
    }

    //Generate UVs
    Vector2[] getUVs() {

        Vector2[] u = new Vector2[bufferedTerrain.vertices.Length];

        for (int i = 0; i < bufferedTerrain.vertices.Length; i++){

            u[i] = new Vector2(bufferedTerrain.vertices[i].x, bufferedTerrain.vertices[i].z);

        }

        return u;
    }
}
