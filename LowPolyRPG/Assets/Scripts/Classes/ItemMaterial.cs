using UnityEngine;
using System.Collections;

public class ItemMaterial {

	public Material Wood, Iron, Steel, Blue;
	public Material TabletBasic, ProbeBasic;

	public ItemMaterial(){
		this.Wood = Resources.Load ("Materials/Wood") as Material;
		this.Iron = Resources.Load ("Materials/Iron") as Material;
		this.Steel = Resources.Load ("Materials/Steel") as Material;
		this.Blue = Resources.Load ("Materials/Blue") as Material;

		this.TabletBasic = Resources.Load ("Materials/TabletBasic") as Material;
		this.ProbeBasic = Resources.Load ("Materials/ProbeBasic") as Material;
	}
}
