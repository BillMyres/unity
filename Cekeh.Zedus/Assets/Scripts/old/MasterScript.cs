using UnityEngine;
using System.Collections;

public class MasterScript : MonoBehaviour {

    Mesh[] chunk;
    public Material Matt;
    public Camera Cam;

    int count = 0;

    int x = 0;
    int z = 0;

	// Use this for initialization
	void Start () {
        chunk = new Mesh[64];
        chunk[count] = GeneratePlane.GenerateChunk(x, z);
        count++;
    }
	
	// Update is called once per frame
	void Update () {
        if (count < 64) { 
            if (Input.GetKeyUp(KeyCode.LeftArrow)) {
                x -= 32 - 1;
                Cam.transform.Translate(new Vector3(-31, 0, 0), Space.World);
                chunk[count] = GeneratePlane.GenerateChunk(x, z);
                count++;
            } else if (Input.GetKeyUp(KeyCode.UpArrow)) {
                z += 32 - 1;
                Cam.transform.Translate(new Vector3(0, 0, +31), Space.World);
                chunk[count] = GeneratePlane.GenerateChunk(x, z);
                count++;
            } else if (Input.GetKeyUp(KeyCode.DownArrow)) {
                z -= 32 - 1;
                Cam.transform.Translate(new Vector3(0, 0, -31), Space.World);
                chunk[count] = GeneratePlane.GenerateChunk(x, z);
                count++;
            } else if (Input.GetKeyUp(KeyCode.RightArrow)) {
                x += 32 - 1;
                Cam.transform.Translate(new Vector3(+31, 0, 0), Space.World);
                chunk[count] = GeneratePlane.GenerateChunk(x, z);
                count++;
            }
        }

        foreach (Mesh m in chunk) {
            if (m != null) {
                Graphics.DrawMesh(m, Vector3.zero, Quaternion.identity, Matt, 0);
            }
        }

	}
}
