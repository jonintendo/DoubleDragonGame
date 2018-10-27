using UnityEngine;
using System.Collections;


public class EnemyControl : MonoBehaviour {
	
	public AnimationClip punchAnimation;
	public AudioClip soco;
	
	private GameObject enemy;
	private GameObject player;
	
	
	bool apanha  =true;
	private int socostomados=0;
	private int derrotados=0;
	
	
	
	// Use this for initialization
	void Start () {
		
	enemy= this.transform.gameObject;//  GameObject.Find("Enemy");
	
		
	}
	
	// Update is called once per frame
	void Update () {
		
		player=GameObject.Find("Player(Clone)");
		
		if(socostomados >10)
		{
			//GameObject instance  = (GameObject)Instantiate(enemy,new Vector3(posx,3.948653f,posz),Quaternion.LookRotation(Vector3.zero));

			Destroy(enemy);
		}
		
		
		
	if( player.animation.IsPlaying(punchAnimation.name) && Vector3.Distance(player.transform.position,enemy.transform.position)<2 && apanha)
		{		
		
		apanha=false;
		Debug.Log("tomando soco ");
			audio.PlayOneShot(soco);
		enemy.animation.Play("Ferido2");	
		
		socostomados +=1;
		}
		
		
		apanha=!enemy.animation.IsPlaying("Ferido2");	
		
		
		if(networkView.isMine)
		{
	this.transform.LookAt(new Vector3(player.transform.position.x,transform.position.y,player.transform.position.z));
	
	   // Move the object forward along its z axis 1 unit/second.
	if(Vector3.Distance(player.transform.position,transform.position)>2)
    this.transform.Translate(0.3f*Time.deltaTime, 0, 0.3f*Time.deltaTime);
		}
		
	}
	
}
