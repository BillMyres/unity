using UnityEngine;
using System.Collections;


public class TerrainController : MonoBehaviour {


    public bool update = false, detail = false;
    int size;
    int detailWidth, detailHeight;

    Terrain t;//width 513
    Chunk chunk;

    Texture2D heightMap;


    void Start() {

        t = GetComponent<Terrain>();

        size = t.terrainData.heightmapWidth;
        detailWidth = t.terrainData.detailWidth;
        detailHeight = t.terrainData.detailHeight;

        chunk = new Chunk(5000, 5000, size, 1);

        heightMap = new Texture2D(size, size);
        heightMap.SetPixels(chunk.c);
        t.terrainData.SetHeights(0, 0, chunk.heights);
        //int[,] det = detailGetSet();
        //t.terrainData.SetDetailLayer(0, 0, 0, det);
        detailGetSet();
        t.Flush();
    }
    public int iX = 0, iZ = 0;
    void Update() {
        if (update) {
            Start();
            update = false;
        }
        if (detail) {
            detailGetSet();
            detail = false;
        }
    }
    
    void detailGetSet() {
        int count = 0;
        int[,] detail = t.terrainData.GetDetailLayer(0, 0, detailWidth, detailHeight, 0);
        print(detailWidth+", "+detailHeight);
        for (int i = 0; i < detailWidth; i++) {
            for (int j = 0; j < detailHeight; j++) {
                count++;
                detail[i, j] = 1;
            }
        }
        print(count);
        t.terrainData.SetDetailLayer(0, 0, 0, detail);
    }
}
