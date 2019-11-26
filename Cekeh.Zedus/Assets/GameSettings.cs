using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour {

    public float perlinStepSpeed = 0.01f;

    [Range(1f,64f)]
    public float textureScale = 1.0f;
    public static float _Perlinx = 0.0f, _Perlinz = 0.0f, _Scale;

    public static FilterMode textureFilter = FilterMode.Point;
    public bool textureAA = false; //AntiAliasing

    void Start() {
        Update();
    }
    float x1 = 0, z1 = 0;
    
    void Update() {
        _Scale = textureScale;

        x1 += perlinStepSpeed;

        if (x1 > 50000) {
            x1 = 0;
            z1 += 0.01f;
        }
        if (z1 > 1000) { x1 = 0; z1 = 0; }

        _Perlinx = Mathf.PerlinNoise(x1, z1) * 2 - 1;
        _Perlinz = Mathf.PerlinNoise(x1 - 50000, z1 - 50000) * 2 - 1;

        if (textureAA) {
            textureFilter = FilterMode.Bilinear;
        } else {
            textureFilter = FilterMode.Point;
        }
    }
}
