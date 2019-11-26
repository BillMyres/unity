using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item{

	public string name;
	public Object resource;
	public bool equipable;
	InventoryManager im;

	public int[] equip_bones;

	GameObject game_object;

	public Item(string name, InventoryManager im){
		this.name = name;

		this.resource = im.RM.GetResource (name);

		this.im = im;

		if (this.resource == null) {
			this.equipable = false;
		} else {
			equip_bones = im.RM.GetItem(name).equip_bones;
			this.equipable = true;
		}
	}

	BodyPart bp;
	Material old;

	public bool Equip(int body_part){
		if(!equipable){ return false; }

		//this.equipped = true;

		im.inventory.Remove (this);
		im.equipment.Add (this);
		GameObject obj;

		obj = (GameObject)GameObject.Instantiate (resource, im.equipable_body_part [body_part].bone.transform);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.Euler (0, 0, 0);
		obj.transform.localScale = Vector3.one;

		if (body_part == 3 || body_part == 4 || body_part == 5) {
			SkinnedMeshRenderer bone = im.equipable_body_part [body_part].bone.GetComponent<SkinnedMeshRenderer>();
			bone.sharedMesh = obj.GetComponent<MeshFilter> ().sharedMesh;
			old = bone.sharedMaterial;
			bp = im.equipable_body_part [body_part];
			bone.sharedMaterial = obj.GetComponent<MeshRenderer> ().sharedMaterial;
			obj.SetActive(false);

			foreach(Transform t in im.armature.GetComponentInChildren<Transform>()){
				t.transform.rotation = Quaternion.Euler (-90, 180, 0);
			}
		}

		game_object = obj;


		return true;
	}

	public bool Unequip(){
		//this.equipped = false;

		im.equipment.Remove (this);
		im.inventory.Add (this);

		if(bp != null){
			bp.bone.GetComponent<SkinnedMeshRenderer> ().sharedMaterial = old;
		}

		GameObject.Destroy (game_object);

		return true;
	}
}
