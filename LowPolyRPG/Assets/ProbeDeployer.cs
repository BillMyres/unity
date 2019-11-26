using UnityEngine;
using System.Collections;

public class ProbeDeployer : MonoBehaviour {

	Player player;
	Animator animator;
	AudioSource source;
	CameraController cam;

	public AudioClip load, explosion, drill;

	public bool probe = false, reload = false;
	bool explode = false, reset = false;

	public bool die = false, testing = false, test_x1000 = false, test_x100 = false, test_x10 = false;

	public delegate void UseFunction();
	public UseFunction use;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		animator = GetComponent<Animator> ();
		source = GetComponent<AudioSource> ();
		cam = Camera.main.GetComponent<CameraController> ();
		CheckForProbe ();
	}

	void CheckForProbe(){
		if (!probe) {
			use = Use_ProbeUnavailable;
		} else {
			use = Use_ProbeAvailable;
		}
	}

	void Use_ProbeUnavailable(){
		reload = true;


	}

	void Use_ProbeAvailable(){

		source.volume = 1;
		source.PlayOneShot (drill);
		animator.SetTrigger ("deploy");

		probe = false;
		explode = true;

		use = Use_ProbeUnavailable;
	}

	int x = 0;
	void Update () {

		if(test_x1000){
			if(x == 1000){ x = 0; test_x1000 = false; return; }
			Drop (1f);

			x++;
			return;
		} else if(test_x100){
			if(x == 100){ x = 0; test_x100 = false; return; }
			Drop (10);

			x++;
			return;
		}else if(test_x10){
			if(x == 10){ x = 0; test_x10 = false; return; }
			Drop (10);

			x++;
			return;
		}

		CheckForProbe ();

		animator.SetBool ("probe", probe);

		if (!testing) {
			if (explode && animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1) {
				explode = false;
				source.volume = .1f;
				source.PlayOneShot (explosion);
				cam.Shake (0.1f);
				Drop (10);
				reset = true;
			}

			if (reload && !source.isPlaying && !explode) {
				reload = false;
				source.volume = 1;
				source.PlayOneShot (load);
				probe = true;
			}
		} else {
			if (reload) {
				reload = false;
				Drop (10);
			}
		}

		if(reset){
			animator.SetTrigger ("reset");
			reset = false;
		}

		if(die && !source.isPlaying){
			die = false;
			Die ();
		}
	}

	void Die(){
		
		foreach(Transform child in transform){
			Rigidbody rb = child.gameObject.AddComponent<Rigidbody> ();
			rb.AddExplosionForce (100f, transform.position + Random.insideUnitSphere, 5, 3f);
		}

		if(!fading){
			StartCoroutine (FadeOut ());
		}
	}

	public float time = 3f, alpha = 1f;
	public bool fading = false;
	IEnumerator FadeOut(){
		fading = true;

		while(alpha > 0){
			if (time <= 1f) {
				alpha -= Time.deltaTime + 0.01f;

				Renderer[] renderer = GetComponentsInChildren<MeshRenderer> ();

				foreach (Renderer r in renderer) {
					r.material.SetFloat ("_Alpha", alpha);
				}
			}

			time -= Time.deltaTime;
			yield return null;
		}

		fading = false;
		Destroy (transform.gameObject);
	}

	void Drop(float tier){
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

		f *= tier;


		float range = Random.Range(1f, 10f);
		if(range > 9.9f){
			float r = Random.Range (1f, 10f);
			range += tier * r;
			if(r > 9.9f){
				range *= tier * Random.Range (1f, 10f);
			}
		}

		f = (int)(f * range);

		player.content.drops++;
		float keep = Random.Range (1, 10);
		//if(keep != 1){ print ("Nothing..."); return; }

		float[] temp = new float[] {0,0,0,0,0, 0,0,0,0,0};

		for(int i = 0; i < 9; i++){
			if (player.content.hits [i + 1] != null) {
				temp [i] = player.content.hits [i + 1];
			}
		}

		temp [9] = f;
		player.content.hits = temp;
		print ("Tier" + tier + ": " + f + " -- " + (int)range);
		if(f > player.content.all_time_highest_hit){ player.content.all_time_highest_hit = f; }
		player.content.last_hit = f;
		player.content.all_hits += f;
		player.AddToInventory (new Bauxite (player), (int)f);
	}
}
