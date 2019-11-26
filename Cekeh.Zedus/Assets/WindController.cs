using UnityEngine;
using System.Collections;

public class WindController : MonoBehaviour {

    Camera cam;
    GameObject player, grass;
    MeshRenderer mRend;

    Material material;
    float direction, speed, displacement, time, blending;
    
	void Start () {
        cam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        grass = GameObject.FindGameObjectWithTag("Grass");
        mRend = GetComponent<MeshRenderer>();
        material = GetComponent<MeshRenderer>().sharedMaterial;

        //transform.parent = grass.transform;
        //Update();
    }
    public bool show = false;
    void Update() {
        player = GameObject.FindGameObjectWithTag("Player");
        mRend = GetComponent<MeshRenderer>();

        if (player && mRend) {
            float dis = Vector3.Distance(transform.position, player.transform.position);
            if (dis > 80) {
                //show = false;
                //mRend.enabled = false;
            } else {
                show = true;
                mRend.enabled = true;
            }
        }
        float x1 = 0, z1 = 0;
        if (show) {
            //GET
            //direction = GameSettings.s_Direction;
            //speed = GameSettings.s_Windspeed;

            //displacement = GameSettings.s_ShakeDisplacement;
            //time = GameSettings.s_ShakeTime;
            //blending = GameSettings.s_ShakeBending;

            ////SET
            //material.SetFloat("WindDirectionx", direction);
            //material.SetFloat("WindDirectionz", direction);
            //material.SetFloat("_ShakeWindspeed", speed);

            //material.SetFloat("_ShakeDisplacement", displacement);
            //material.SetFloat("_ShakeTime", time);
            //material.SetFloat("_ShakeBending", blending);

            if (x1 > 50) {
                x1 = 0;
                z1++;
            }
            if (z1 > 1000) { x1 = 0; z1 = 0; }
            Random.seed = (int)((x1 * 3453) + (z1 * 87344));
            material.SetFloat("_Time00", time);



            Vector3 v = new Vector3(cam.transform.position.x, 0, cam.transform.position.z);
            transform.LookAt(v);
        }
    }
}
