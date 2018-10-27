using UnityEngine;
using System.Collections;
using System;

public class MPBase2 : MonoBehaviour {
	
	public GUISkin menuSkin;
	public float width;
	public float height;
	
	private bool showMenu;
	
	private delegate void GUIMethod();
private GUIMethod currentMenu;
	
	
	public string connectToIp="127.0.0.1";
	public int connectPort=25000;
	public bool useNAT=false;
	public string ipaddress="";
	public string port="";
	
	
	string playerName="<NAME ME>";
	string playerColor="blue";
	
	
	bool desconecta=false;
	
	void OnLevelWasLoaded(int level) {
        if (Application.loadedLevelName=="Opening" && !(Network.peerType==NetworkPeerType.Disconnected))
		{
        desconecta=true;
				
			
		}
        
    }
	
	
	
	
	void OnGUI()
	{
		if(!showMenu)
return;
		
		if(Network.peerType==NetworkPeerType.Disconnected)
		{
		
			if(GUILayout.Button("Connect"))
			{
			
				if(Application.loadedLevelName=="Opening")
				{
					Application.LoadLevel("DD");
				}
				
					Network.Connect(connectToIp,connectPort);
						
					PlayerPrefs.SetString("playerName",playerName);
				    PlayerPrefs.SetString("playerColor",playerColor);
				
			}
		
		
		if(GUILayout.Button("Start Server"))
		{
			
				if(Application.loadedLevelName=="Opening")
				{
					Application.LoadLevel("DD");
				}
				
				//if(playerName != "<NAME ME>")
				{
					PlayerPrefs.SetString("playerName",playerName);
					PlayerPrefs.SetString("playerColor",playerColor);
					Network.InitializeServer(32,connectPort);
					
//				foreach(GameObject go in FindObjectOfType(typeof(GameObject)))
//				{
//					go.SendMessage("OnNetworkLoadedLevel",SendMessageOptions.DontRequireReceiver);
//					
//				}
				
			
					
				}
			}
			
			playerName=GUILayout.TextField(playerName);
			playerColor=GUILayout.TextField(playerColor);
			connectToIp=GUILayout.TextField(connectToIp);
			connectPort= Convert.ToInt32( GUILayout.TextField(connectPort.ToString()));
		}
		
	else{
			
			
		if(Network.peerType==NetworkPeerType.Connecting) GUILayout.Label("Connect Status : Connecting");
			else if(Network.peerType==NetworkPeerType.Client)
			{
				GUILayout.Label("Connection Status : Client!");
					GUILayout.Label("Ping to Server "+ Network.GetAveragePing(Network.connections[0]));
			}
		else if(Network.peerType==NetworkPeerType.Server)
			{
				GUILayout.Label("Connection Status : Server!");
				GUILayout.Label("Connections:  "+ Network.connections.Length);
				
		if(Network.connections.Length>=1)
					GUILayout.Label("Ping to Server: " +Network.GetAveragePing(Network.connections[0]));
			}
			
			
			
		if(GUILayout.Button("Disconect"))
			{
				Network.Disconnect(200);
			}
			
			ipaddress=Network.player.ipAddress;
			port=Network.player.port.ToString();
			GUILayout.Label("IP Address = "+ipaddress+":"+port);
			
			if (Application.loadedLevelName=="Opening" && desconecta )
			{
			Network.Disconnect(200);
			}
			//return;
		
	}
		
				
//		if(!showMenu)
//return;
		//currentMenu();
	}
	
	
	
	public bool ShowMenu {
get{return showMenu; }
}
	
public void Toggle() {
showMenu=!showMenu;
}
	
	
	// Use this for initialization
	void Start () {
		
	showMenu=false;
		
	}
	
	
	// Update is called once per frame
	void Update () {
	
	}
}
