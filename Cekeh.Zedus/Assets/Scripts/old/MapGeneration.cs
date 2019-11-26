using UnityEngine;
using System.Collections;

public class MapGeneration : MonoBehaviour {
	
	static Vector2 seed = new Vector2(344883, 324894);
	
	[Range(0.25f, 4f)]
    static float amplitude = 3.5f, frequency = 0.25f, scale = 4f;
	
    //Request height at any point of the map
	public static float getHeightAt(float x, float z){
        float amp = 1f, freq = 1f, y = 0;
		
		//PerlinNoise Generation
		for (int i = 0; i < 4; i++) {
			float tX = (x + seed.x) / scale * freq,
				  tZ = (z + seed.y) / scale * freq;
			y += (Mathf.PerlinNoise(tX, tZ) * 2 - 1) * amp;
			
			amp *= amplitude;
			freq *= frequency;
		}
		
		//Setting flat spots
		if (y > 6.5f && y < 11) { y = 6.5f; }
		if (y > 11) { y = y - 4.5f; }

		if (y > 15 && y < 18) { y = 15; }
		if (y > 18) { y = y - 3; }

		if (y > 25 && y < 30) { y = 25;	}
		if (y > 30) { y = y - 5; }
		
		return y;
	}
	
}