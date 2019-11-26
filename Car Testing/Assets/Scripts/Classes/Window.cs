using UnityEngine;
using System.Collections;

[System.Serializable]
public class Window {

	public string name;
	public Rect rect;
	public int window_id;
	public GUI.WindowFunction function;

	public Window(string name, int window_id, Rect rect, GUI.WindowFunction function){
		this.name = name;
		this.rect = rect;
		this.window_id = window_id;
		this.function = function;
	}

	public Rect ShowWindow(){
		return GUI.Window (this.window_id, this.rect, this.function, this.name);
	}

	public void SetRect(Rect rect){
		this.rect = rect;
	}
}
