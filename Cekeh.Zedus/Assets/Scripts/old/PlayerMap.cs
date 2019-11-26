using UnityEngine;
using System.Collections;

public class PlayerMap : MonoBehaviour {

    GameObject player;
    public Material material;
    ArrayList meshEdge, meshFlat;

    public int debugDistance = 100;

    //Map settings
    int fullSize = 100;
    
	void Start () {
        //Set player object
        player = GameObject.FindGameObjectWithTag("Player");
        meshEdge = new ArrayList();
        meshFlat = new ArrayList();

    }
	
	void Update () {
        //Generate 3x3 grid
        for (int x = 0; x < 3; x++) {
            for (int z = 0; z < 3; z++) {
                //Get x,z for each loop
                float xTemp = (player.transform.position.x - fullSize) + (x * fullSize), 
                      zTemp = (player.transform.position.z - fullSize) + (z * fullSize);
                //Generate
                GenerateMesh(xTemp, zTemp);
            }
        }
	}

    void OnDrawGizmos() {
        bool red = false, green = false;
        Vector3 avg = Vector3.zero;
        if (red) { 
            Gizmos.color = Color.red;
            if (meshEdge.Count > 0) {
                foreach (Vector3 v in meshEdge) {
                    if (Vector3.Distance(v, player.transform.position) < debugDistance) {
                        //Gizmos.DrawWireSphere(v, 0.05f);
                        avg += v;
                        //lines
                        Vector3 pos = v + new Vector3(0, 0.05f, 0);

                        Vector3 right = new Vector3(1, 0, 0);
                        Vector3 forward = new Vector3(0, 0, 1);

                        if (meshEdge.Contains(v + right)) {
                            Gizmos.DrawLine(pos, v + right + new Vector3(0, 0.05f, 0));
                        }
                        if (meshEdge.Contains(v - right)) {
                            Gizmos.DrawLine(pos, v - right + new Vector3(0, 0.05f, 0));
                        }
                        if (meshEdge.Contains(v + forward)) {
                            Gizmos.DrawLine(pos, v + forward + new Vector3(0, 0.05f, 0));
                        }
                        if (meshEdge.Contains(v - forward)) {
                            Gizmos.DrawLine(pos, v - forward + new Vector3(0, 0.05f, 0));
                        }
                    }else {
                        //meshEdge.Remove(v);
                    }
                }
            }
        }
        avg /= meshEdge.Count;

        Gizmos.DrawWireSphere(avg, 1);

        if (green) { 
        Gizmos.color = Color.green;
            if (meshFlat.Count > 0) {
                foreach (Vector3 v in meshFlat) {
                    if (Vector3.Distance(v, player.transform.position) < debugDistance) { 
                        Gizmos.DrawWireCube(v, new Vector3(.75f, .1f, .75f));


                    }else {
                        meshEdge.Remove(v);
                    }
                }
            }
        }
        else {
            meshFlat = new ArrayList();
        }
    }


    void GenerateMesh(float xPos, float zPos) {
        //Size -1 so we don't throw errors
        int size = fullSize - 1;

        //Set start Pos
        int xTemp = (int)(xPos / size),
            zTemp = (int)(zPos / size),
            xOffs = xTemp * size,
            zOffs = zTemp * size;

        //If the mesh already exists , don't bother generating again
        if (GameObject.Find("Map [" + xTemp + ", " + zTemp + "]")) { return; }

        //Mesh init
        Mesh mesh = new Mesh();
        Vector3[] ver = new Vector3[fullSize * fullSize];
        Vector3[] nor = new Vector3[fullSize * fullSize];
        Vector2[] uvs = new Vector2[fullSize * fullSize];
        Color[] color = new Color[ver.Length];
        int[] tri = new int[(size * size) * 6];

        //Counters
        int tCount = 0;

        //MAIN LOOP
        for (int x = 0; x < fullSize; x++){
            for (int z = 0; z < fullSize; z++){
                //Get index = n
                int n = (z * fullSize) + x;

                //Get height of verticie
                float y = MapGeneration.getHeightAt(x + xOffs, z + zOffs);

                float h = 25f;
                if (y == h) {
                    if (MapGeneration.getHeightAt(x + xOffs + 1, z + zOffs + 0) == h &&
                        MapGeneration.getHeightAt(x + xOffs + 0, z + zOffs + 1) == h &&
                        MapGeneration.getHeightAt(x + xOffs - 1, z + zOffs + 0) == h &&
                        MapGeneration.getHeightAt(x + xOffs + 0, z + zOffs - 1) == h &&
                        MapGeneration.getHeightAt(x + xOffs + 1, z + zOffs + 1) == h &&
                        MapGeneration.getHeightAt(x + xOffs - 1, z + zOffs - 1) == h &&
                        MapGeneration.getHeightAt(x + xOffs + 1, z + zOffs - 1) == h &&
                        MapGeneration.getHeightAt(x + xOffs - 1, z + zOffs + 1) == h) {
                        if (!meshFlat.Contains(new Vector3(x + xOffs, y, z + zOffs))) {
                            meshFlat.Add(new Vector3(x + xOffs, y, z + zOffs));
                        }
                    }else{
                        if (!meshEdge.Contains(new Vector3(x + xOffs, y, z + zOffs))) { 
                            meshEdge.Add(new Vector3(x + xOffs, y, z + zOffs));
                        }
                    }
                }

                //Set height of verticie
                ver[n] = new Vector3(x + xOffs, y, z + zOffs);

                //Set triangles
                if(x < size && z < size) {
                    tri[tCount + 0] = n;
                    tri[tCount + 1] = n + fullSize;
                    tri[tCount + 2] = n + (fullSize + 1);

                    tri[tCount + 3] = n + (fullSize + 1);
                    tri[tCount + 4] = n + 1;
                    tri[tCount + 5] = n;

                    tCount += 6;
                }

                //Set uvs / normals
                uvs[n] = new Vector2(x, z);
                nor[n] = Vector3.up;

                //Colors
                if (y > 5.75f && y <= 6.5f) {
                    color[n] = new Color32(129, 191, 120, 1);
                }else if (y == 15) {
                    color[n] = new Color32(72, 133, 64, 1);
                }else if (y == 25) {
                    color[n] = new Color32(117, 117, 48, 1);
                }
                if (y > 6.5f && y < 15) {
                    color[n] = new Color32(33, 84, 29, 1);
                } else if (y > 15 && y < 25) {
                    color[n] = new Color32(64, 64, 25, 1);
                }
            }
        }
        //Set Mesh
        mesh.vertices = ver;
        mesh.triangles = tri;
        mesh.uv = uvs;
        mesh.normals = nor;
        mesh.colors = color;

        //CREATE OBJECT
        GameObject obj = new GameObject("Map [" + xTemp + ", " + zTemp + "]");
        obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();
        obj.AddComponent<TerrainUnloader>();
        obj.tag = "Terrain";

        //SET COMPONENTS
        obj.GetComponent<MeshFilter>().sharedMesh = mesh;
        obj.GetComponent<MeshRenderer>().sharedMaterial = material;
        obj.GetComponent<MeshRenderer>().receiveShadows = false;
    }
}
