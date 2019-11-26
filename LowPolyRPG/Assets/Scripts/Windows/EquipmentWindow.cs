using UnityEngine;
using System.Collections;

public class EquipmentWindow : MonoBehaviour {
	Mouse mouse;
	Player player;
	GUISkin skin;

	public Rect rect = new Rect(245, 20, 125, 185);
	Rect drag_zone;

	int width = 3, height = 4;
	public int button_size = 35, button_margin = 5;

	Rect[] slots = new Rect[7];

	//Vector2 slot_size = new Vector2 (button_size, button_size);

	void Start () {
		mouse = GetComponent<Mouse> ();
		player = GetComponent<Player> ();
		this.skin = Resources.Load ("GUI Skins/Normal") as GUISkin;

		if (PlayerPrefs.GetInt ("Equipment set") == 1) {
			Vector2 pos = new Vector2 (PlayerPrefs.GetInt ("Equipment x"), PlayerPrefs.GetInt ("Equipment y"));
			rect.position = pos;
		}

		CalculateRects ();
	}

	void OnDestroy(){
		PlayerPrefs.SetInt ("Equipment set", 1);
		PlayerPrefs.SetInt ("Equipment x", (int)rect.position.x);
		PlayerPrefs.SetInt ("Equipment y", (int)rect.position.y);
	}

	bool dragging_window = false;
	void Update(){
		CalculateRects ();

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

	void OnGUI(){
		
		GUI.skin = skin;
		GUI.Window (1, this.rect, WindowFunction, "Equipment");
		GUI.Box (drag_zone, "drag zone");
	}

	Rect head_rect, body_rect;

	void WindowFunction(int windowID){
		int index = 0;

		foreach(Rect r in slots){
			GUI.Box (r, "");

			if (player.equipment_items [index] != null) {
				if (player.equipment_items [index].type != ItemType.Null) {  

					if (player.equipment_items [index] != null) {
						Item item = player.equipment_items [index];
						Vector2 position;
						Rect button;

						button = slots [index];

						if (GUI.Button (button, item.texture)) {
							if (Input.GetMouseButtonUp (0)) {
								player.Unequip (item);
							} else if (Input.GetMouseButtonUp (1)) {
								RightClickMenu rcm = transform.gameObject.AddComponent<RightClickMenu> ();
								rcm.Init (item, WindowType.Equipment);
							}
						}
					}
				}
			}
			index++;
		}
	}

	void CalculateRects(){
		int index = 0,
		slot1 = button_margin,
		slot2 = button_margin + (button_margin + button_size),
		slot3 = button_margin + (button_margin + button_size) * 2;

		Vector2 size = new Vector2 (button_size, button_size),
		position = new Vector2 (slot2, skin.window.border.top + ((button_size + button_margin) * index++) + button_margin);
		slots[0] = new Rect (position, size);

		position = new Vector2 (slot2, skin.window.border.top + ((button_size + button_margin) * index++) + button_margin);
		slots[1] = new Rect (position, size);

		position = new Vector2 (slot2, skin.window.border.top + ((button_size + button_margin) * index++) + button_margin);
		slots[2] = new Rect (position, size);

		position = new Vector2 (slot2, skin.window.border.top + ((button_size + button_margin) * index++) + button_margin);
		slots[3] = new Rect (position, size);

		position = new Vector2 (slot1, slots[0].y);
		slots[4] = new Rect (position, size);

		position = new Vector2 (slot1, slots[1].y);
		slots[5] = new Rect (position, size);

		position = new Vector2 (slot3, slots[1].y);
		slots[6] = new Rect (position, size);

		rect.size = new Vector2((button_size + button_margin) * width + button_margin, (button_size + button_margin) * height + button_margin + skin.window.border.top);
		drag_zone = new Rect(0, 0, rect.width, skin.window.border.top);
	}
}
