using UnityEngine;
using System.Collections;

public class RightClickMenu : MonoBehaviour {
	Player player;

	GUISkin skin;
	public Item item;
	WindowType type;
	Rect rect;

	int button_height = 20,
		button_width  = 150;

	Vector2 position, size;
	Vector2 mouse_position;

	public void Start(){
		player = GetComponent<Player>();
		skin = Resources.Load ("GUI Skins/Normal") as GUISkin;

		//set the size of the rect : start with 0 height, will be determined later in WindowFunction
		size = new Vector2 (button_width, 0);

		//get the right_click_menu on the player
		RightClickMenu rcm = transform.GetComponent<RightClickMenu> ();
		if(rcm != this){//if a right_click_menu already exists, delete it
			Destroy (rcm);
		}

		//set the position of the menu at the cursor
		position = new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y);
	}

	//called after start in Inventory script
	public void Init(Item item, WindowType type){
		this.item = item;
		this.type = type;
	}

	public void Update(){
		if(item == null){ return; }//if there is no item, return
		rect = new Rect (position, size);//the size of the window

		//calculate mouse position, because Event.current.mousePosition was giving me trouble
		mouse_position = new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y);

		bool contained = rect.Contains (mouse_position);//rect contains mouse
		if(!contained && Input.GetMouseButtonDown(0) ||
		   !contained && Input.GetMouseButtonDown(1)){
			Destroy (this);//destroy if you click outside of the rectangle
		}
	}

	public void OnGUI(){
		if(item == null){ return; }
		GUI.skin = skin;

		GUI.Window (31, rect, WindowFunction, item.name);
	}

	private void WindowFunction(int windowID){
		int index = 0;//increment for each button
		Rect button = new Rect (new Vector2(0, index++ * button_height), new Vector2(button_width, button_height));

		switch(type){
		case WindowType.Inventory:
			index = InventoryMenu (index);
			break;
		case WindowType.Equipment:
			index = EquipmentMenu (index);
			break;
		case WindowType.WorldObject:
			index = WorldObjectMenu (index);
			break;
		}

		if(type != WindowType.WorldObject){
			button.position = new Vector2 (0, index++ * button_height);
			if (GUI.Button (button, "Drop")) {
				if(!player.RemoveFromInventory(item, item.count)){
					player.RemoveFromEquipment (item);
				}
				item.DestroyWorldObject ();
				item.CreateWorldObject (transform.position);
				Destroy (this);
			}
		}

		//change the size of the window
		size = new Vector2 (button_width, index++ * button_height);
	}

	#region Menus : ItemType
	int InventoryMenu(int index){
		Rect button = new Rect (0, index++ * button_height, button_width, button_height);

		GUI.Button (button, "Use");

		if(item.type == ItemType.Probe){
			button.position = new Vector2 (0, index++ * button_height);
			if(GUI.Button(button, "Deploy")){
				if (item.count == 1) {
					if (!player.RemoveFromInventory (item, 1)) {
						player.RemoveFromEquipment (item);
					}
				} else {
					//player.inventory_items [item.inventory_index].count--;
					player.RemoveFromInventory(item, 1);
				}
				item.DestroyWorldObject ();
				item.CreateWorldObject (transform.position);
				item.world_object.GetComponent<Animator> ().SetTrigger ("Deploy");
				Destroy (this);
			}
		}

		if(item.type == ItemType.Tablet && GetComponent<MiningInformation>() == null){
			button.position = new Vector2 (0, index++ * button_height);
			if (GUI.Button (button, "Open Interface")) {//if pressed
				if(!GetComponent<MiningInformation>()){
					transform.gameObject.AddComponent<MiningInformation> ();
				}
				Destroy (this);
			}
		}

		if(item.equip_location != null){
			foreach(EquipLocation eq in item.equip_location){
				//Equip buttons
				button.position = new Vector2 (0, index++ * button_height);
				if (GUI.Button (button, "Equip : " + eq.ToString ())) {//if pressed
					player.Equip(item, eq);
					Destroy (this);
				}
			}
		}

		return index;
	}

	int EquipmentMenu(int index){
		Rect button = new Rect (0, index++ * button_height, button_width, button_height);

		if(GUI.Button(button, "Unequip")){//if pressed
			player.Unequip(item);
			Destroy (this);
		}

		return index;
	}

	int WorldObjectMenu(int index){
		Rect button = new Rect (0, index++ * button_height, button_width, button_height);

		//deployer right-clicked
		if (item.type == ItemType.Deployer) {
			ProbeDeployer pd = item.world_object.GetComponent<ProbeDeployer> ();
			if(GUI.Button(button, "Use")){
				pd.use();
			}

			return index;
		} else {
			if(GUI.Button(button, "Pick-up")){//if pressed
				player.AddToInventory(item, item.count);
				item.DestroyWorldObject ();
				Destroy (this);
			}
		}

		return index;
	}
	#endregion

	#region Menu Options
	void Use(int index){
		Rect button = new Rect (0, index++ * button_height, button_width, button_height);
		GUI.Button (button, "Use");
	}
	#endregion
}
