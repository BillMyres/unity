using UnityEngine;
using System.Collections;

[System.Serializable]
public class EquipmentSlot {

	public string name;
	public Rect rect;
	public Item item;

	public EquipmentSlot(string name, Rect rect){
		this.name = name;
		this.rect = rect;
	}

	public bool Occupied(){
		return true;
	}
}

[System.Serializable]
public class EquipmentSlotList {

	public EquipmentSlot[] slots;

	public EquipmentSlotList(int number){
		slots = new EquipmentSlot[number];
	}

	public EquipmentSlot GetEquipmentSlot(EquipLocation eq){
		return GetEquipmentSlot (eq.ToString());
	}

	public EquipmentSlot GetEquipmentSlot(string name){
		
		foreach(EquipmentSlot eq in slots){
			if(eq.name == name){ 
				return eq; 
				Debug.Log ("name == name");
			}

		}

		return null;
	}
}