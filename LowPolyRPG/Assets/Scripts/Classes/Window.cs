using UnityEngine;
using System.Collections;

[System.Serializable]
public class Window{
	public GUISkin skin;

	public int windowID;
	public bool active;

	public Rect window_rect = new Rect(0, 0, 300, 300);
	public Rect content_rect;

	public event Function function;
	WindowComponent component;

	public Window(int windowID, Function function, Transform transform, Rect window_rect){
		this.windowID = windowID;
		this.skin = Resources.Load("GUI Skins/Normal") as GUISkin;
		this.function = function;
		this.window_rect = window_rect;

		content_rect = new Rect (window_rect.position + new Vector2(0, skin.window.border.top), window_rect.size + new Vector2(0, -skin.window.border.top));
		component = transform.gameObject.AddComponent<WindowComponent> ();
		component.window = this;
	}

	public void GUIFunction(int windowID){
		//GUI.skin = skin;

		function (content_rect);
		DragWindow ();
	}

	//dragwindow()
	bool dragging = false;//are you holding mouse0 in the drag area (top border)
	Vector2 start_position;//where you started dragging

	public void DragWindow(){
		Vector2 mouse_position = new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y);
		Rect drag_box = new Rect (window_rect.position, new Vector2(window_rect.width, skin.window.border.top));

		if(drag_box.Contains(mouse_position) && Input.GetMouseButtonDown(0)){
			start_position = mouse_position;
			dragging = true;
		}else if(Input.GetMouseButtonUp(0)){
			dragging = false;
		}

		if(dragging){
			window_rect.position += mouse_position - start_position;
			start_position = mouse_position;
		}
	}
}