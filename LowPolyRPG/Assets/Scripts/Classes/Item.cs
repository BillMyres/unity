using UnityEngine;
using System.Collections;

#region Tier 1
[System.Serializable]
public class Item {//mesh type, add item type
	
	public string name;
	public string description;
	public bool stackable;
	public int count;
	public Tier tier;
	public ItemType type;
	public EquipLocation[] equip_location;
	public Player player;

	public bool nullified;//true if item is null
	public Texture texture;//image of the item only if available in the Resources folder
	public Object resource;//null unless the item has a model in the Resources folder

	public string id;//unique identifier for each item

	public GameObject world_object;//null unless it exists in the world
	public EquipLocation attached_to;//null unless item is equipped

	public Item(){
		this.nullified = true;
	}

	public Item(string name, string description, bool stackable, int count, Tier tier, ItemType type, EquipLocation[] equip_location, Player player){
		this.name = name;
		this.description = description;
		this.stackable = stackable;
		this.count = count;
		this.tier = tier;
		this.type = type;
		this.equip_location = equip_location;
		this.player = player;

		this.nullified = false;
		this.texture = Resources.Load ("Textures/" + name) as Texture;
		this.resource = Resources.Load ("Item Prefabs/" + name);

		this.id = System.Guid.NewGuid ().ToString ();
	}

	public Item(string name, string description, int count, bool stackable, Player player){
		this.nullified = false;
		this.name = name;
		this.description = description;
		this.texture = Resources.Load ("Textures/" + name) as Texture;
		this.count = count;
		this.stackable = stackable;
		this.player = player;
		//this.type = ItemType.Consumable; maybe set item type here
		id = System.Guid.NewGuid().ToString();
	}

	public Item(string name, string description, int count, bool stackable, ItemType item_type, EquipLocation[] equip_location, Player player){
		this.nullified = false;
		this.name = name;
		this.description = description;
		this.texture = Resources.Load ("Textures/" + name) as Texture;
		this.count = count;
		this.stackable = stackable;
		this.type = item_type;
		this.resource = Resources.Load ("Item Prefabs/" + name);
		this.equip_location = equip_location;
		this.player = player;
		id = System.Guid.NewGuid().ToString();
	}

	public Item(string name, string description, Tier tier, int count, bool stackable, ItemType item_type, EquipLocation[] equip_location, Player player){
		this.nullified = false;
		this.name = name;
		this.description = description;
		this.tier = tier;
		this.texture = Resources.Load ("Textures/" + name) as Texture;
		this.count = count;
		this.stackable = stackable;
		this.type = item_type;
		this.resource = Resources.Load ("Item Prefabs/" + name);
		this.equip_location = equip_location;
		this.player = player;
		id = System.Guid.NewGuid().ToString ();
	}

	//create object from Resources folder
	//set position and material of the obejct
	//set world object to the world GameObject
	public void CreateWorldObject(Vector3 position){
		attached_to = EquipLocation.Null;
		if(world_object && type != ItemType.Probe){
			world_object.transform.parent = null;
			world_object.transform.position = position;//need to add position offset
			return;
		}
		GameObject obj = (GameObject) GameObject.Instantiate (resource);
		obj.transform.position = obj.transform.position + position;
//		if(mesh_type == MeshType.Solid){
//			obj.GetComponent<MeshRenderer> ().material = material;
//		}
		obj.AddComponent<ObjectInformation> ().item = this;

		world_object = obj;
	}

	//if worldobject doesn't exist, create one
	//if item is not skinned mesh, set item to the equip location
	//if item is skinned mesh, replace the default mesh and material with the items
	//set attached-to the equip location
	public void AttachTo(EquipLocation e){
		if (!world_object) { CreateWorldObject (Vector3.zero); }

		BodyPart bone = player.bones [e.GetHashCode()];

		if (type != ItemType.SkinnedArmour) {
			world_object.transform.parent = bone.transform;
			world_object.transform.localPosition = Vector3.zero;
			world_object.transform.localRotation = Quaternion.Euler (0, 0, 0);
		} else {
			SkinnedMeshRenderer renderer = bone.transform.GetComponent<SkinnedMeshRenderer> ();
			renderer.sharedMesh = world_object.GetComponent<MeshFilter> ().mesh;
			renderer.material = world_object.GetComponent<MeshRenderer> ().material;
			GameObject.Destroy (world_object);
		}

		attached_to = e;
	}

	//delete world object, 
	//if its a skinned mesh return mesh and material to defaults
	//set attached-to location to null
	public void DestroyWorldObject(){
		GameObject.Destroy (world_object);
		world_object = null;
		if(type == ItemType.SkinnedArmour && attached_to != EquipLocation.Null){
			BodyPart bone = player.bones[attached_to.GetHashCode()];
			SkinnedMeshRenderer renderer = bone.transform.GetComponent<SkinnedMeshRenderer> ();

			if(renderer != null){
				renderer.sharedMesh = bone.default_mesh;
				renderer.material = bone.default_material;
			}
		}
		attached_to = EquipLocation.Null;
	}
}
#endregion

#region Tier 2
[System.Serializable]
public class Weapon : Item{
	public Weapon (string name, string description, Player player) : base( name, description, 1, false, ItemType.Weapon, new EquipLocation[] { EquipLocation.Hand_R, EquipLocation.Hand_L }, player ){}
}
[System.Serializable]
public class Shield : Item{
	public Shield (string name, string description, Player player) : base( name, description, 1, false, ItemType.Armour, new EquipLocation[] { EquipLocation.Hand_L, EquipLocation.Hand_R }, player ){}
}
[System.Serializable]
public class Body : Item{
	public Body (string name, string description, Player player) : base( name, description, 1, false, ItemType.SkinnedArmour, new EquipLocation[] { EquipLocation.Body }, player ){}
}
[System.Serializable]
public class Legs : Item{
	public Legs (string name, string description, Player player) : base( name, description, 1, false, ItemType.SkinnedArmour, new EquipLocation[] { EquipLocation.Legs }, player ){}
}
[System.Serializable]
public class Cape : Item{
	public Cape (string name, string description, Player player) : base( name, description, 1, false, ItemType.SkinnedArmour, new EquipLocation[] { EquipLocation.Cape }, player ){}
}
[System.Serializable]
public class Shoe : Item{
	public Shoe (string name, string description, Player player) : base( name, description, 1, false, ItemType.SkinnedArmour, new EquipLocation[] { EquipLocation.Feet }, player ){}
}
[System.Serializable]
public class Hat : Item{
	public Hat (string name, string description, Player player) : base( name, description, 1, false, ItemType.Armour, new EquipLocation[] { EquipLocation.Head }, player ){}
}

[System.Serializable]
public class MiningTabletClass : Item{
	public MiningTabletClass(string name, string description, Tier tier, Player player) : base( name, description, tier, 1, false, ItemType.Tablet, new EquipLocation[] { EquipLocation.Hand_L, EquipLocation.Hand_R }, player){}
}
[System.Serializable]
public class MiningProbeClass : Item{
	public MiningProbeClass(string name, string description, Tier tier, Player player) : base( name, description, tier, 1, true, ItemType.Probe, null, player ){}
}


[System.Serializable]
public class Ore : Item{
	public Ore(string name, string description, Player player) : base( name, description, Tier.Null, 1, true, ItemType.Ore, null, player ){}
}
#endregion

//Items------------------------------------------------------------------------------------------------

#region Hats
[System.Serializable]
public class PointedHat : Hat{
	public PointedHat (Player player) : base( "Pointed Hat", "A magical hat.", player ){}
}
#endregion

#region Capes
[System.Serializable]
public class Cloak : Cape{
	public Cloak (Player player) : base( "Cloak", "A magical cloak.", player ){}
}
#endregion

#region Bodys
[System.Serializable]
public class Shirt : Body{
	public Shirt (Player player) : base( "Shirt", "A magical shirt.", player ){}
}
#endregion

#region Legs
[System.Serializable]
public class Pants : Legs{
	public Pants (Player player) : base( "Pants", "A magical pair of pants.", player ){}
}
#endregion

#region Shoes
[System.Serializable]
public class Shoes : Shoe{
	public Shoes (Player player) : base( "Shoes", "A beautiful pair of shoes.", player ){}
}
#endregion

#region Shields
[System.Serializable]
public class HeaterShield : Shield{
	public HeaterShield (Player player) : base( "Heater Shield", "A powerful shield.", player ){}
}
#endregion

#region Swords
//SWORDS
[System.Serializable]
public class NormanSword : Weapon{
	public NormanSword (Player player) : base( "Norman Sword", "A powerful sword.", player ){}
}
#endregion

#region Staves
//STAVES
[System.Serializable]
public class Staff : Weapon{
	public Staff (Player player) : base( "Staff", "A magical staff.", player ){}
}
#endregion

#region Mining
[System.Serializable]
public class MiningTablet : MiningTabletClass{
	public MiningTablet (Tier tier, Player player) : base( "Mining Tablet", "A tablet used to control and locate mining probes.", tier, player ){}
}
[System.Serializable]
public class MiningProbe : MiningProbeClass{
	public MiningProbe (Tier tier, Player player) : base( "Mining Probe", "A tablet used to control and locate mining probes.", tier, player ){}
}

//ORE
[System.Serializable]
public class Bauxite : Ore{
	public Bauxite (Player player) : base( "Bauxite", "An abundant ore, usually refined into aluminum.", player ){}
}
#endregion