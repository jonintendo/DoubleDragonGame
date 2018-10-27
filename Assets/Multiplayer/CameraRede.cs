using UnityEngine;
using System.Collections;
using System;

public class CameraRede : MonoBehaviour {

	public GameObject target;
    Vector3 offset;
   
	void Awake()

	{
		 // if(!NetworkView.isMine)
		{
    //enabled=false;  

    }
		
		
		offset = transform.position - target.transform.position;
	}
	
	
	void Start() {
       
		
	}
	
	void LateUpdate() {
	
   target=GameObject.Find("Player(Clone)");
    Vector3 desiredPosition = target.transform.position + offset;
    this.transform.position = desiredPosition;
		
		
}
	


	// Update is called once per frame
	void Update () {
	
		
	}
}
