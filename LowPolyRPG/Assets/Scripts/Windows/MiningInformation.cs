using UnityEngine;
using System.Collections;

public class MiningInformation : MonoBehaviour {
	Mouse mouse;
	Player player;
	GUISkin skin;

	public Rect rect = new Rect(465, 20, 215, 125);//215
	Rect drag_zone;
	bool dragging_window = false;

	int margin = 5, margin_right = 55;//55
	//int button_size = 35;

	void Start () {
		mouse = GetComponent<Mouse> ();
		player = GetComponent<Player> ();
		skin = Resources.Load ("GUI Skins/Normal") as GUISkin;
		drag_zone = new Rect (0, 0, rect.width, skin.window.border.top);

		if (PlayerPrefs.GetInt ("MiningInformation set") == 1) {
			Vector2 pos = new Vector2 (PlayerPrefs.GetInt ("MiningInformation x"), PlayerPrefs.GetInt ("MiningInformation y"));
			rect.position = pos;
		}
	}

	void Update(){
		DragWindow ();
	}


	void OnGUI(){
		GUI.skin = skin;
		GUI.Window (2, this.rect, WindowFunction, "Mining Information");
		GUI.Box (drag_zone, "");
	}

	void WindowFunction(int windowID){
		MiningGraph ();

		Rect last_hit = new Rect (rect.width - (30 + (margin)), skin.window.border.top + margin + 15, 30, 30);
		GUI.Button (last_hit, player.last_hit.ToString());
		last_hit.y -= 20;
		GUI.Label (last_hit, "Hit");
	}

	Rect graph;
	void MiningGraph(){
		graph = new Rect (margin, skin.window.border.top + margin, (rect.width - margin_right) - (margin * 2), (rect.height - skin.window.border.top - (margin * 2)));

		Vector2 gap = new Vector2(graph.width / 10, graph.height / 10);
		Vector2 offset = new Vector2 (margin, skin.window.border.top + margin);

		//DROP AVERAGE GUI (Red Bar)
		GUI.color = Color.red;
		float avg = 0, i = 0;
		foreach (float f in player.mining_hits) {
			if(f != 0){
				avg += f;
				i++;
			}
		}
		avg /= i;
		float cut = (graph.height - margin) / player.highest_hit;
		Rect avg_bar = new Rect (new Vector2 (graph.position.x, graph.position.y + graph.size.y - 2.5f - (cut * avg)), new Vector2 (graph.width, 5));
		GUI.Box (avg_bar, GUIContent.none);
		Rect label = new Rect (avg_bar.position + new Vector2(avg_bar.width + margin, -8), new Vector2(25, 20));
		GUI.Label (label, avg.ToString());
		GUI.color = Color.white;

		//BIGGEST DROP GUI (Green Bar)
		GUI.color = Color.green;
		Rect biggest_bar = new Rect (new Vector2 (graph.x, graph.y), new Vector2 (graph.width, 5));
		GUI.Box (biggest_bar, GUIContent.none);
		label = new Rect (biggest_bar.position + new Vector2(biggest_bar.width + margin, -8), new Vector2(25, 20));
		GUI.Label(label, player.highest_hit.ToString());
		GUI.color = Color.white;

		//SLIDERS (Graph)
		int index = 0;
		foreach(float f in player.mining_hits){
			Rect slider = new Rect(index * gap.x + offset.x, offset.y, 20, graph.height);
			GUI.VerticalSlider (slider, player.mining_hits [index], player.highest_hit, 0);

			Rect slider_label = new Rect (new Vector2(1,2),
										  new Vector2(1,2));

			index++;
		}
	}

	void DragWindow(){
		drag_zone.position = rect.position;

		if(drag_zone.Contains(mouse.position) && Input.GetMouseButtonDown(0)){
			dragging_window = true;
		}else if(Input.GetMouseButtonUp(0)){
			dragging_window = false;
		}

		if(dragging_window && mouse.dragging){
			rect.position += mouse.drag_offset;
			mouse.ResetDragOffset ();
		}
	}

	void OnDestroy(){
		PlayerPrefs.SetInt ("MiningInformation set", 1);
		PlayerPrefs.SetInt ("MiningInformation x", (int)rect.position.x);
		PlayerPrefs.SetInt ("MiningInformation y", (int)rect.position.y);
	}
}
