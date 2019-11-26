using UnityEngine;
using System.Collections;

class Chunk {
    //public Mesh mesh = new Mesh();
    public string name;//name will be searched to make sure it doesn't already exists

    public int size;//grid will be size*size big
    int xPos, zPos;//position of the first vertice
    //ArrayList trees, rocks;//TODO

    public int LOD; //1=64v .5=32v .25=16v LOD_1, LOD_2, LOD_3 (AKA, Level of Detail, or scale)
    //1=quarter 2=half 4=full
    // 64v=1unit 32v=2unit 16v=4unit
    // 64/64=1unit 64/32=2unit 64/16=4unit
    // 64*1=64 64*.5=32 64*.25=16
    public Vector3[,] detailPos;
    public float[,] heights;

    public Color[] c;//colors
    public Vector3[] v;//vertices
    public Vector2[] u;//uvs
    public int[] t;//triangles
    public Vector3[] norm;
    private float scale0 = 20f, frequency0 = 0.25f, amplitude0 = 5f;
    private float scale1 = 16f, frequency1 = 0.15f, amplitude1 = 8f;//scale 4
    int octaves = 4;

    public Chunk(int xPos, int zPos, int size, int LOD) {//xposition, zposition, full size
        this.xPos = xPos;
        this.zPos = zPos;
        this.LOD = LOD;
        this.size = size / LOD;//64 is temp, casting to int because im using base64, so i know it will divide evenly

        detailPos = new Vector3[size, size];
        heights = new float[size, size];

        int s = size + 2;
        c = new Color[s * s];
        v = new Vector3[s * s];
        u = new Vector2[s * s];
        t = new int[(s * s) * 6];
        norm = new Vector3[s * s];

        name = xPos + ", " + zPos;

        generate();
    }

    float hold = 0;
    void generate() {
        int count = 0;//used to keep track of all the f*cking triangles

        int xEnd = xPos + ((size) * LOD),//the last vertice position
            zEnd = zPos + ((size) * LOD);//(64 * lod) = verts, (64 / verts) = scale, (size * scale) = same size different LOD

        //set x = xpos so I don't have to add an offset later each time I want to use x or z
        for (int x = xPos; x < xEnd; x += LOD) {//for the x (->) position of every vertice
            for (int z = zPos; z < zEnd; z += LOD) {//for the z (^) position
                int n = (((z - zPos) / LOD) * size) + ((x - xPos) / LOD);//get index (ie. vertice[n] instead of vertice[(row * size) + col])

                float y = getHeight(x, z);//set y position equal to the perlin noise(x, z)

                if (x == 4600 && z == 2600) { hold = y; }

                v[n] = new Vector3(x, y, z);//set vertice position

                if (x < xEnd - LOD && z < zEnd - LOD) {//make sure we arn't on the edge, there would be no triangle to make
                    t[count++] = n;//bottom left (v)
                    t[count++] = n + size;//top left (v)
                    t[count++] = n + size + 1;//top right (v)

                    t[count++] = n + size + 1;//top right (v)
                    t[count++] = n + 1;//bottom right (v)
                    t[count++] = n;//bottom left (v)
                }
                int ss = size * 4;
                float frac = 1f / ss;
                u[n] = new Vector2(x * 0.0078125f, z * 0.0078125f);//set uv for the vertice

                //norm[n] = Vector3.up;

                Vector3 s1 = new Vector3(x - 1, getHeight(x - 1, z), z),
                        s2 = new Vector3(x + 1, getHeight(x + 1, z), z),
                        s3 = new Vector3(x, getHeight(x, z - 1), z - 1),
                        s4 = new Vector3(x, getHeight(x, z + 1), z + 1);
                //norm[n] = Quaternion.;
                norm[n] = Vector3.up;

                int r = 0, g = 0, b = 0;
                //System.Random rand = new System.Random((x * 34879) + (z * 9832));
                float rand = (y * 47293873) %20;
                float sand = 10 + rand,
                      grass = 60 + rand;
                
                if (y < sand) {
                    b = 255;
                }
                if (y >= sand ) {//&& y < grass
                    g = 255;
                    b /= 2;
                }
                if (y >= grass) {
                    //r = 255;
                }
                float dis = .25f;//CAN BE SIMPLIFIED
                if (getHeight(x - 1, z) - y > dis ||
                    getHeight(x + 1, z) - y > dis ||
                    getHeight(x, z - 1) - y > dis ||
                    getHeight(x, z + 1) - y > dis) {
                    r = 128;
                    g = 0;
                    b = 0;
                }
                
                c[n] = new Color(r, g, b);

                heights[x - xPos, z - zPos] = y / 500;
                detailPos[x - xPos, z - zPos] = new Vector3(xPos + x, y, zPos + z);
                //c[n] = Color.Lerp(Color.white, Color.black, y + 50 / 100);
                //c[n] = Color.red;
            }
        }
    }

    float getHeight(float x, float z) {
        float y0 = 0, y1 = 0;//init y so we can do some perlin layering
        float amp0 = 1f, fre0 = 1f, amp1 = 1f, fre1 = 1f;//settings for the perlin generator built into unity (Mathf.perlin(x, z))

        for (int i = 0; i < octaves; i++) {//for each "octave" || (+=)each layer we add of ~random numbers
            float x0Sample = x / scale0 * fre0,//adjust x and z so they work with perlinNoise
                  z0Sample = z / scale0 * fre0,
                  x1Sample = x / scale1 * fre1,//sample and add later for more detail, not so flat
                  z1Sample = z / scale1 * fre1;

            //add to the height of the vertice
            y0 += (Mathf.PerlinNoise(x0Sample, z0Sample) * 2 - 1) * amp0;//must multiply by small float to get perlinNoise to work, will get same result when using an int
            y1 += (Mathf.PerlinNoise(x1Sample, z1Sample) * 2 - 1) * amp1;
            //perlin * 2 - 1, makes 0>1 turn to -1>1, giving more detail

            amp0 *= amplitude0;//change amplitude, makes bigger mountains
            fre0 *= frequency0;//change frequency, make mountains more frequent
            amp1 *= amplitude1;
            fre1 *= frequency1;

        }
        return y0 + y1;
    }
}