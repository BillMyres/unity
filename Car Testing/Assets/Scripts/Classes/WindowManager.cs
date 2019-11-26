using UnityEngine;
using System.Collections;

[System.Serializable]
public class WindowManager {

	InventoryManager im;

	WindowArray windows = new WindowArray();

	int id = 0;
	public WindowManager(InventoryManager im){
		this.im = im;

		windows.Add (new Window("Inventory", id++, new Rect(20, 20, 120, 50), Inventory));
		windows.Add (new Window("Equipment", id++, new Rect(160, 20, 120, 50), Equipment));
		windows.Add (new Window("Console", id++, new Rect(20, Screen.height - 170, 280, 150), Console));

		windows.Add (new Window("RightClick", id++, new Rect(0, 0, 0, 0), RightClick));
	}

	public void Update (){
		ConsoleUpdate ();
		RightClickUpdate ();
	}

	public void ShowGUI(){
		Show ("Inventory");
		Show ("Equipment");
		Show ("Console");

		if(right_click_menu.active){
			Show ("RightClick");
		}
	}

	Rect Show(string name){
		return windows.GetWindow (name).ShowWindow();
	}

	// INVENTORY //-------------------------------------------------------------------------------------------------------
	void Inventory(int windowID){
		if(im.inventory.count <= 0) return;

		Window inventory_window = windows.GetWindow("Inventory");

		Event e = Event.current;
		Rect rect = inventory_window.rect;
		//rect.height = 50 + 20 * (im.inventory_max_size - 1);
		rect.height = 50 + 20 * (im.inventory.count - 1);
		inventory_window.SetRect (rect);

		int i = 0;
		foreach(Item item in im.inventory.items){
			Rect button = new Rect (10, 20 + 20 * i, 100, 20);
			if(GUI.Button(button, item.name)){
				MenuOption[] options;
				if(item.equipable){
					options = new MenuOption[item.equip_bones.Length];
					for(int j = 0; j < item.equip_bones.Length; j++){
						options[j] = new MenuOption("Equip : " + im.equipable_body_part[item.equip_bones[j]].name, item.equip_bones[j], item);
					}
				}else{
					options = new MenuOption[1];
					options [0] = new MenuOption ("Drop", -2, item);
				}

				if(e.button == 1){
					right_click_menu = new Menu (options);

				}else if(e.button == 0){
					options [0].Click ();
				}
			}
			i++;
		}
	}
	// INVENTORY //-------------------------------------------------------------------------------------------------------

	// EQUIPMENT //-------------------------------------------------------------------------------------------------------
	void Equipment(int windowID){
		if(im.equipment.count <= 0) return; 

		Window equipment_window = windows.GetWindow ("Equipment");

		Event e = Event.current;
		Rect rect = equipment_window.rect;
		rect.height = 30 + 20 * im.equipment.count;
		equipment_window.SetRect (rect);

		int i = 0;
		foreach(Item item in im.equipment.items){
			Rect button = new Rect (10, 20 + 20 * i, 100, 20);
			if(GUI.Button(button, item.name)){
				MenuOption[] options = new MenuOption[2];
				options [0] = new MenuOption ("Unequip", -1, item);
				options [1] = new MenuOption ("Drop", -2, item);

				if(e.button == 1){
					right_click_menu = new Menu (options);

				}else if(e.button == 0){
					options[0].Click();
				}
			}
			i++;
		}
	}
	// EQUIPMENT //-------------------------------------------------------------------------------------------------------

	// CONSOLE //---------------------------------------------------------------------------------------------------------
	string console_input = "", console_output = "", last_command = "";

	void Console(int windowID){
		Event e = Event.current;

		this.console_input = GUI.TextField (new Rect(0, 130, 280, 20), this.console_input);

		if(e.keyCode == KeyCode.Return){
			this.console_output = this.console_input;
			Debug.Log ("return");
		}else if(e.keyCode == KeyCode.UpArrow){
			this.console_input = this.last_command;
		}
	}



	void ConsoleUpdate(){
		if(console_output != ""){
			if(console_output.Substring(0, 1) == "/"){
				string[] words = console_output.Substring (1).Split (" " [0]);

				if(words.Length == 2){

					switch(words[0]){
					case "give":
						im.inventory.Add (new Item (words [1], im));
						last_command = console_output;
						break;
					default:
						Debug.Log ("Invalid command.");
						break;
					}

				}else if(words.Length == 1){

					switch(words[0]){
					case "clear":
						im.inventory = new ItemArray ();
						im.equipment = new ItemArray ();
						break;
					default:
						Debug.Log ("Invalid command.");
						break;
					}

				}
			}
			console_input = "";
			console_output = "";
		}
	}
	// CONSOLE //---------------------------------------------------------------------------------------------------------

	// RIGHTCLICK //------------------------------------------------------------------------------------------------------
	Menu right_click_menu = new Menu();
	Vector2 mouse_position;

	void RightClick(int windowID){
		Window right_click_window = windows.GetWindow ("RightClick");
		right_click_window.rect = right_click_menu.rectangle;

		Rect option_rect = new Rect (0, 0, right_click_menu.width, 20);

		Event e = Event.current;
		foreach(MenuOption option in right_click_menu.options){

			if (GUI.Button (option_rect, option.label)) {

				if (option.Click ()) {
					right_click_menu.active = false;
				}
			}

			option_rect.y += option_rect.height;
		}
	}

	void RightClickUpdate(){
		mouse_position = new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y);

		if(!right_click_menu.rectangle.Contains(mouse_position)){
			if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)){
				right_click_menu.active = false;
			}
		}
	}
	// RIGHTCLICK //------------------------------------------------------------------------------------------------------
}
