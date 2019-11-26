using UnityEngine;
using System.Collections;

public class ProbeScript : MonoBehaviour {

	//public 

	Animator animator;
	ObjectInformation info;
	Item item;
	Player player;
	AudioSource audio;
	public AudioClip explosion_01;

	void Start () {
		animator = GetComponent<Animator> ();
		info = GetComponent<ObjectInformation> ();
		item = info.item;
		player = item.player;
		audio = GetComponent<AudioSource> ();
	}


	bool exploded = false;
	void Update () {
		if (animator.GetCurrentAnimatorStateInfo (0).normalizedTime > 1 && !exploded) {
			//GameObject.Destroy (transform.gameObject);
			exploded = true;
			audio.PlayOneShot(explosion_01);
		}

		if(!audio.isPlaying && exploded){
			GameObject.Destroy (transform.gameObject);
		}
	}

	void OnDestroy(){
		//player.AddToInventory(new MiningProbe(Tier.One, , player));
		Drop(10);
		//player.Explosion ();
	}

	void Drop(int tier){
		float f = Random.Range (1, 10);
		if(f > 5 && tier > 1){ f = Random.Range (10, 20); }
		if(f > 15 && tier > 2){ f = Random.Range (20, 30); }
		if(f > 25 && tier > 3){ f = Random.Range (30, 40); }
		if(f > 35 && tier > 4){ f = Random.Range (40, 50); }
		if(f > 45 && tier > 5){ f = Random.Range (50, 60); }
		if(f > 55 && tier > 6){ f = Random.Range (60, 70); }
		if(f > 65 && tier > 7){ f = Random.Range (70, 80); }
		if(f > 75 && tier > 8){ f = Random.Range (80, 90); }
		if(f > 85 && tier > 9){ f = Random.Range (90, 100); }

		float keep = Random.Range (1, 10);
		//if(keep != 1){ print ("Nothing..."); return; }

		float[] temp = new float[10];

		for(int i = 0; i < 9; i++){
			if (player.mining_hits [i + 1] != null) {
				temp [i] = player.mining_hits [i + 1];
			}
		}

		temp [9] = f;
		player.mining_hits = temp;
		print ("Tier" + tier + ": " + f);
		if(f > player.highest_hit){ player.highest_hit = f; }
		player.last_hit = f;
		player.AddToInventory (new Bauxite (player), (int)f);
	}
}
