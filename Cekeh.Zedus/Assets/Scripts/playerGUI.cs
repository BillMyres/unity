using UnityEngine;
using System.Collections;

public class playerGUI : MonoBehaviour {

    public GUISkin skin;

    Rect vechicles;
    bool VMOpen = false;
    int VMSpeed = 15;
    public Vector2 VMSize;//vechicleMenuSize
    Vector2 VMPosition;//vechicleMenuPosition

    void Start () {
        Init();
        menuStart();

    }

    void Init() {
        VMPosition = new Vector2(Screen.width - VMSize.x, 0);
        vechicles = new Rect(VMPosition, VMSize);
    }

    void Update () {
        Init();

        if (VMOpen && VMSize.x < 180) {
            VMSize.x += 1 * VMSpeed;
        } else if (!VMOpen && VMSize.x > 30) {
            VMSize.x -= 1 * VMSpeed;
        }

        if (VMOpen && VMSize.y < 250) {
            VMSize.y += 1 * VMSpeed;
        } else if (!VMOpen && VMSize.y > 30) {
            VMSize.y -= 1 * VMSpeed;
        }
    }

    void OnGUI() {
        GUI.skin = skin;
        if (GUI.Button(vechicles, "<<")) {
            if (!VMOpen) {
                VMOpen = true;
            } else {
                VMOpen = false;
            }
        }
        MoreGUI();
    }
    // ///////////////////////////////////////////////////////////////
    void MoreGUI() {
        
    }
    void menuStart() {
        
    }
}
