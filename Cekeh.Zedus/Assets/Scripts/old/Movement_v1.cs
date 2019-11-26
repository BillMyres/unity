using UnityEngine;
using System.Collections;

public class Movement_v1 : MonoBehaviour {
    public float distance = 5000f;
    public float speed = 5f;
    Vector3 target;
    public GameObject obj;

    void Start () {
        target = Vector3.zero;
    }
	
	void Update () {
        
	 //if mouse button (left hand side) pressed instantiate a raycast
        if(Input.GetMouseButtonDown(0)){
            print("mouseDown(0)");
            //create a ray cast and set it to the mouses cursor position in game
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast (ray, out hit, distance)) {
                //draw invisible ray cast/vector
                Debug.DrawLine (ray.origin, hit.point);
                //log hit area to the console
                Debug.Log(hit.point);
                target = hit.point;
            }    
        }

        if (Vector3.Distance(transform.position, new Vector3(target.x, transform.position.y, target.z)) < 0.2f) { target = Vector3.zero; }
        if (target != Vector3.zero) {
            if (transform.position.x > target.x) {
                transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * speed);
            }else if (transform.position.x < target.x) {
                transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * speed);
            }
            if (transform.position.z > target.z) {
                transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime * speed);
            }else if (transform.position.z < target.z) {
                transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * speed);
            }
        }

        Camera camera = Camera.main;
        Vector3 p = camera.ViewportToWorldPoint(new Vector3(100, 100, camera.nearClipPlane));
        obj.transform.position = p;
    }
}
