using UnityEngine;
using System.Collections;

public class PlayerPreferenceManager {

	InventoryManager im;

	public PlayerPreferenceManager(InventoryManager im){
		this.im = im;
	}

	public bool SaveItemArray(ItemArray array, string array_name){

		PlayerPrefs.SetInt (array_name + "_COUNT", array.count);
		PlayerPrefs.SetInt (array_name + "_MAX_SIZE", array.max_size);

		for(int i = 0; i < array.count; i++){
			PlayerPrefs.SetString (array_name + ":" + i, array.items[i].name);
		}

		return true;
	}

	public ItemArray LoadItemArray(string array_name){
		
		ItemArray array = new ItemArray (PlayerPrefs.GetInt(array_name + "_MAX_SIZE"));

		for(int i = 0; i < PlayerPrefs.GetInt(array_name + "_COUNT"); i++){
			Item item = new Item (PlayerPrefs.GetString(array_name + ":" + i), im);

			array.Add (item);
		}

		return array;
	}
}
