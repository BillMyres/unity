using UnityEngine;
using System.Collections;
using System.Threading;

public class ThreadedTerrain : MonoBehaviour {
    GameObject player, TILES;
    Texture map;

    Thread t;
    bool running = true;

    public static int size = 64;//64
    public Material material;
    Mesh mesh;
    MeshFilter mFilter;
    MeshRenderer mRender;

    GameObject obj;
    public GameObject treeObj, rockObj, ROCK2;
    bool updateMesh = false;
    Color[] col;
    Vector3[] ver;
    Vector2[] _uv;
    int[] tri;

    ArrayList trees, rocks;

    public Vector3 playerPos;
    float playerHeight = 0;

    [Range(1, 20)]
    public int rDistance = 12;
    public static int sDistance;

    void Start() {
        print("Script : START");
        trees = new ArrayList();
        rocks = new ArrayList();
        TILES = GameObject.Find("TILES");
        sDistance = rDistance;
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform.position;

        mesh = new Mesh();
        mFilter = GetComponent<MeshFilter>();
        mRender = GetComponent<MeshRenderer>();

        t = new Thread(Run);
        t.Start();
    }

    void Update() {//60FPS
        sDistance = rDistance;
        playerHeight = getHeight(playerPos.x, playerPos.z);
        //player.transform.position = new Vector3(player.transform.position.x, playerHeight, player.transform.position.z);
        playerPos = player.transform.position;
        
        if (updateMesh) {
            string name = ver[0].x + ", " + ver[0].z;
            mesh = new Mesh();
            mesh.vertices = ver;
            mesh.uv = _uv;
            mesh.triangles = tri;
            mesh.colors = col;
            if (!GameObject.Find(name)) {
                obj = new GameObject(name);
                obj.AddComponent<MeshFilter>();
                obj.AddComponent<MeshRenderer>();
                obj.AddComponent<MeshCollider>();
                obj.AddComponent<Unloader>();

                obj.tag = "Terrain";

                obj.GetComponent<MeshFilter>().sharedMesh = mesh;
                obj.GetComponent<MeshRenderer>().sharedMaterial = material;
                obj.GetComponent<MeshCollider>().sharedMesh = mesh;
                obj.transform.parent = TILES.transform;
            }
            col = null;
            ver = null;
            _uv = null;
            tri = null;
            obj = null;

            foreach (tree t in trees) {
                //print("Tree Planted");
                if (!GameObject.Find("TREE: " + t.pos)) {
                    GameObject temp = Instantiate(treeObj);
                    System.Random rp = new System.Random((int)((t.pos.x * 30894) + (t.pos.z * 92934)));
                    float rand = rp.Next() % 10;
                    temp.name = "TREE: " + t.pos;
                    temp.transform.parent = GameObject.Find(t.name).transform;
                    float tpos = (rand);
                    temp.transform.localScale *= tpos;
                    temp.transform.position = t.pos;
                }
            }
            trees = new ArrayList();

            foreach (tree t in rocks) {
                //print("Tree Planted");
                if (!GameObject.Find("ROCK: " + t.pos)) {
                    Transform tr = rockObj.transform;
                    System.Random rp = new System.Random((int)((t.pos.x * 30894) + (t.pos.z * 92934)));
                    float rand = rp.Next() % 25;
                    //print(rand);
                    if (rand > 12) {
                        tr = rockObj.transform;
                    } else {
                        tr = ROCK2.transform;
                    }
                    GameObject temp = Instantiate(tr.gameObject);
                    temp.name = "ROCK: " + t.pos;

                    temp.transform.parent = GameObject.Find(t.name).transform;
                    float tpos = rand;
                    temp.transform.localScale *= tpos + 1;
                    temp.transform.localRotation = Quaternion.Euler(new Vector3(random(t.pos.x + tpos, t.pos.z, 360), random(t.pos.x, t.pos.z + tpos, 720), random(t.pos.x + tpos, t.pos.z + tpos, 1080)));
                    temp.transform.position = t.pos;
                }
            }
            rocks = new ArrayList();

            updateMesh = false;
        }
//        ArrayList tem = trees;
//        trees = new ArrayList();
//        if (tem != new ArrayList()) {
//            //print(tem.Count);
        
        //            tem = new ArrayList();
        //        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            print(random(Random.Range(0, 10), Random.Range(0, 10), 100));
        }
    }

    void OnDrawGizmos() {
        
    }

    void Run() {//ASAP!!!
        print("GO!");
        trees = new ArrayList();
        while (running) {
            checkInBound();
            int rDis = rDistance + 3;
            for (int i = 0; i < rDis; i++) {//i = x : j = z
                for (int j = 0; j < rDis; j++) {
                    float xPos = playerPos.x - ((size - 1) * ((rDis) / 2)) + (i * (size - 1)),
                          zPos = playerPos.z - ((size - 1) * ((rDis) / 2)) + (j * (size - 1));
                    //if (trees.) { }
                    CalculateTerrain(xPos, zPos);
                } 
            }

        }
    }
    Color[] colors;
    void CalculateTerrain(float xPos, float zPos) {//Self explanatory
        int xOffset = (int)(xPos / (size - 1)) * (size - 1),
            zOffset = (int)(zPos / (size - 1)) * (size - 1);

        ArrayList theTrees = new ArrayList();
        ArrayList theRocks = new ArrayList();

        Vector3 v00 = Vector3.zero;
        Vector3[] vertices = new Vector3[size * size];
        Vector2[] uvs = new Vector2[size * size];
        int[] triangles = new int[size * size * 6];
        colors = new Color[size * size];

        int count = 0;

        for (int x = 0; x < size; x++) {
            for (int z = 0; z < size; z++) {
                int n = (z * size) + x;
                Color c = new Color32(72, 155, 72, 255);
                float xTemp = x + xOffset, zTemp = z + zOffset;
                /////////////////////////////////////
                float height = 0;
                float a1 = 1f, a2 = 1f, a3 = 1f,//amplitude
                      f1 = 1f, f2 = 1f, f3 = 1f,//frequency
                      y1 = 0f, y2 = 0f, y3 = 0f;//height

                for (int i = 0; i < 4; i++) {
                    float x1 = xTemp / scale1 * f1, z1 = zTemp / scale1 * f1,
                          x2 = xTemp / scale2 * f2, z2 = zTemp / scale2 * f2,
                          x3 = xTemp / scale2 * f3, z3 = zTemp / scale2 * f3;

                    y1 += (Mathf.PerlinNoise(x1, z1) * 2 - 1) * a1;
                    y2 += (Mathf.PerlinNoise(x2, z2) * 2 - 1) * a2;
                    y3 += (Mathf.PerlinNoise(x3, z3) * 2 - 1) * a3;

                    float y5 = (Mathf.PerlinNoise(x1 / 10, z1 / 10) * 2 - 1) * a1;
                    y1 += y5;
                    y2 += y5;
                    y3 += y5;

                    a1 *= amplitude1; a2 *= amplitude2; a3 *= amplitude3;
                    f1 *= frequency1; f2 *= frequency2; f3 *= frequency3;
                }

                if (y2 > y1) { y1 = y2;}
                y3 += y1;
                if (y3 > y1) { y1 = y3; }

                height = y1;
                ///////////////////////////////////////
                vertices[n] = new Vector3(x + xOffset, height, z + zOffset);
                if (x == 0 && z == 0) { v00 = new Vector3(x + xOffset, height, z + zOffset); }
                if (x < size - 1 && z < size - 1) {
                    triangles[count + 0] = n;
                    triangles[count + 1] = n + size;
                    triangles[count + 2] = n + (size + 1);

                    triangles[count + 3] = n + (size + 1);
                    triangles[count + 4] = n + 1;
                    triangles[count + 5] = n;

                    count += 6;
                }
                uvs[n] = new Vector2(x + xOffset, z + zOffset);

                c = Color.Lerp(Color.black, Color.green, height/50);
                colors[n] = c;

                //TREES
                //Random.seed = (x * 234897 + xOffset) * (z * 390847 + zOffset);
                
                float r = random(x + xOffset, z + zOffset, 5000);
                if (r == 84 && height > 6) { 
                    string s = v00.x + ", " + v00.z;
                    Vector3 v = new Vector3(x + xOffset, height, z + zOffset);
                    theTrees.Add(new tree(s, v));
                }
                float te = random(x + xOffset, z + zOffset, 10000);
                if (te == 99) { 
                    string s = v00.x + ", " + v00.z;
                    Vector3 v = new Vector3(x + xOffset, height, z + zOffset);
                    theRocks.Add(new tree(s, v));
                }
            }
        }
        if (!updateMesh) {
            trees = theTrees;
            rocks = theRocks;
            ver = vertices;
            tri = triangles;
            _uv = uvs;
            col = colors;
            updateMesh = true;
        }
    }

    void checkInBound() {
        if (playerHeight != getHeight(playerPos.x, playerPos.z)) {
            //playerHeight = getHeight(playerPos.x, playerPos.z);
        }
    }

    void OnApplicationQuit() {
        print("OnApplicationQuit : Stoped Play Mode");
        running = false;
    }

    void OnDestroy() {
        print("OnDestroy : Closed Application");
        running = false;
    }
    
    public static float scale1     = 20,   scale2     = 15,  scale3     = 3,
                 amplitude1 = 6,    amplitude2 = 5,   amplitude3 = 4,
                 frequency1 = .25f, frequency2 = .3f, frequency3 = .5f;

    public static float getHeight(float x, float z) {
        float height = 0;
        float a1 = 1f, a2 = 1f, a3 = 1f,//amplitude
              f1 = 1f, f2 = 1f, f3 = 1f,//frequency
              y1 = 0f, y2 = 0f, y3 = 0f;//height

        for (int i = 0; i < 4; i++) {
            float x1 = x / scale1 * f1, z1 = z / scale1 * f1,
                  x2 = x / scale2 * f2, z2 = z / scale2 * f2,
                  x3 = x / scale2 * f3, z3 = z / scale2 * f3;

            y1 += (Mathf.PerlinNoise(x1, z1) * 2 - 1) * a1;
            y2 += (Mathf.PerlinNoise(x2, z2) * 2 - 1) * a2;
            y3 += (Mathf.PerlinNoise(x3, z3) * 2 - 1) * a3;

            float y5 = (Mathf.PerlinNoise(x1 / 10, z1 / 10) * 2 - 1) * a1;
            y1 += y5;
            y2 += y5;
            y3 += y5;

            a1 *= amplitude1; a2 *= amplitude2; a3 *= amplitude3;
            f1 *= frequency1; f2 *= frequency2; f3 *= frequency3;
        }

        if (y2 > y1) { y1 = y2; }
        y3 += y1;
        if (y3 > y1) { y1 = y3; }

        height = y1;

        return height;
    }
    
    float random(float x, float z, int limit) {
        System.Random rnd = new System.Random(546 + (int)(x * 348794) + (int)(z * 1112));
        float f = rnd.Next(limit);
        return f;
    }
}
class tree {
    public string name;
    public Vector3 pos;

    public tree(string s, Vector3 v) {
        name = s;
        pos = v;
    }
}