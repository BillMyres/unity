using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    Animator animator;
    public Camera camera;
    CharacterController controller;

    public GameObject GrassPrefab;

    int chunkSize;
    int currentChunk_x, currentChunk_z;

    public float gravity = 4.5f;
    Vector3 velocity = Vector3.zero;
    public float xVel, yVel, zVel;
    Vector3 maxVel = new Vector3(.5f, 10f, .5f);

    public bool inCar = false;

    public float speed = 0, maxSpeed = 5, acceleration = 1, jump = 0.25f;

    void Start() {
        //init
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        chunkSize = GenerateTR.chunkSize;

        //set position
        transform.position = LoadPrefsPosition() + new Vector3(0, 10f, 0);
        
    }
    public bool go = false;
    void FixedUpdate() {
        if (!inCar) {
            //player's chunk
            currentChunk_x = (int)(transform.position.x / (chunkSize)) * (chunkSize);
            currentChunk_z = (int)(transform.position.z / (chunkSize)) * (chunkSize);

            string name = currentChunk_x + ", " + currentChunk_z;
            if (GameObject.Find(name)) {//IF CHUNK EXISTS
                                        //yVel = 0;
                if (controller.isGrounded) {
                    //PLAYER IS ON THE GROUND
                    //yVel = 0;
                    if (Input.GetKeyDown(KeyCode.Space)) {
                        yVel = jump;
                    }
                } else {
                    yVel -= gravity * Time.deltaTime;
                }
            }

            //MOVEMENT
            if (speed < 10) { speed = 10f; }
            if (Input.GetKey(KeyCode.W)) {
                animator.SetBool("Running", true);
                //velocity += transform.forward * speed * Time.deltaTime;
                if (speed < maxSpeed) {
                    speed += acceleration * Time.deltaTime;
                }


                xVel = transform.forward.x * speed * Time.deltaTime;
                zVel = transform.forward.z * speed * Time.deltaTime;
            } else {
                speed = 10f;
                animator.SetBool("Running", false);
                xVel = 0;
                zVel = 0;
            }
            if (xVel > maxVel.x) { xVel = maxVel.x; }
            if (xVel < -maxVel.x) { xVel = -maxVel.x; }

            if (yVel > maxVel.y) { yVel = maxVel.y; }
            if (yVel < -maxVel.y) { yVel = -maxVel.y; }

            if (zVel > maxVel.z) { zVel = maxVel.z; }
            if (zVel < -maxVel.z) { zVel = -maxVel.z; }

            velocity = new Vector3(xVel, yVel, zVel);

            controller.Move(velocity);
        }

        RaycastHit hit;
        Vector3 screenToWorldPoints = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.nearClipPlane));
        //Vector3 screenToWorldPoint = camera.(Input.mousePosition);
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100)) {
            if (hit.transform.tag == "Terrain" || hit.transform.tag == "Car") {
                if (Input.GetMouseButtonUp(1)) {
                    //right click option when cursor is >100m away and on the terrain
                    tag = hit.transform.tag;
                    RightClick();
                }
            }
            mousePosition3D = hit.point;
        }
    }
    string tag = "";
    public Vector3 mousePosition3D;
    public Vector2 mousePosition2D = Vector2.zero;
    public GameObject spawnTarget;
    public GameObject teeemp;
    bool spawned = true;

//------Right Click Menu------

    bool terrainMenu = false, clicked = false;
    Vector2 menuPosition;
    Vector2 menuSize = new Vector2(220, 25);
    Rect menuRectangle;
    public GUISkin Skin;

    void RightClick() {
        clicked = true;//print("rightclick: "+mousePosition2D);
        terrainMenu = true;
    }
    void OnGUI() {
        GUI.skin = Skin;
        mousePosition2D = Event.current.mousePosition;
        if (clicked) {
            clicked = false;
            print(mousePosition2D);
            menuPosition = mousePosition2D;
        }

        if (terrainMenu) {
            menuRectangle = new Rect(menuPosition + new Vector2(1, 1), menuSize);
            if (tag == "Terrain") {
                GUI.Button(menuRectangle, "Place Vehicle");
            }else if (tag == "Car") {
                GUI.Button(menuRectangle, "Pick-Up Vehicle");
            }

        }
    }
    bool usingVeicle = false;
    void Spawn() {
        if (!usingVeicle && tag == "Terrain") {
            usingVeicle = true;
            Instantiate(teeemp, mousePosition3D + new Vector3(0, 1, 0), Quaternion.identity);
            tag = "";
        } else if(tag == "Car") {
            usingVeicle = false;
            Destroy(GameObject.FindGameObjectWithTag(tag));
            tag = "";
        }
    }
    void Update() {
        if (Input.GetMouseButtonUp(0)) {
            terrainMenu = false;
            if (menuRectangle.Contains(mousePosition2D)) {
                Spawn();
            }
        }
        if (Input.GetKeyDown(KeyCode.G)) {
            Instantiate(GrassPrefab, transform.position, Quaternion.identity);
        }
    }

//-----------------------------

    //LOAD PLAYER PREFS
    Vector3 LoadPrefsPosition() {
        float x = PlayerPrefs.GetFloat("x"),
              y = PlayerPrefs.GetFloat("y"),
              z = PlayerPrefs.GetFloat("z");
        return new Vector3(x, y, z);
    }

    //SAVE PLAYER PREFS
    bool SAVED_PREFS = false;
    void OnApplicationQuit() {
        if (!SAVED_PREFS) {
            SAVED_PREFS = true;
            print("SavedPlayerPrefs");
            PlayerPrefs.SetFloat("x", transform.position.x);
            PlayerPrefs.SetFloat("y", transform.position.y);
            PlayerPrefs.SetFloat("z", transform.position.z);
        }
    }

    void OnDestroy() {
        if (!SAVED_PREFS) {
            SAVED_PREFS = true;
            print("SavedPlayerPrefs");
            PlayerPrefs.SetFloat("x", transform.position.x);
            PlayerPrefs.SetFloat("y", transform.position.y);
            PlayerPrefs.SetFloat("z", transform.position.z);
        }
    }
}
