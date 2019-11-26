using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {

    Camera cam;
    GameObject car, player;

    public float distance = 5f;

	void Start () {
        cam = Camera.main;
        car = GameObject.FindGameObjectWithTag("Car");
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update () {
        if (GameObject.Find("Player").GetComponent<PlayerController>().inCar) {
            if (Input.GetKeyUp(KeyCode.E)) {
                print("Pressed E");
                //player.active = true;
                //player.transform.localPosition -= new Vector3(5, 0, 0);
                GameObject.Find("Player").GetComponent<PlayerController>().inCar = false;
                CarController.inCar = false;
                //player.transform.parent = null;
                player.transform.position = car.transform.position - (car.transform.forward * 5);
                //GameObject.Find("Player").active = true;
                car.tag = "Car";
                player.tag = "Player";
            }
        }
        if (GameObject.FindGameObjectWithTag("Car")) {
            car = GameObject.FindGameObjectWithTag("Car");
            if (Vector3.Distance(car.transform.position, player.transform.position) < distance) {
                if (Input.GetKeyUp(KeyCode.E)) {
                    //player.GetComponent<PlayerController>().inCar = true;
                    GameObject.Find("Player").GetComponent<PlayerController>().inCar = true;
                    CarController.inCar = true;
                    car.tag = "Player";
                    player.tag = "null";
                    //player.active = false;
                    //player.GetComponent<CapsuleCollider>().enabled = false;
                    //player.transform.parent = car.transform;
                    //player.transform.localPosition = Vector3.zero;
                    //player.transform.localRotation = Quaternion.Euler(0, 0, 0);

                    //GenerateTR.player = GameObject.FindGameObjectWithTag("Player");
                    //cam.GetComponent<Camera_v4>().position = car.transform;
                    //cam.GetComponent<Camera_v4>().lookAt = car.transform;
                }
            }
        }
        //if (Input.GetKeyUp(KeyCode.R) && ) { print("working..."); }
        
        
	}
}
