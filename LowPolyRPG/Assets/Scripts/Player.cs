using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	//GUI
	GUISkin skin;
	GUIManager gui;

	//ITEMS
	Vector2 inventory_position, equipment_position;
	public Item[] inventory_items = new Item[28],
		   		  equipment_items = new Item[7];

	//PLAYER TRANSFORMS
	public BodyPart[] bones;
	ItemMaterial material;

	//MINING
	public float[] mining_hits = new float[10];
	public float total_hit = 0, last_hit = 0, highest_hit = 0, all_time_average;
	public int drops = 0;



	public WindowContent content;
	public Window test, green;

	#region Unity Callbacks
	void Start () {
		gui = new GUIManager (this);
		skin = Resources.Load ("GUI Skins/Normal") as GUISkin;
		material = new ItemMaterial();
		AddToInventory (new Staff(this), 1);
		AddToInventory (new HeaterShield(this), 1);
		AddToInventory (new Shirt(this), 1);
		AddToInventory (new Cloak(this), 1);
		AddToInventory (new Shoes(this), 1);
		AddToInventory (new NormanSword(this), 1);
		AddToInventory (new PointedHat (this), 1);
		AddToInventory (new Pants(this), 1);

		AddToInventory (new MiningProbe (Tier.One, this), 1000);
		AddToInventory (new MiningTablet (Tier.One, this), 1);

		test = new Window (43, content.Mining, transform, new Rect(0, 0, 300, 300));
		//green = new Window (44, content.Green, transform, new Rect(300, 300, 300, 300));
	}

	void Update(){
		//content.hits = mining_hits;
		//content.average = all_time_average;
		all_time_average = total_hit / drops;
		if(Input.GetKeyUp(KeyCode.I)){
			gui.Toggle<InventoryWindow> ();
		}else if(Input.GetKeyUp(KeyCode.E)){
			gui.Toggle<EquipmentWindow> ();
		}
	}

	void OnGUI(){
		gui.Show ();

		GUI.color = Color.white;
		//GUI.Window (43, test.window_rect, test.GUIFunction, "test");
		//test.Show ();
	}
	#endregion

	#region Inventory and Equipment - Interactions
	//Equip an item to EquipLocation
	public void Equip(Item item, EquipLocation e){

		RemoveFromInventory (item, item.count);
		Item e_item = equipment_items [e.GetHashCode ()];

		if (!e_item.nullified) {
			if(e_item.type != ItemType.Null){
				Unequip(e_item);
			}
		}

		equipment_items [e.GetHashCode ()] = item;

		item.AttachTo (e);

	}

	//Unequip an item
	public void Unequip(Item item){
		RemoveFromEquipment(item);

		item.DestroyWorldObject ();

		AddToInventory(item, 1);
	}
	#endregion

	#region Inventory and Equipment - Add and Remove
	public bool AddToInventory(Item item, int count){

		int index = CheckInventoryFor (item);
		if(item.stackable && index != -1){
			inventory_items[index].count += count;
			return true;
		}

//		if(item.stackable){
//			for(int i = 0; i < inventory_items.Length; i++){
//				if(inventory_items[i] == item){
//					inventory_items [i].count += count;
//					return true;
//				}
//			}
//		}

		//for items not stackable, find nearest inventory spot
		for(int i = 0; i < inventory_items.Length; i++){
			if(inventory_items[i] == null || inventory_items[i].nullified){ 
				//item.count = count;
				inventory_items [i] = item;
				item.count = count;
				//item.inventory_index = i;
				return true;
			}
			if(inventory_items[i].type == ItemType.Null || inventory_items[i].nullified){
				//item.count = count;
				inventory_items [i] = item;
				item.count = count;
				//item.inventory_index = i;
				return true;
			}
		}
		return false;
	}

	int CheckInventoryFor(Item item){
		for(int i = 0; i < inventory_items.Length; i++){
			Item inventory_item = inventory_items [i];
			if (inventory_item != null) {
				if (inventory_items [i].name == item.name) {
					return i;
				}
			}
		}
		return -1;
	}

	public bool RemoveFromInventory(Item item, int count){
		for(int i = 0; i < inventory_items.Length; i++){
			if(inventory_items[i] == item){
				if (!item.stackable) {
					inventory_items [i] = new Item();
				} else {
					inventory_items [i].count -= count;
					if(item.count <= 0){
						inventory_items [i] = new Item();
					}
				}
				return true;
			}
		}
		return false;
	}

	public void AddToEquipment(Item item){
		int index = item.attached_to.GetHashCode();
		equipment_items [index] = item;
	}

	public void RemoveFromEquipment(Item item){
		int index = item.attached_to.GetHashCode();
		equipment_items [index] = null;
	}
	#endregion

	public AudioClip explosion_01;
	public void Explosion(){
		
	}
}
