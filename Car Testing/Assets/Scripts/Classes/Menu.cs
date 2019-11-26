using UnityEngine;
using System.Collections;

[System.Serializable]
public class Menu{
	public bool active = false;
	public int count, width, height;
	public float x, y;
	public Rect rectangle;
	public MenuOption[] options;
	public int longest_label = 0;
	int text_size = 9;

	public Menu(MenuOption[] options){
		this.options = options;
		this.count = options.Length;
		this.height = 20 * this.count;
		this.x = Input.mousePosition.x;
		this.y = Screen.height - Input.mousePosition.y;


		foreach(MenuOption opt in options){
			if(opt.label.Length > this.longest_label){
				this.longest_label = opt.label.Length;
			}
		}

		this.width = this.longest_label * this.text_size;
		this.rectangle = new Rect (this.x, this.y, this.width, this.height);
		this.active = true;
	}

	public Menu(){
		this.active = false;
	}
}

[System.Serializable]
public class MenuOption{

	public string label;
	public int command;
	public Item item;

	public MenuOption(string label, int command){
		this.label = label;
		this.command = command;
	}

	public MenuOption(string label, int command, Item item){
		this.label = label;
		this.command = command;
		this.item = item;
	}

	public bool Click(){
		GameObject player = GameObject.FindGameObjectWithTag ("NPC");
		InventoryManager im = player.GetComponent<InventoryManager> ();

		if (command < 0) {
			switch (command) {
			case -2:
				
				return true;
			case -1:
				return this.item.Unequip ();
			}
		} else {

			return this.item.Equip (this.command);
		}
		return false;
	}
}