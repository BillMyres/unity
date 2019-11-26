using UnityEngine;
using System.Collections;

public class deSpawn : MonoBehaviour {

    GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        
        Vector3 me = new Vector3(transform.position.x, 0, transform.position.z);
        foreach (GameObject go in GameObject.FindGameObjectsWithTag(transform.tag)) {
            Vector3 ro = new Vector3(go.transform.position.x, 0, go.transform.position.z);
            if (Vector3.Distance(me, ro) < 4f && transform.position != go.transform.position) {
                DestroyMyself();
            }
        }
        
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 me = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 pl = new Vector3(player.transform.position.x, 0, player.transform.position.z);

        if (Vector3.Distance(me, pl) > TerrainTest.spawnDistance) {
            DestroyMyself();
        }

	}

    void DestroyMyself() {
        GameObject.Destroy(transform.gameObject);
      //  if (transform.tag == "Rock") {
       ///     TerrainTest.rocksSpawned.Remove(transform.position);
       // }
       // if (transform.tag == "Tree") {
       //     TerrainTest.treesSpawned.Remove(transform.position);
       // }
        
    }
}
