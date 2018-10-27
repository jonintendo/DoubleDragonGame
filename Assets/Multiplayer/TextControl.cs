using UnityEngine;
using System.Collections;

public class TextControl : MonoBehaviour {

	public TextMesh playerRedeText;


	 GameObject chat;

		Chat chatScript;

	// Use this for initialization



 void OnNetworkInstantiate (NetworkMessageInfo info) {



		Debug.Log("New object instantiated by " + info.sender);



}

void Awake()

	{
		chat=GameObject.Find("NetworkChat");

		playerRedeText.text="i";


	}

	void OnPlayerConnected(NetworkPlayer playerRede)
	{

	

	}
	
	
	Color GetColor(string PlayerColor)
	{
		Color color=Color.grey;
		
		switch(PlayerColor)
		{
		case "yellow":
			color=Color.yellow;
			break;
			
		case "green":
			color=Color.green;
			break;
			
			case "red":
			color=Color.red;
			break;
			
		case "blue":
			color=Color.blue;
			break;
			
		}
		
		
		
		
		return color;
	}
	
	void Start () {

	}


	// Update is called once per frame
	void Update () {
			if(playerRedeText.text=="i")
			{
				chat=GameObject.Find("NetworkChat");
					 chatScript = chat.GetComponent<Chat>();
						
			if(networkView.isMine)
			{
				
				
				try{
					
			     playerRedeText.text=chatScript.GetPlayerNode(networkView.owner).playerName;
			    renderer.material.color=GetColor(chatScript.GetPlayerNode(networkView.owner).color);
				
					}catch{
					
					}

			}else{
				
					
			try{
					
			     playerRedeText.text=chatScript.GetPlayerNode(networkView.owner).playerName;
			     renderer.material.color=GetColor(chatScript.GetPlayerNode(networkView.owner).color);
				
					}catch{
					}
					
			}

				}

			

	}
}
