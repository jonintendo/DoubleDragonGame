using UnityEngine;
using System.Collections;
using System;

public class Chat : MonoBehaviour {
	
	public bool usingChat=false;
	public bool showchat=false;
	
	string inputfield="";
	Vector2 scrollposition;
	int width=500;
	int height=100;	
	float lastUnfocusTime=0;
	Rect window;
	
	string playerName;
	string playerColor="yellow";
	
	
	
	public ArrayList playerList=new ArrayList();
	
	public class PlayerNode
	{
	    public string playerName;
		public NetworkPlayer player;
		public string color;
		
	}
	ArrayList chatEntries=new ArrayList();
	
	 class ChatEntry
	{
		public string name="";
		public string text ="";
	}
	
	
	
	void OnConnectedToServer()
	{
		playerName=PlayerPrefs.GetString("playerName","");
		if(playerName=="") playerName ="RandomName"+UnityEngine.Random.Range(1,999);
		playerColor=PlayerPrefs.GetString("playerColor","");
		ShowChatWindow();
		
		
		networkView.RPC("TellServerOurName",RPCMode.Server,playerName,Network.player,playerColor);
		addGameChatMessage(playerName+" have just joined the chat");
		
	//.GetComponent(NetworkView).networkView.RPC("MyFunction", RPCMode.AllBuffered, vars);	//networkView.RPC("JustEntriedPlayer",RPCMode.All,playerName);
		
	}
	
	void OnServerInitialized()
	{
		
		playerName=PlayerPrefs.GetString("playerName","");
		playerColor=PlayerPrefs.GetString("playerColor","");
		
		if(playerName=="") playerName ="RandomName"+UnityEngine.Random.Range(1,999);
		
		ShowChatWindow();
		PlayerNode newEntry= new PlayerNode();
		
		newEntry.playerName=playerName;
		newEntry.player=Network.player;
		newEntry.color=playerColor;
		
		playerList.Add(newEntry);
		addGameChatMessage(playerName+ "have just joined the chat");
		
	}
	
	
	
	
	    void OnPlayerConnected(NetworkPlayer player) 
	{
 //GetPlayerNode( player);
		
    }
	
	
	
	
	void OnPlayerDisconnected(NetworkPlayer netPlayer)
	{
	
		playerList.Remove(GetPlayerNode(netPlayer));
		addGameChatMessage(playerName+"has disconnected from the server");
	}
	
	
	void OnDisconnectedFromServer()
	{
	CloseChatWindow();	
	}
	
	
	public PlayerNode GetPlayerNode(NetworkPlayer netPlay)
	{
	
		foreach(PlayerNode entry in playerList)
		{
			if(entry.player==netPlay)
			{
				return entry;
			}
		}
		Debug.LogError("GetPlayerNode: Requested a playernode of nonexisting player!");
		return null;
		
	}
	
	
	
	
	
	void CloseChatWindow()
	{
	showchat=false;
		inputfield="";
		chatEntries=new ArrayList();
			
	}
	
	void ShowChatWindow()
	{
	showchat=true;
		inputfield="";
		chatEntries=new ArrayList();
			
	}
	
	void OnGUI()
		
	{
	if(!showchat) return;
		
		if(Event.current.type==EventType.keyDown && Event.current.character=='\n' & inputfield.Length<=0)
		{
			if(lastUnfocusTime +.25f< Time.time)
			{
				usingChat=true;
				GUI.FocusWindow(5);
				GUI.FocusControl("Chat input field");
				
			}
			
		}
		
		window=GUI.Window(5,window ,GlobalChatWindow,"");
	}
	
	void GlobalChatWindow(int id)
	{
	GUILayout.BeginVertical();
		GUILayout.Space(10);
		GUILayout.EndVertical();
		scrollposition =GUILayout.BeginScrollView(scrollposition);
		
		foreach(ChatEntry entry in chatEntries)
		{
		GUILayout.BeginHorizontal();
			if(entry.name==" - ")
				GUILayout.Label(entry.name+entry.text);
			else
				GUILayout.Label(entry.name+";"+entry.text);
		GUILayout.EndHorizontal();
			GUILayout.Space(2);
		
		}
		
		GUILayout.EndScrollView();
		
		if(Event.current.type==EventType.keyDown && Event.current.character == '\n' & inputfield.Length>0)
		{
			
			HitEnter(inputfield);
		}
		GUI.SetNextControlName("Chat input field");
		inputfield=GUILayout.TextField(inputfield);
		
		if(Input.GetKeyDown("mouse 0"))
		{
			if(usingChat)
			{
			//usingChat=false;
			//GUI.UnfocusWindow();
			//lastUnfocusTime=Time.time;
			}
		}
		
		
	}
	
	void HitEnter(string msg)
	{
		
		msg=msg.Replace('\n',' ');
		
		ApplyGlobalChatText(" - ",msg);
		if(Network.connections.Length>0)
		networkView.RPC("ApplyGlobalChatText",RPCMode.Others,playerName,msg);
		
		//networkView.RPC("ApplyGlobalChatText",RPCMode.All,playerName,msg);
		
		
	}
	
	void addGameChatMessage(string str)
	{
	ApplyGlobalChatText(" - ",str);
		if(Network.connections.Length>0)
			networkView.RPC("ApplyGlobalChatText",RPCMode.Others," - ",str);
		
	}
	
	
	[RPC]
	void TellServerOurName(string name,NetworkPlayer player,string color)
	{
		
		PlayerNode newEntry=new PlayerNode();
		
		newEntry.playerName=name;
		newEntry.player= player;
		newEntry.color=color;
		
		playerList.Add(newEntry);
				
		foreach(PlayerNode entry in playerList)
		{
			
		networkView.RPC("SendPlayersOnGame",RPCMode.Others,entry.player,entry.playerName, entry.color);
		}
		
		addGameChatMessage(playerName+ " have just joined the chat");

	}
	
	
	
	[RPC]
	void ApplyGlobalChatText(string name,string msg)
	{
			
		ChatEntry entry=new ChatEntry();
		entry.name=name;
		entry.text=msg;
		
		chatEntries.Add(entry);
		if(chatEntries.Count>4)
			chatEntries.RemoveAt(0);
		scrollposition.y=1000000;
		inputfield="";
		
	}
	
	
		[RPC]
	void SendPlayersOnGame(NetworkPlayer playerC, string playernameC, string colorC)
	{
		//if(GetPlayerNode(playerC)!=null)
		{
			//Debug.Log("aquiiiii"+playernameC);
		PlayerNode newEntry=new PlayerNode();
			
		newEntry.playerName=playernameC;
		newEntry.player=playerC;
		newEntry.color=	colorC;
			
		playerList.Add(newEntry);
		}
		
	}
	
	void Awake() {
        DontDestroyOnLoad(transform.gameObject);
		
    }
	
	
	// Use this for initialization
	void Start () {
	
		GameObject[] others = GameObject.FindGameObjectsWithTag("Chat");
		if(others.Length>1)
			Destroy(transform.gameObject);
		
		window =new Rect(Screen.width/2 -width/2,Screen.height-height+5,width,height);
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	 //if(Network.peerType==NetworkPeerType.Disconnected)
		//{
		//}else
		//{
			
		//if(networkView.isMine)	
				//playerRedeText.text=GetPlayerNode(networkView.owner).playerName;
		//	else
			//	playerRedeText.text=GetPlayerNode(networkView.owner).playerName;
		
		//}
	}
}
