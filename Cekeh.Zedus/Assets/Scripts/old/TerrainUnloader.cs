using UnityEngine;
using System.Collections;

public class TerrainUnloader : MonoBehaviour {
    GameObject Player;
    public int Distance = 500;

	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update () {

        Vector3 firstVertex = transform.gameObject.GetComponent<MeshFilter>().sharedMesh.vertices[0];

        Vector3 p = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
        Vector3 o = new Vector3(firstVertex.x, 0, firstVertex.z);

        if (Vector3.Distance(p, o) > Distance) {
            print("Destroyed: "+ transform.name);
            GameObject.Destroy(transform.gameObject);
        }
    }
	
}
