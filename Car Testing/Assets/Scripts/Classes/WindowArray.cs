using UnityEngine;
using System.Collections;

[System.Serializable]
public class WindowArray {

	public Window[] windows;
	public int length;

	public WindowArray(){
		this.length = 0;
	}

	public void Add(Window window){
		Window[] new_windows = new Window[this.length + 1];

		if(this.length > 0){
			for(int i = 0; i < this.length; i++){
				new_windows [i] = this.windows [i];
			}
		}

		new_windows [this.length] = window;
		this.windows = new_windows;
		length++;
	}

	public void Remove(Window window){
		Window[] new_windows = new Window[this.length - 1];

		int j = 0;
		for(int i = 0; i < this.length; i++){
			if(this.windows[i] != window){
				new_windows [j] = this.windows [i];
				j++;
			}
		}

		this.length--;
		this.windows = new_windows;
	}

	public Window GetWindow(string name){
		foreach(Window window in windows){
			if(window.name == name){ return window; }
		}

		return null;
	}
}
