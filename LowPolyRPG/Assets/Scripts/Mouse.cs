using UnityEngine;
using System.Collections;

public class Mouse : MonoBehaviour{

	Player player;

	public bool dragging = false;
	public Vector2 drag_offset;

	public Vector2 position;

	public void Start(){
		player = GetComponent<Player> ();
	}

	#region Unity Callbacks
	public void Update(){
		position = new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y);
		dragging = IsDragging ();

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if(Physics.Raycast(ray, out hit)){
			ObjectInformation info = hit.transform.GetComponent<ObjectInformation> ();
			if (info) {
				if (Input.GetMouseButtonUp (0) && info.item.type != ItemType.Deployer) {
					player.AddToInventory(info.item, info.item.count);
					info.item.DestroyWorldObject ();
				} else if (Input.GetMouseButtonUp (1)) {
					RightClickMenu rcm = transform.gameObject.AddComponent<RightClickMenu> ();
					rcm.Init (info.item, WindowType.WorldObject);
					print (info.item.type.ToString());
				}
			}
		}
	}
	#endregion

	#region Drag Detection
	bool clicked = false;
	Vector2 clicked_position;

	bool IsDragging(){
		bool isdragging = false;

		if(Input.GetMouseButtonDown(0)){
			clicked_position = position;
			clicked = true;
		}else if(Input.GetMouseButtonUp(0)){
			isdragging = false;
			clicked = false;
		}

		if(clicked && clicked_position != position){
			drag_offset = position - clicked_position;
			isdragging = true;
		}
		return isdragging;
	}

	public void ResetDragOffset(){
		clicked_position = position;
	}
	#endregion

}
