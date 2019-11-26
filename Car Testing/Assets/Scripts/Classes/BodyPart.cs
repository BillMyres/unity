using UnityEngine;
using System.Collections;

[System.Serializable]
public class BodyPart{
	public string name;
	public int id;
	public GameObject bone;
	//public Item item;

	public BodyPart(string name, GameObject bone, int id){
		this.name = name;
		this.bone = bone;

		this.id = id;
	}
}