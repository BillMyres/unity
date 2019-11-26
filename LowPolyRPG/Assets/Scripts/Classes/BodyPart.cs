using UnityEngine;
using System.Collections;

[System.Serializable]
public class BodyPart {
	public string name;
	public Transform transform;
	public Mesh default_mesh;
	public Material default_material;

	public BodyPart(string name, Transform transform, Mesh default_mesh, Material default_material){
		this.name = name;
		this.transform = transform;
		this.default_mesh = default_mesh;
		this.default_material = default_material;
	}
}