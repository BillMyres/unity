using UnityEngine;
using System.Collections;

public class GizmoDisplay : MonoBehaviour {

    Mesh mesh;
    public bool show = false, showNorms = false;
    public bool spitTriangles = false;

	void Start () {
        mesh = GetComponent<MeshFilter>().sharedMesh;
	}

    int ind = 0;
	void OnDrawGizmos() {
        if (ind >= mesh.vertices.Length) {
            ind = 0;
        }
        if (show) { 
            foreach (Vector3 v in mesh.vertices) {
                Gizmos.DrawIcon(v, "Vector.png");
            }
        }
        if (showNorms) {
            ind++;
            Vector3 me = mesh.vertices[ind];
            Gizmos.DrawLine(me, me + mesh.normals[ind]);

        } else {
            ind = 0;
        }
    }

    void Update() {
        if (spitTriangles) {
            spitTriangles = false;
            for(int i = 0; i < 12; i++) {
                print(mesh.triangles[i]);
            }
        }
    }
}
