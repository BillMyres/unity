using UnityEngine;
using System.Collections;

public class Player_v1 : MonoBehaviour {

    Animator animator;
   // Rigidbody rBody;
    CharacterController controller;
    public float speed = 40f,
                 jumpSpeed = .25f;
    public float gravity = 1f;
    [Range(1, 10)]
    public float scrollSpeed = 8.1f;
    Vector2 mousePOS;
    public float rotationX, rotationY;

    public bool teleport = false;
    public GameObject TP, cube;

    Vector3 vel = Vector3.zero;
    float vSpeed = 0;

    Transform cam;

    Vector2 chunk;

    public static bool inBound = true;
    public float adjustHeight = 0f;

    int mapX = 0,
        mapY = 0;

    void Start () {
        if (PlayerPrefs.GetFloat("x") != 0 && PlayerPrefs.GetFloat("z") != 0) {
            float x = PlayerPrefs.GetFloat("x"),
                  y = PlayerPrefs.GetFloat("y"),
                  z = PlayerPrefs.GetFloat("z");
            transform.position = new Vector3(x, y+0.1f, z);
            int size = ThreadedTerrain.size;
            int xOffset = (int)(x / (size - 1)) * (size - 1),
                zOffset = (int)(z / (size - 1)) * (size - 1);
            chunk = new Vector2(xOffset, zOffset);
        }
        // rBody = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        animator = GetComponent<Animator>();
        mousePOS = Input.mousePosition;
    }

    void OnGUI() {
        
    }

    void FixedUpdate() {
        //cControl.SimpleMove(-transform.up * Time.deltaTime * gravity);
        int size = ThreadedTerrain.size;
        int xOffset = (int)(transform.position.x / (size)) * (size),
            zOffset = (int)(transform.position.z / (size)) * (size);
        chunk = new Vector2(xOffset, zOffset);
        string name = chunk.x + ", " + chunk.y;
        if (GameObject.Find(name)) {
            if (controller.isGrounded) {
                vSpeed = -gravity;
                if (Input.GetKeyDown(KeyCode.Space)) {
                    vSpeed = jumpSpeed;
                }
            } else {
                vSpeed -= gravity * Time.deltaTime;
            }
            vel.y = vSpeed;
        }

        
        controller.Move(vel);


        //MOUSE
        Camera_v4.staticDistance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        if (Input.GetMouseButtonDown(2)) {
            mousePOS = Input.mousePosition;
        }else if (Input.GetMouseButton(2)) {
            //float d = Vector2.Distance(mousePOS, Input.mousePosition);
            float distanceX = Input.mousePosition.x - mousePOS.x;
            float distanceY = (Input.mousePosition.y - mousePOS.y) * -1;
            
            rotationX = distanceX;
            rotationY = distanceY;
            
        }
        Camera_v4.staticPositionX = rotationX;
        Camera_v4.staticPositionY = rotationY;

        //MOVEMENT
        if (Input.GetKey(KeyCode.W)) {
            animator.SetBool("Running", true);
            //transform.position += transform.forward * Time.deltaTime * speed;
            //rBody.MovePosition(transform.position + transform.forward * Time.deltaTime * speed);
            vel = transform.forward * speed * Time.deltaTime;
        } else {
            animator.SetBool("Running", false);
            vel.x = 0;
            vel.z = 0;
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.position -= (transform.forward * Time.deltaTime) *speed;
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.position += (transform.right * Time.deltaTime) *speed;
        }else if (Input.GetKey(KeyCode.A)) {
            transform.position -= (transform.right * Time.deltaTime) *speed;
        }
        if (Input.GetKeyUp(KeyCode.W)) { 
            
        }
        
        if (animator.GetBool("Running") == true) { 
            transform.rotation = Quaternion.Euler(0, rotationX, 0);
        }

        if (Input.GetKeyUp(KeyCode.T)) {
            teleport = true;
        }

        if (teleport) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && teleport) {

                //telePos = hit.point;
                TP.transform.position = hit.point + new Vector3(0, 0.01f, 0);

                if (Input.GetMouseButtonDown(0)) {
                    print(hit.point);
                    transform.position = hit.point;
                    teleport = false;
                    TP.transform.position = new Vector3(0, -100, 0);
                }
            }
                
            
        }
    }
    bool prefs = false;
    void OnApplicationQuit() {
        if (!prefs) {
            prefs = true;
            print("SavedPlayerPrefs");
            PlayerPrefs.SetFloat("x", transform.position.x);
            PlayerPrefs.SetFloat("y", transform.position.y);
            PlayerPrefs.SetFloat("z", transform.position.z);
        }
    }

    void OnDestroy() {
        if (!prefs) {
            prefs = true;
            print("SavedPlayerPrefs");
            PlayerPrefs.SetFloat("x", transform.position.x);
            PlayerPrefs.SetFloat("y", transform.position.y);
            PlayerPrefs.SetFloat("z", transform.position.z);
        }
    }
}
