using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

    int itemSize = 52,
        height = 8,
        width = 4;

    public bool active = true;

    void Start() {

    }

    void Update() {

    }

    void OnGUI() {
        if (active) {
            for (int x = 0; x < 4; x++) {
                for (int y = 0; y < 8; y++) {
                    int tx = Screen.width - (10 + ((4 - x) * itemSize)),
                        ty = Screen.height - (10 + ((8 - y) * itemSize));

                    GUI.Box(new Rect(tx, ty, 50, 50), " #" + ((y * width) + x + 1));
                }
            }
        }
    }
}
