using UnityEngine;
using System.Collections;

public class Unloader : MonoBehaviour {

    GameObject player;
    Material material;
    Vector3 me, pl, center;

    Texture splatMap;

    int renderDistance = (GenerateTR.chunkSize * GenerateTR.distance) * 2;
    public bool showDistance = false;

    void Start() {
        material = GetComponent<MeshRenderer>().sharedMaterial;
        splatMap = material.GetTexture("_SplatMap");
        me = GetComponent<MeshFilter>().sharedMesh.vertices[0];
        player = GameObject.FindGameObjectWithTag("Player");
        int dis = ThreadedTerrain.sDistance,
            siz = ThreadedTerrain.size;
        //renderDistance = (siz - 1) * ((dis + 3) / 2) + ((dis + 3) * (siz - 1)) + siz;
        //print(GetComponent<MeshFilter>().sharedMesh.vertices[0]);
    }

    void Update() {
        material.SetFloat("_Perlinx", GameSettings._Perlinx);
        material.SetFloat("_Perlinz", GameSettings._Perlinz);
        material.SetFloat("_Scale", GameSettings._Scale);

        splatMap.filterMode = GameSettings.textureFilter;


        player = GameObject.FindGameObjectWithTag("Player");
        me = new Vector3(me.x, 0, me.z);
        pl = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        if (Vector3.Distance(me, pl) > renderDistance / 2) {
            GameObject.Destroy(transform.gameObject);
        }
    }

    void OnDrawGizmos() {
        if (showDistance) {
            int size = ThreadedTerrain.size;
            center = GetComponent<MeshFilter>().sharedMesh.vertices[0];
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(center, renderDistance / 2);
        }
    }
}