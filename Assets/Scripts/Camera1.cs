using UnityEngine;
using System.Collections;

public class Camera1 : MonoBehaviour {
	
	
	public GameObject target;
    Vector3 offset;
   
		
	void Start() {
		
        offset = transform.position - target.transform.position;
		
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
