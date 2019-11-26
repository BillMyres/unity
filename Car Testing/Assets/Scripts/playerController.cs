using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

	public GameObject[] vehicle;
	GameObject activeObj;

	public int activeNum = 0, changeNum;

	void Start () {
		changeNum = activeNum;
	}

	void Update () {
		if (!GameObject.FindWithTag (vehicle [activeNum].tag)) {
			//print (transform.position + " : " + activeNum );
			Instantiate (vehicle [activeNum], transform.position + vehicle[activeNum].transform.position, vehicle[activeNum].transform.rotation);
			activeObj = GameObject.FindWithTag (vehicle [activeNum].tag);
			transform.parent = activeObj.transform;
			transform.localPosition = Vector3.zero;
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}

		if(activeNum != changeNum){
			transform.parent = null;
			GameObject.Destroy (activeObj);
			activeNum = changeNum;
		}

		if(Input.GetKeyDown(KeyCode.Tab)){
			changeNum++;
			int num = vehicle.Length;
			if(changeNum > num - 1){ changeNum -= num;}
		}
	}
}
