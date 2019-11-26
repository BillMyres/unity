using UnityEngine;
using System.Collections;

public class waterStay : MonoBehaviour {
    GameObject player;
    public float waterHeight = 6f;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.rotation = Quaternion.identity;
    }
	
	// Update is called once per frame
	void Update () {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = new Vector3(player.transform.position.x, waterHeight, player.transform.position.z);
        
	}
}
