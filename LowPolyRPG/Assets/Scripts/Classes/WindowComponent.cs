using UnityEngine;
using System.Collections;

public class WindowComponent : MonoBehaviour {

	public Window window;

	void OnGUI () {
		if(window == null){ return; }
		GUI.skin = window.skin;
		GUI.Window (window.windowID, window.window_rect, window.GUIFunction, "test");
	}
}
