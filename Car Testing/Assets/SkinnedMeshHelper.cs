using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkinnedMeshHelper : MonoBehaviour {

	public GameObject body, player;
	SkinnedMeshRenderer smr, body_smr;
	public Transform[] bones;
	//Animator anim;
	//List<GameObject> result;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("NPC");
		smr = GetComponent<SkinnedMeshRenderer> ();
		body_smr = body.GetComponent<SkinnedMeshRenderer> ();
		//anim.wei
		//player.GetComponent<Animator>().

		//result = SkinnedMeshTools.AddSkinnedMeshTo (Skin, Skin.transform);
		bones = body.GetComponent<SkinnedMeshRenderer>().bones;
		smr.bones = bones;

		smr.rootBone = bones [0];

		smr.sharedMesh.boneWeights = body_smr.sharedMesh.boneWeights;
		print ("MEsh: " + smr.sharedMesh.blendShapeCount);
		int i = 0;
//		foreach(Transform bone in bones){
//			i++;
//		}
	}


	void Update () {
		



	}
}
