using UnityEngine;
using System.Collections;

public class TerrainTexturing : MonoBehaviour {

    MeshRenderer mRend;

    public Texture2D texture;
    Texture2D terrainPNG;

    int chunkSize;

    void Start () {
        mRend = GetComponent<MeshRenderer>();

        //terrainPNG = GenerateTR.staticTerrainPNG;
        chunkSize = GenerateTR.chunkSize;
        

        Generate();
	}

    void Generate() {
        texture = new Texture2D(chunkSize * 4, chunkSize * 4);
        //Random.seed = (int)((transform.position.x * 3048204) + (transform.position.z * 30948));

        
        Color[] p = terrainPNG.GetPixels(Random.Range(0, 4) * terrainPNG.height, 0, 64, 64);
        texture.SetPixels(0, 0, 64, 64, p);

        texture.SetPixel(256, 255, Color.red);


        texture.filterMode = FilterMode.Point;
        //texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();

        mRend.material.mainTexture = texture;
	}
}

