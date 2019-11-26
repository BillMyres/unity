using UnityEngine;
using System.Collections;

public class InventoryWindow : MonoBehaviour {
	Mouse mouse;
	Player player;
	GUISkin skin;

	int width = 5, height = 5;

	public Rect rect = new Rect(20, 20, 205, 225);
	Rect drag_zone = new Rect (0,0,0,0);

	public int button_size = 35;
	public int button_margin = 5;

	#region Unity Callbacks
	public void Start(){
		mouse = GetComponent<Mouse> ();
		player = GetComponent<Player> ();
		this.skin = Resources.Load ("GUI Skins/Normal") as GUISkin;

		if(PlayerPrefs.GetInt ("Inventory set") == 1){
			Vector2 pos = new Vector2 (PlayerPrefs.GetInt("Inventory x"), PlayerPrefs.GetInt("Inventory y"));
			rect.position = pos;
		}
	}

	void OnDestroy(){
		PlayerPrefs.SetInt ("Inventory set", 1);
		PlayerPrefs.SetInt ("Inventory x", (int)rect.position.x);
		PlayerPrefs.SetInt ("Inventory y", (int)rect.position.y);
	}

	bool dragging_window = false;
	public void Update(){
		drag_zone.size = new Vector2(rect.width, skin.window.border.top);
		drag_zone.position = rect.position;

		if(drag_zone.Contains(mouse.position) && Input.GetMouseButtonDown(0)){
			dragging_window = true;
		}else if(Input.GetMouseButtonUp(0)){
			dragging_window = false;
		}

		if(dragging_window && mouse.dragging){
			rect.position += mouse.drag_offset;
			mouse.ResetDragOffset ();
		}
	}

	public void OnGUI(){
		GUI.skin = skin;

		rect.size = new Vector2 (width * (button_size + button_margin) + button_margin, height * (button_size + button_margin) + skin.window.border.top + button_margin);

		GUI.Window (0, this.rect, WindowFunction, "Inventory");
	}
	#endregion

	#region Window Function (GUI)
	void WindowFunction(int windowID){
		int x = 0, y = 0;

		//for each item in inventory
		foreach(Item item in player.inventory_items){
			

			//index position of the item : add to y
			if (x >= width) {
				x -= width;
				y++;
			}
			if (!item.nullified) { 
				if (item.type != ItemType.Null) {  
					//default button : used with all items : add window top border
					Vector2 position = new Vector2 ((button_size + button_margin) * x + button_margin, (button_size + button_margin) * y + skin.window.border.top + button_margin),
					size = new Vector2 (button_size, button_size);
					Rect button = new Rect (position, size);

					//item button
					if (GUI.Button (button, item.texture)) {

						//Left clicked the item
						if (Event.current.button == 0 && item.equip_location != null) {//left clicked

							//equip to the first equip_location

							player.Equip (item, item.equip_location [0]);
							return;

							//Right clicked the item
						} else if (Event.current.button == 1) {

							//add right click menu to the player object as a script
							RightClickMenu rcm = transform.gameObject.AddComponent<RightClickMenu> ();
							rcm.Init (item, WindowType.Inventory);
						}
					}
					if(item.stackable){
						GUI.Label (button, item.count.ToString());
					}
				}
			}
			x++;
		}
	}
	#endregion
}
