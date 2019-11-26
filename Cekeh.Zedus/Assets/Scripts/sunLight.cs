using UnityEngine;
using System.Collections;

public class sunLight : MonoBehaviour {

    public float timeOfday = 0; //0-180
    public float speed = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update() {
        timeOfday += speed * Time.deltaTime;
        if (timeOfday > 360) {
            timeOfday = 0;
        }
        transform.rotation = Quaternion.Euler(new Vector3(timeOfday, -45, 0));


    }
}
