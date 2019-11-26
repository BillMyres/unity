using UnityEngine;
using System.Collections;

public delegate void Function(Rect rect);

[System.Serializable]
public class Enums {}

[System.Serializable]
public enum EquipLocation{
	Head,
	Body,
	Legs,
	Feet,
	Cape,
	Hand_L,
	Hand_R,
	Null
}

[System.Serializable]
public enum ItemType {
	Null,
	Armour,
	SkinnedArmour,
	Weapon,
	Tablet,
	Probe,
	Consumable,
	Ore,
	Deployer
}

[System.Serializable]
public enum WindowType{
	Inventory,
	Equipment,
	WorldObject,
	Mining
}

[System.Serializable]
public enum Tier {
	Null,
	One,
	Two,
	Three,
	Four,
	Five,
	Six,
	Seven,
	Eight,
	Nine,
	Ten
}

