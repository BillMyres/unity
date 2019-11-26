using UnityEngine;
using System.Collections;

public class Perlin : MonoBehaviour {
    
    float lerp(float a, float b, float n) {
        return (1.0f - n) * a + n * b;
    }
}
