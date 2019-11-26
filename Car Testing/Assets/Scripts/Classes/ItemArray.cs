using UnityEngine;
using System.Collections;

[System.Serializable]
public class ItemArray{

	public Item[] items;
	public int count = 0, max_size = 0;

	public ItemArray(){
	}
	public ItemArray(int max_size){
		this.max_size = max_size;
	}

	public bool Add(Item item){
		if(this.count == max_size && max_size > 0){ return false; }//if array is already full, and has a limit : ( <0 = no limit )
		Item[] new_items = new Item[this.count + 1];
		if (this.count > 0) {

			for (int i = 0; i < count; i++) {
				new_items [i] = this.items [i];
			}
		}
		new_items [this.count] = item;
		this.items = new_items;
		this.count++;
		return true;
	}

	public bool Remove(Item item){
		//if(this.count <= 0){ return false; }
		Item[] new_items = new Item[this.count - 1];
		int cc = 0;
		for(int i = 0; i < this.count; i++){
			if(this.items[i] != item){
				new_items [cc] = this.items [i];
				cc++;
			}
		}

		this.items = new_items;
		this.count--;
		return true;
	}
}
