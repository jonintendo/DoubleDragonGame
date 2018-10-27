using UnityEngine;
using System.Collections;

public class MenuGUI : MonoBehaviour {
	
	public GUISkin menuSkin;
	public float width;
	public float height;
	
	private bool showMenu;
	
	private delegate void GUIMethod();
    private GUIMethod currentMenu;
	
	
	void OnGUI()
	{
		
				
		if(!showMenu)
        return;
		currentMenu();
	}
	
	
	void OnConnectedToServer()
	{
		showMenu=false;	
		currentMenu=MainMenu;
	}
	
	void OnServerInitialized()
	{
		
		showMenu=false;	
		currentMenu=MainMenu;
		
	}
	
	
	void OpeningGame()
		
	{
	    GUI.skin=menuSkin;
		
		float screenX=Screen.width*0.5f-width*0.5f;
		float screenY=Screen.height*0.5f+height*0.5f;
		GUILayout.BeginArea(new Rect(screenX,screenY,width,height));
		 
		
		if(GUILayout.Button("1P Game"))
		{
			
			showMenu=false;
		    currentMenu=MainMenu;
			Application.LoadLevel("DD");
		
		}
		
		if(GUILayout.Button("Multiplayer Game"))
		{
			GameObject menuObj = GameObject.Find("NetworkChat");
			MPBase2 menu = menuObj.GetComponent<MPBase2>();
			menu.Toggle();
			
			currentMenu=MultiplayerMenu;
		
		}
		
			GUILayout.EndArea();
	}
	
	
	void MultiplayerMenu()
	{
		GUI.skin=menuSkin;
		
		float screenX=Screen.width*0.5f-width*0.5f;
		float screenY=Screen.height*0.5f+height*0.5f;
		GUILayout.BeginArea(new Rect(screenX,screenY,width,height));
		 
		
		if(GUILayout.Button("Return"))
		{
			GameObject menuObj = GameObject.Find("NetworkChat");
			MPBase2 menu = menuObj.GetComponent<MPBase2>();
			menu.Toggle();
			currentMenu=OpeningGame;
		
		}
		
		
		
			GUILayout.EndArea();
	}
	
	
	
void MainMenu()
{
	 	
		GUI.skin=menuSkin;
		
		float screenX=Screen.width*0.5f-width*0.5f;
		float screenY=Screen.height*0.5f-height*0.5f;
		GUILayout.BeginArea(new Rect(screenX,screenY,width,height));
		
		if(GUILayout.Button("Play"))
		{
		Debug.Log("Play button!");
		}
		if(GUILayout.Button("Options"))

		{
			Debug.Log("Options button!");
			currentMenu=OptionsMenu;		
		}

		if(GUILayout.Button("Quit"))

		{
			currentMenu=OpeningGame;		
			Application.LoadLevel("Opening");
		}
		GUILayout.EndArea();	
		
	
	}
	
	void OptionsMenu()
{
GUI.skin = menuSkin;
		
float screenX=Screen.width*0.5f-width*0.5f;
		float screenY=Screen.height*0.5f-height*0.5f;
				
GUILayout.BeginArea(new Rect(screenX,screenY,width,height));
		
GUILayout.Label("Settings");
if(GUILayout.Button("Sound"))

		{
			Debug.Log("Sound !");
			currentMenu=Sound;		
		}
	if(GUILayout.Button("Buttons"))

		{
			Debug.Log("Buttons!");
			currentMenu=Buttons;		
		}	
	if(GUILayout.Button("Camera"))

		{
			Debug.Log("Camera");
			currentMenu=Cameras;		
		}	
		
if(GUILayout.Button("Return to main menu"))
currentMenu=MainMenu;
		
GUILayout.EndArea();
}
	
void Sound()
{
GUI.skin = menuSkin;
		
float screenX=Screen.width*0.5f-width*0.5f;
		float screenY=Screen.height*0.5f-height*0.5f;
				
GUILayout.BeginArea(new Rect(screenX,screenY,width,height));
		
GUILayout.Label("Sound");

		
if(GUILayout.Button("Return"))
currentMenu=OptionsMenu;
		
GUILayout.EndArea();
}	
	
	void Buttons()
{
GUI.skin = menuSkin;
		
float screenX=Screen.width*0.5f-width*0.5f;
		float screenY=Screen.height*0.5f-height*0.5f;
				
GUILayout.BeginArea(new Rect(screenX,screenY,width,height));
		
GUILayout.Label("Button");

		
if(GUILayout.Button("Return"))
currentMenu=OptionsMenu;
		
GUILayout.EndArea();
}	
	
	void Cameras()
{
GUI.skin = menuSkin;
		
float screenX=Screen.width*0.5f-width*0.5f;
		float screenY=Screen.height*0.5f-height*0.5f;
				
GUILayout.BeginArea(new Rect(screenX,screenY,width,height));
		
GUILayout.Label("Cameras");

		
if(GUILayout.Button("Return"))
currentMenu=OptionsMenu;
		
GUILayout.EndArea();
}	
	
	
	
	public bool ShowMenu {
get{return showMenu; }
}
	
public void Toggle() {
showMenu=!showMenu;
}
	
	
	void Awake() {
        DontDestroyOnLoad(transform.gameObject);
		
		GameObject[] others = GameObject.FindGameObjectsWithTag("Menu");
		if(others.Length>1)
		{
			DestroyImmediate ( this.gameObject);
		}else
		{
		if(Application.loadedLevelName=="Opening")
		{
		showMenu=true;
		currentMenu=OpeningGame;
		}else{
	    showMenu=false;
		currentMenu=MainMenu;
		}
		}
    }
	
	
	
	// Use this for initialization
	void Start () {
		
		
		
	}
	
	
	// Update is called once per frame
	void Update () {
		
		if(Application.loadedLevelName=="Opening")
			return;
		
		
		if(Input.GetKeyDown(KeyCode.Escape))
{
GameObject menuObj = GameObject.Find("Menu");
MenuGUI menu = menuObj.GetComponent<MenuGUI>();
menu.Toggle();
//SetMotionStatus(!menu.ShowMenu);
}
	
	
			
			
	if(Input.GetKeyDown(KeyCode.F3))
{
GameObject menuObj = GameObject.Find("NetworkChat");
MPBase2 menu = menuObj.GetComponent<MPBase2>();
menu.Toggle();
//SetMotionStatus(!menu.ShowMenu);
}	
		
		
		
	
	}
}
