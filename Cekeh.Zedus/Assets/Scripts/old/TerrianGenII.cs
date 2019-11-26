using UnityEngine;
using System.Collections;

public class TerrianGenII : MonoBehaviour {

    Mesh MAP;
    Color[] colors;
    public GameObject VERTEX_MODEL;
    public Material MATT;

    int POS_X, POS_Y, SIZE, HEIGHT;
    float AMPLITUDE = 5;

    void Update() {
        if (Input.GetKeyUp(KeyCode.R)) {
            print("PRESSES R");
        }
        if (!MAP) {
            MAP = new Mesh();
            GENERATE_MAP(0, 0, 64, 64);
        }
        else {
            
            Graphics.DrawMesh(MAP, Vector3.zero, Quaternion.identity, MATT, 0);
        }
    }

    void GENERATE_MAP(int x, int y, int size, int height) {
        //SET SIZE FOR MAP GENERATION
        POS_X = x;
        POS_Y = y;
        SIZE = size;
        HEIGHT = height;

        //CREATE MESH
        colors = new Color[SIZE * SIZE];
        MAP.vertices = getVerticies();
        
        MAP.triangles = getTriangles();
        MAP.uv = getUVs();
        MAP.colors = colors;
        MAP.RecalculateNormals();
        //foreach (Vector3 v in MAP.vertices) {
        //Debug.Log(v.x+", "+v.y+", "+v.z);
        //Random.seed = (int)v.x * 39458 + (int)v.z * 238945;
        // GameObject.Instantiate(VERTEX_MODEL, new Vector3(v.x, getAVG((int)v.x, (int)v.z), v.z), Quaternion.identity);

        // }
        
        
    }

    //GENERATE MESH
    Vector3[] getVerticies() {
        Vector3[] v = new Vector3[SIZE * SIZE];
        
        for (int x = 0; x < SIZE; x++) {
            for (int z = 0; z < SIZE; z++) {
                //Random.seed = x * 42589 + z * 35082;
                int n = (z * (SIZE)) + x;
                v[n].x = x;
                v[n].z = z;
                //Debug.Log("x: "+x+", z: "+z+", #"+ ((z * (SIZE)) + x));
            }
        }
        
        return applyHeightBrush(v);
    }

    int[] getTriangles() {
        int[] t = new int[((SIZE - 1) * (SIZE - 1)) * 6];
        int count = 0;
        for (int x = 0; x < SIZE - 1; x++) {
            for (int z = 0; z < SIZE - 1; z++) {
                int n = ((z * (SIZE)) + x);
                t[count + 0] = n;
                t[count + 1] = n + SIZE;
                t[count + 2] = n + (SIZE + 1);

                t[count + 3] = n + (SIZE + 1);
                t[count + 4] = n + 1;
                t[count + 5] = n;
                
                count+=6;
            }
        }

        Debug.Log(t[0]+", "+t[1]+", "+t[2]+" : "+
            t[3]+", "+t[4]+", "+t[5]);
        return t;
    }

    Vector2[] getUVs() {
        Vector2[] u = new Vector2[MAP.vertices.Length];
        for (int i = 0; i < MAP.vertices.Length; i++) {
            u[i] = new Vector2(MAP.vertices[i].x, MAP.vertices[i].z);
        }
        return u;
    }

    Vector3[] applyHeightBrush(Vector3[] v) {
        for (int x = 0; x < SIZE; x++) {
            for (int z = 0; z < SIZE; z++) {
                int n = ((z * (SIZE)) + x);
                int height = getNoise(x, z);

                v[n].y += getSmoothNoise(x, z);

                if (height < 10) {//WATER
                    colors[n] = Color.blue;
                }
                if (height > 9 && height < 41) {//SAND
                    colors[n] = Color.grey;
                }
                if (height > 40) {//LAND
                    colors[n] = Color.green;
                }
                if (height < 1 || height > 100) {
                    Debug.Log("ERROR : e"+height+":2034987208374A-1\nheight incorrect value");
                }
            }
        }
        return v;
    }

   int getNoise(int x, int z) {
        Random.seed = x * 334566 + z * 92398;

        return Random.Range(1, 101);//1-100
    }

    float getSmoothNoise(int x, int z) {
        float corners =(getNoise(x - 1, z - 1)+
                        getNoise(x + 1, z + 1)+
                        getNoise(x - 1, z + 1)+
                        getNoise(x + 1, z - 1)) / 16;
        float sides   =(getNoise(x + 1, z) +
                        getNoise(x - 1, z) +
                        getNoise(x, z + 1) +
                        getNoise(x, z - 1)) / 8;
        return (corners + sides) / 10;
    }

    
}
