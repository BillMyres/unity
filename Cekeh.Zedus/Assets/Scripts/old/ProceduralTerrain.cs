using UnityEngine;
using System.Collections;

public class ProceduralTerrain : MonoBehaviour {

    public Material MATERIAL;

    public int width, height;
    public float amplitude = 3.5f, frequency = 0.25f, scale = 3f, xOffset, zOffset;

    public Texture heightMap;

    void Start() {
        if (!heightMap) { GenerateMaps(); }
    }

    void OnGUI() {
        if (heightMap) { GUI.DrawTexture(new Rect(10, 10, heightMap.width, heightMap.height), heightMap); }
    }







    /////////////////////////////////////////////////////
    void GenerateMaps() {
        //MAPS
        Texture2D HEIGHTS = new Texture2D(width, height);
        

        //LOOPS
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                //VARIATION
                int n = (z * width) + x;
                float amp = 1f, freq = 1f, yPosition = 0;

                //OCTAVES
                for (int i = 0; i < 4; i++) {
                    float tx = (x + 344883 + xOffset) / scale * freq,
                          tz = (z + 324894 + zOffset) / scale * freq;
                    yPosition += (Mathf.PerlinNoise(tx, tz) * 2 - 1) * amp;
                    amp  *= amplitude;
                    freq *= frequency;
                }

                //COLORS
                HEIGHTS.SetPixel(x, z, Color.Lerp(Color.black, Color.white, yPosition / 25));
                
            }
        }

        //APPLY / SET MAPS
        HEIGHTS.Apply();
        heightMap = HEIGHTS;

        //APPLY TO MESH RENDERER
        MATERIAL.SetTexture("_MainTex", HEIGHTS);
        MATERIAL.SetTexture("_ParallaxMap", HEIGHTS);


    }

}
