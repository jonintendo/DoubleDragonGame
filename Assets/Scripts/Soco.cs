using UnityEngine;
using System.Collections;

public class Soco : MonoBehaviour {
	
	public AnimationClip punchAnimation;
	public AudioClip soco;
	
	private GameObject enemy;
	private GameObject player;
	
	bool soca  =true;
	
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
				
		
				
	}
	
	void Awake(){
	
		enemy=GameObject.Find("Enemy");
		player=this.transform.gameObject;
		
	}
	
	
	// Use this for initialization
	void Start () {
		
		
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyUp(KeyCode.LeftControl))
			soca=true;
			
		if (Input.GetKey (KeyCode.LeftControl) &&  !player.animation.IsPlaying(punchAnimation.name)&&soca)
		{
		soca=false;
		player.animation.Play(punchAnimation.name);
		//audio.PlayOneShot(soco);
		
		}
		
		
	}
}
