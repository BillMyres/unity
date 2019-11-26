using UnityEngine;
using System.Collections;

[System.Serializable]
public class GUIManager{
	GUISkin skin;
	Player player;

	public GUIManager(Player player){
		this.skin = Resources.Load ("GUI Skins/Normal") as GUISkin;
		this.player = player;
	}

	//Rect inventory_rect, equipment_rect;
	public void Show(){
		GUI.skin = skin;
		Rect inventory_toggle = new Rect (0, (Screen.height / 2) - 100, 20, 100),
		equipment_toggle = new Rect (0, (Screen.height / 2), 20, 100);

		InventoryWindow inventory_window = player.transform.GetComponent<InventoryWindow> ();
		EquipmentWindow equipment_window = player.transform.GetComponent<EquipmentWindow> ();

		if (inventory_window) { GUI.color = Color.green; } else { GUI.color = Color.red; }
		if(GUI.Button (inventory_toggle, "I")){
			Toggle<InventoryWindow> ();
		}
		if (equipment_window) { GUI.color = Color.green; } else { GUI.color = Color.red; }
		if(GUI.Button (equipment_toggle, "E")){
			Toggle<EquipmentWindow> ();
		}
	}

	public void Toggle<T>(){
		Component component = player.GetComponent (typeof(T));
		if (component) {
			GameObject.Destroy (component);
		} else {
			player.gameObject.AddComponent(typeof(T));
		}
	}
}
