using UnityEngine;
using System.Collections;

public class InventoryManager : MonoBehaviour {
	//public int inventory_max_size = 5;

	public GameObject armature;

	public BodyPart[] equipable_body_part;

	public ItemArray inventory, equipment;

	//public ResourceManager RM;
	PlayerPreferenceManager PPM;
	WindowManager WM;


	void Start () {
		//RM = GetComponent<ResourceManager> ();
		PPM = new PlayerPreferenceManager (this);
		WM = new WindowManager (this);

		inventory = PPM.LoadItemArray("inventory");
		equipment = PPM.LoadItemArray("equipment");
	}

	void Update(){
		WM.Update ();
	}
		
	void OnGUI(){
		WM.ShowGUI ();
	}

	void OnApplicationQuit(){
		PPM.SaveItemArray(inventory, "inventory");
		PPM.SaveItemArray(equipment, "equipment");
	}
}