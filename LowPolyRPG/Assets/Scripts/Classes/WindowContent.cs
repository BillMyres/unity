using UnityEngine;
using System.Collections;

[System.Serializable]
public class WindowContent {

	public WindowContent(){}

	public float[] hits = new float[10];
	int graph_points = 10;
	public float hit = 0, all_hits = 0, last_hit = 0, lowest_hit = 99999, highest_hit = 0, all_time_highest_hit = 0, median = 0, average = 0;
	public int drops = 0;

	public void Mining(Rect rect){
		GetStats ();

		GUI.color = Color.gray;
		GUI.Box (rect, GUIContent.none);

		Graph (rect);
		GraphStats (rect);
	}

	public void GraphStats(Rect rect){
		GUISkin skin = GUI.skin;
		GUIStyle style = new GUIStyle (skin.box);
		style.fontSize = 15;

		Rect box = new Rect (rect.position + new Vector2(0, rect.height / 2), new Vector2(rect.width, rect.height / 2));

		float stat_width = (box.width - (GUI.skin.window.border.left * 1.5f)) / 2;
		float stat_height = (box.height - (GUI.skin.window.border.left * 7f)) / 6;

		Vector2 vertical_gap = new Vector2 (0, stat_height + GUI.skin.window.border.left);

		Rect stat = new Rect (box.position + new Vector2(GUI.skin.window.border.left, GUI.skin.window.border.left), new Vector2(stat_width, stat_height));

		GUI.color = Color.white;
		GUI.Box (stat, "Last Hit: " + last_hit.ToString(), style);

		stat.position += vertical_gap;
		GUI.Box (stat, "Average: " + average.ToString(), style);

		stat.position += vertical_gap;
		GUI.Box (stat, "Median: " + median.ToString(), style);

		stat.position += vertical_gap;
		GUI.Box (stat, "Highest Hit: " + all_time_highest_hit.ToString(), style);

		stat.position += vertical_gap;
		GUI.Box (stat, "Drops: " + drops.ToString(), style);

		stat.position += vertical_gap;
		GUI.Button (stat, "Obtained: " + all_hits.ToString(), style);
	}

	//MINING STUFFS
	void GetStats(){
		hit = 0;
		highest_hit = 0;
		lowest_hit = 99999;

		foreach(float f in hits){
			hit += f;
			if(f > highest_hit){ highest_hit = f; }
			if(f < lowest_hit){ lowest_hit = f; }
		}

		median = hit / hits.Length;
		average = all_hits / drops;
	}

	void Graph(Rect rect){
		GUISkin skin = GUI.skin;

		GUIStyle style = new GUIStyle (GUI.skin.box);
		style.fontSize = 9;

		Rect graph = new Rect (rect.position, new Vector2(rect.width, rect.height / 2));
		GUI.color = Color.black;
		GUI.Box (graph, GUIContent.none);
		GUI.color = Color.white;

		float point_width = graph.width / (graph_points + 2f);
		float point_height = graph.height / highest_hit;//graph is 100 points tall

		//Median Bar
		GUI.color = new Color(0.3f, 0.3f, 0.3f);
		Rect median_bar = new Rect (graph.position + new Vector2(0, graph.height - (median * point_height)), new Vector2(graph.width - (point_width / 2), 4));
		GUI.Box (median_bar, GUIContent.none);

		//Average Bar
		GUI.color = new Color(0.8f, 0.2f, 0f);
		Rect average_bar = new Rect (graph.position + new Vector2(0, graph.height - (average * point_height)), new Vector2(graph.width, 4));
		GUI.Box (average_bar, GUIContent.none);

		//Median Marker
		GUI.color = new Color(0.3f, 0.3f, 0.3f);
		Rect median_marker = new Rect (graph.position + new Vector2(point_width * (graph_points + 1f), graph.height - (median * point_height)), new Vector2(point_width / 2, median * point_height + graph.height));
		GUI.Box (median_marker, "M\ne\nd\ni\na\nn", style);

		//Average Marker
		GUI.color = new Color(0.8f, 0.2f, 0f);
		Rect average_marker = new Rect (graph.position + new Vector2(point_width * (graph_points + 1.5f), graph.height - (average * point_height)), new Vector2(point_width / 2, average * point_height + graph.height));
		GUI.Box (average_marker, "A\nv\ne\nr\na\ng\ne", style);

		for(int i = 1; i < graph_points; i++){
			float height = graph.height - ((hits[i] - lowest_hit) * point_height);
			Vector2 point = graph.position + new Vector2 (point_width * (i + 1), height) + new Vector2(point_width / 2, point_width / 2);

			float previous_hit = (hits [i - 1] - lowest_hit) * point_height;
			float current_hit = (hits [i] - lowest_hit) * point_height;

			Vector2 previous = graph.position + new Vector2 (point_width * i, graph.height - previous_hit);
			Vector2 current = graph.position + new Vector2(point_width * (i + 1), graph.height - current_hit);

			Drawing.DrawLine (previous, current, Color.white, 2f);
		}

		for (int i = 0; i < graph_points; i++) {
			float height = graph.height - ((hits[i] - lowest_hit) * point_height);
			Rect point = new Rect (graph.position + new Vector2(point_width * (i + 1), height) + new Vector2(0, -5), new Vector2(point_width, 10));

			float h = hits [i];
			Color color = Color.white;
			if (h > average) {
				color = Color.green;
			} else {
				color = Color.red;
			}

			if(h == highest_hit){
				color = Color.yellow;
			}

			GUI.color = color;
			if (GUI.Button (point, hits [i].ToString (), style)) {
				Debug.Log (hits[i].ToString());
			}

			GUI.color = Color.white;
		}

		GUI.color = Color.white;
		Rect highest = new Rect (graph.position, new Vector2(point_width, point_width / 2));
		GUI.Box (highest, highest_hit.ToString(), style);

		Rect lowest = new Rect (graph.position + new Vector2(0, graph.height - (point_width / 2)), new Vector2(point_width, point_width / 2));
		GUI.Box (lowest, lowest_hit.ToString(), style);
	}
}
