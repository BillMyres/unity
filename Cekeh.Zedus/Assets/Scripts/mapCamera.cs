using UnityEngine;
using System.Collections;

public class mapCamera : MonoBehaviour {

    GameObject player;

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update () {
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 pl = player.transform.position;
        transform.position = new Vector3(pl.x, 400, pl.z);
	}
}
