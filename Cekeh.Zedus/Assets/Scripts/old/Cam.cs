using UnityEngine;
using System.Collections;

public class Cam : MonoBehaviour {

    int x = 0;
    int z = 0;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
        }else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            x--;
            transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
        }else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            z++;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
        }else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            z--;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
        }
	}
}
