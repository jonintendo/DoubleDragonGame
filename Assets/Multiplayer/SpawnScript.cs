using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {
	
	public Transform camera;
	
	public Transform enemy;
	
	public Transform playerRede;
	
	public TextMesh playerRedeText;
	
	public Transform playerLocal;
	
	public Transform cameraLocal;

	
	
	int posx;
	int posz;
	private float timer=0;
	
	void OnServerInitialized()
	{
	
		SpawnPlayer();	
		
		
	}
	
	void OnConnectedToServer()
	{
			
	    SpawnPlayer();	
	}
	
	void OnDisconnectedFromServer()
	{
		Network.RemoveRPCs(Network.player);
		Network.DestroyPlayerObjects(Network.player);
		Application.LoadLevel(Application.loadedLevel);
	}
	
	void OnPlayerDisconnected(NetworkPlayer playerDaRede)
	{
		Network.RemoveRPCs(playerDaRede);
		Network.DestroyPlayerObjects(playerDaRede);
		
	}
	
	void OnPlayerConnected(NetworkPlayer playerRede)
	{

		
	}
	

    void OnLevelWasLoaded(int level) {
       
		GameObject[] others = GameObject.FindGameObjectsWithTag("Player");
		if (others.Length>0)
			return;
		
		if (Application.loadedLevelName=="DD" && !(Network.peerType==NetworkPeerType.Disconnected))
		{
         // yield  break WaitForSeconds(4.0);		
		SpawnPlayer();
		}else{
		
			SpawnLocal();
			
		}
        
    }
	
	
	void SpawnPlayer()
	{
		 
		Destroy(GameObject.Find("Camera(Clone)"));
		Destroy(GameObject.Find("Player(Clone)"));
		
		
		GameObject[] others = GameObject.FindGameObjectsWithTag("Enemy") ;

foreach(GameObject other in others)
{
    DestroyObject(other);
}
		
		//DestroyObject(GameObject.Find("Enemy"));
		
		playerRede.name="Player";
		
		Network.Instantiate(playerRede,new Vector3(0,5.948653f,-80),Quaternion.LookRotation(Vector3.zero),0);
		
		Network.Instantiate(playerRedeText,new Vector3(-3,5.948653f,-77),Quaternion.LookRotation(Vector3.zero),0);
			
		Network.Instantiate(enemy,new Vector3(-3,4,-72),Quaternion.LookRotation(Vector3.zero),0);
		
		Instantiate(camera);
		
	
		
		
	}
	
	void SpawnLocal()
	{
		playerLocal.name="Player";
		
		Instantiate(playerLocal);
		
		//cameraLocal.name="Cameralocal";
		
		Instantiate(cameraLocal);
	
	 
		
	}
	
	
	  void Awake() {
        DontDestroyOnLoad(transform.gameObject);
		
//	 if ( !(Network.peerType==NetworkPeerType.Disconnected))
//		{
//         // yield  break WaitForSeconds(4.0);		
//		SpawnPlayer();
//		}else{
//		
//			SpawnLocal();
//			
//		}
			
    }
	
	
	// Use this for initialization
	void Start () {
		
			GameObject[] others = GameObject.FindGameObjectsWithTag("Spawn");
		if(others.Length>1)
			Destroy(transform.gameObject);
	
	}
	
	// Update is called once per frame
	void Update () {
		
		 posx = Random.Range(-11, 11);
		 posz = Random.Range(-73, 76);
		
		timer+=Time.deltaTime;
		if (timer>30 )
		{
			timer=0;
			
			if(Network.peerType==NetworkPeerType.Disconnected && !(Application.loadedLevelName=="Opening"))
			
		Instantiate(enemy,new Vector3(posx,3.948653f,posz),Quaternion.LookRotation(Vector3.zero));
			else
			Network.Instantiate(enemy,new Vector3(-3,4,-72),Quaternion.LookRotation(Vector3.zero),0);	
			
			
		}
	
	}
}
