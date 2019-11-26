using UnityEngine;
using System.Collections;

public class UserController : MonoBehaviour {

	private string key = "mySecretKey";
	string getUserURL = "http://localhost/CekehNetowork/AddUser.php?";

	string response;

	void Start () {
		StartCoroutine (Test());
	}

	void Update(){
		if(response != ""){
			print (response);
			response = "";
		}
	}

	IEnumerator Test(){
		WWW user = new WWW (getUserURL);
		yield return user;

		if(user.error == null){
			response = user.text;
		}
	}
}
