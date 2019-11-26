using UnityEngine;
using System.Collections;
using System.Threading;

public class GenerateTR : MonoBehaviour {
    public Material material;
    Chunk chunkTR, chunkBR, chunkBL, chunkTL;

    //thread
    Thread tr, br, bl, tl;//top-right, bottom-right, bottom-left, top-left
    bool running = true,
         quit = false;

    public GameObject grass;
    public static GameObject player;
    Vector3 pos;

    //init
    public static int chunkSize = 32;//size of each chunk //64
    public static int distance = 6;//n*n grid //10

    //Terrain texture
    //public Texture2D terrainPNG;
    //public static Texture2D staticTerrainPNG;

    void Start () {
        //staticTerrainPNG = terrainPNG;
        //set player object and update values
        player = GameObject.FindGameObjectWithTag("Player");
        pos = player.transform.position;
        chunkTR = null;
        chunkBR = null;


        tr = new Thread(TopRight);
        tr.Start();
        br = new Thread(BottomRight);
        br.Start();
        bl = new Thread(BottomLeft);
        bl.Start();
        tl = new Thread(TopLeft);
        tl.Start();
    }

    void Update () {
        player = GameObject.FindGameObjectWithTag("Player");
        //update player position for thread to use, because thread can't call get_transforms (will error)
        pos = player.transform.position;
        if (chunkTR != null) {
            Chunk chunk = chunkTR;
            Init(chunk);
            chunkTR = null;
        }
        if (chunkBR != null) {
            Chunk chunk = chunkBR;
            Init(chunk);
            chunkBR = null;
        }
        if (chunkBL != null) {
            Chunk chunk = chunkBL;
            Init(chunk);
            chunkBL = null;
        }
        if (chunkTL != null) {
            Chunk chunk = chunkTL;
            Init(chunk);
            chunkTL = null;
        }
    }
    
    void TopRight() {
        print("Started Thread: (TR)");
        while (running) {
            int size = chunkSize - 1;
            for (int i = 0; i < distance; i++) {//for x (->) on the render distance
                for (int j = 0; j < distance; j++) {//for z (^)
                    //get nearest chunk / is also the offset of the grid we're about to generate
                    //pos + (spot * size) = position of chunk
                    int xPos = Slam(pos.x + (i * chunkSize), chunkSize),
                        zPos = Slam(pos.z + (j * chunkSize), chunkSize);

                    int LOD = getLOD(pos.x, pos.z, xPos, zPos);//  (64 / LOD) = # of vertices
                    float dis = Vector3.Distance(new Vector3(pos.x, 0, pos.z), new Vector3(xPos, 0, zPos));

                    if (dis < chunkSize * distance) {
                        if (chunkTR == null) {
                            Chunk chunk = new Chunk(xPos, zPos, chunkSize + LOD, LOD);
                            chunkTR = chunk;
                        }
                    }
                    
                }
            }
        }
    }
    void BottomRight() {
        print("Started Thread: (BR)");
        while (running) {
            int size = chunkSize - 1;
            for (int i = 0; i < distance; i++) {//for x (->) on the render distance
                for (int j = distance; j > 0; j--) {//j (^), is already at its peak
                    int xPos = Slam(pos.x + (i * chunkSize), chunkSize),
                        zPos = Slam(pos.z + (j * chunkSize) - ((distance + 1) * chunkSize), chunkSize);

                    int LOD = getLOD(pos.x, pos.z, xPos, zPos);//  (64 / LOD) = # of vertices
                    float CD = Vector3.Distance(new Vector3(pos.x, 0, pos.z), new Vector3(xPos, 0, zPos));//chunk distance

                    if (CD < chunkSize * distance) {
                        if (chunkBR == null) {
                            Chunk chunk = new Chunk(xPos, zPos, chunkSize + LOD, LOD);
                            chunkBR = chunk;
                        }
                    }
                }
            }
        }
    }
    void BottomLeft() {
        print("Started Thread: (BL)");
        while (running) {
            int size = chunkSize - 1;
            for (int i = distance; i > 0; i--) {
                for (int j = distance; j > 0; j--) {
                    int xPos = Slam(pos.x + (i * chunkSize) - ((distance + 1) * chunkSize), chunkSize),
                        zPos = Slam(pos.z + (j * chunkSize) - ((distance + 1) * chunkSize), chunkSize);

                    int LOD = getLOD(pos.x, pos.z, xPos, zPos);//  (64 / LOD) = # of vertices
                    float CD = Vector3.Distance(new Vector3(pos.x, 0, pos.z), new Vector3(xPos, 0, zPos));//chunk distance

                    if (CD < chunkSize * distance) {
                        if (chunkBL == null) {
                            Chunk chunk = new Chunk(xPos, zPos, chunkSize + LOD, LOD);
                            chunkBL = chunk;
                        }
                    }
                }
            }
        }
    }
    void TopLeft() {
        print("Started Thread: (TL)");
        while (running) {
            int size = chunkSize - 1;
            for (int i = distance; i > 0; i--) {
                for (int j = 0; j < distance; j++) {
                    int xPos = Slam(pos.x + (i * chunkSize) - ((distance + 1) * chunkSize), chunkSize),
                        zPos = Slam(pos.z + (j * chunkSize), chunkSize);

                    int LOD = getLOD(pos.x, pos.z, xPos, zPos);//  (64 / LOD) = # of vertices
                    float CD = Vector3.Distance(new Vector3(pos.x, 0, pos.z), new Vector3(xPos, 0, zPos));//chunk distance
                    if (CD < chunkSize * distance) {
                        if (chunkTL == null) {
                            Chunk chunk = new Chunk(xPos, zPos, chunkSize + LOD, LOD);
                            chunkTL = chunk;
                        }
                    }
                }
            }
        }
    }

    //MESH CREATOR--------------|
    void Init(Chunk chunk) {
        GameObject go = GameObject.Find(chunk.name);
        if (go) {
            if (go.GetComponent<MeshFilter>().sharedMesh.vertices.Length > chunk.v.Length ||
                go.GetComponent<MeshFilter>().sharedMesh.vertices.Length < chunk.v.Length) {//new chunk has better LOD
                GameObject.Destroy(go);
            } else {
                return;
            }
        }
        Texture2D texture = new Texture2D(chunk.size, chunk.size);
        
        texture.SetPixels(chunk.c);
        texture.Apply();
        //texture.filterMode = FilterMode.Bilinear; // NOT WORKING !?!?!?!
        Material _SplatMap = Instantiate(material);
        _SplatMap.SetTexture("_SplatMap", texture);

        Mesh mesh = new Mesh();
        mesh.vertices = chunk.v;
        mesh.triangles = chunk.t;
        mesh.uv = chunk.u;
        //mesh.normals = chunk.norm;
        mesh.RecalculateNormals();

        GameObject obj = new GameObject(chunk.name);
        obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();
        obj.AddComponent<MeshCollider>();
        obj.AddComponent<GizmoDisplay>();
        obj.AddComponent<Unloader>();
        //obj.AddComponent<TerrainTexturing>();
        obj.tag = "Terrain";
        
        obj.GetComponent<MeshFilter>().sharedMesh = mesh;
        obj.GetComponent<MeshRenderer>().material = _SplatMap;
        obj.GetComponent<MeshCollider>().sharedMesh = mesh;
        obj.transform.parent = GameObject.Find("TILES").transform;

        //create Grass
        
    }
    //MESH CREATOR--------------|
    //END THREAD----------------|
    void OnApplicationQuit() {
        print("Unloaded Thread: (TR)");
        running = false;//stop thread
        quit = true;//inform the program the player quit, and didn't close the game
    }

    void OnDestroy() {
        if (!quit) { //if player hasn't quit, they probably crashed or closed the game
            print("Closed / Crashed: (TR)");
            running = false;//emergency stop for thread
        }
    }
    //END THREAD----------------|
    //---------------------------------------------------------------------------------------------|
    //CALCULATORS---------------| (player pos / (chunksize - overlap)) rounded to nearest full number, then multiplied by the amount of chunks
    int Slam(float f, int size) {//returns nearest starting point of the size*size grid
        return (int)((f / size)) * size;
    }

    int getLOD(float xPlayer, float zPlayer, float xTile, float zTile) {
        int LOD = 32;
        Vector3 pl = new Vector3(xPlayer, 0, zPlayer),
                ti = new Vector3(xTile, 0, zTile);
        float dis = Vector3.Distance(ti, pl);
        int TEMP = chunkSize + (chunkSize / 2);

        //if (dis < TEMP * 5) {
        //    LOD = 16;
        //}
        //if (dis < TEMP * 4) {
        //    LOD = 8;
        //}
        //if (dis < TEMP * 3) {
        //    LOD = 4;
        //}
        //if (dis < TEMP * 2) {
        //    LOD = 2;
        //}
        if (dis < TEMP * 3) {
            LOD = 2;
        }
        return LOD;
    }
    //CALCULATORS---------------|
    //---------------------------------------------------------------------------------------------|

}