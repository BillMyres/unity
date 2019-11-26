using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Sfs2X;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Requests;
using Sfs2X.Entities;

public class NetworkManager : MonoBehaviour {

	SmartFox SERVER;
	string IP = "127.0.0.1";
	int TCP = 9933, WSP = 8888;

	public bool login = false;

	List<Room> rooms = new List<Room>();

	void Start () {
		SERVER = new SmartFox ();

		//Event listener for the connection
		SERVER.AddEventListener (SFSEvent.CONNECTION, OnConnection);
		SERVER.AddEventListener (SFSEvent.CONNECTION_LOST, OnConnectionLost);

		//Event listener for the login data
		SERVER.AddEventListener(SFSEvent.LOGIN, OnLogin);
		SERVER.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);

		//
		SERVER.AddEventListener(SFSEvent.USER_VARIABLES_UPDATE, Variable_Update);

		//Server details
		ConfigData config = new ConfigData();
		config.Host = IP;
		config.Port = TCP;
		config.Zone = "BasicExamples";
		config.Debug = true;

		//Attempt connection
		SERVER.Connect (config);

		rooms = SERVER.RoomManager.GetRoomList();


		if(rooms.Count == 0){
			MMORoom room = new MMORoom (0, "johnnny");
			rooms.Add (room);
			print (room.Name);
		}

	}

	void Update(){
		//ProcessEvents : Run Listeners
		if (SERVER != null) {
			SERVER.ProcessEvents ();

			//message = 

			if (login) {
				login = false;
				SERVER.Send (new LoginRequest ("Pollie", "000", "BasicExamples"));
			}
		}
	}

	//CONNECTION
	void OnConnection(BaseEvent e){
		if ((bool)e.Params ["success"]) {
			print ("Connected!");
		} else {
			print ("Unable to Connect");
		}
	}

	void OnConnectionLost(BaseEvent e){
		print ("Lost Connection...");
	}

	//LOGIN
	void OnLogin(BaseEvent e){
		User user = e.Params ["user"] as User;
		//print (user.GetVariable);
		print ("Hello, " + e.Params["user"].ToString());
		//print(e.Params[""]);
	}

	void OnLoginError(BaseEvent e){
		print ("Wrong Username or Password.");
	}

	//
	void Variable_Update(BaseEvent e){
		User user = e.Params ["user"] as User;
		print (user.Name);
	}

	//DESTROY
	void OnApplicationQuit(){
		SERVER.Send (new LogoutRequest ());
		SERVER.Disconnect ();
	}
}
