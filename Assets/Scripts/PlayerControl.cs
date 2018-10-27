using UnityEngine;
using System.Collections;



public class PlayerControl : MonoBehaviour {
	
	public AnimationClip punchAnimation;
	
	public AnimationClip idleAnimation;
	public AnimationClip walkAnimation  ;
	public AnimationClip runAnimation   ;
	public AnimationClip jumpPoseAnimation  ;
	public AudioClip soco;
	
	
	public float walkMaxAnimationSpeed   = 0.75f;
public float trotMaxAnimationSpeed = 1.0f;
public float runMaxAnimationSpeed  = 1.0f;
public float jumpAnimationSpeed  = 1.15f;
public float landAnimationSpeed  = 1.0f;
	
	
	private GameObject enemy;
	private GameObject player;
	
	bool soca  =true;
	
	
	private CharacterState _characterState ;
	private Animation _animation ;

// The speed when walking
float walkSpeed = 2.0f;
// after trotAfterSeconds of walking we trot with trotSpeed
float trotSpeed = 4.0f;
// when pressing "Fire3" button (cmd) we start running
float runSpeed = 6.0f;

float inAirControlAcceleration = 3.0f;

// How high do we jump when pressing jump and letting go immediately
float jumpHeight = 0.5f;

// The gravity for the character
float gravity = 20.0f;
// The gravity in controlled descent mode
float speedSmoothing = 10.0f;
float rotateSpeed = 500.0f;
float trotAfterSeconds = 3.0f;

bool canJump = true;

private float jumpRepeatTime = 0.05f;
private float jumpTimeout = 0.15f;
private float groundedTimeout = 0.25f;



// The current move direction in x-z
private Vector3 moveDirection = Vector3.zero;
// The current vertical speed
private float verticalSpeed = 0.0f;
// The current x-z move speed
private float moveSpeed = 0.0f;

// The last collision flags returned from controller.Move
private CollisionFlags collisionFlags ; 

// Are we jumping? (Initiated with jump button and not grounded yet)
private bool jumping = false;
private bool jumpingReachedApex = false;

// Are we moving backwards (This locks the camera to not do a 180 degree spin)
private bool movingBack = false;
// Is the user pressing any keys?
private bool isMoving = false;
// When did the user start walking (Used for going into trot after a while)
private float walkTimeStart = 0.0f;
// Last time the jump button was clicked down
private float lastJumpButtonTime = -10.0f;
// Last time we performed a jump
private float lastJumpTime = -1.0f;


// the height we jumped from (Used to determine for how long to apply extra jump power after jumping.)
private float lastJumpStartHeight = 0.0f;


private Vector3 inAirVelocity = Vector3.zero;

private float lastGroundedTime = 0.0f;


private bool isControllable = true;

	
enum CharacterState {
	Idle = 0,
	Walking = 1,
	Trotting = 2,
	Running = 3,
	Jumping = 4,
	Punching=5,
}
	
	
	
	void SetMotionStatus(bool status)
	{
		this.GetComponent<ThirdPersonController>().enabled=status;
		this.GetComponent<MouseLook>().enabled=status;
		Camera.mainCamera.GetComponent<MouseLook>().enabled=status;
	}
	
bool IsGrounded () {
	return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
}	
	
	void ApplyGravity()
{
	if(isControllable)	// don't move player at all if not controllable.
	{
		// Apply gravity
		var jumpButton = Input.GetButton("Jump");
		
		
		// When we reach the apex of the jump we send out a message
		if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0)
		{
			jumpingReachedApex = true;
			SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
		}
	
		if (IsGrounded())
			verticalSpeed = 0.0f;
		else
			verticalSpeed -= gravity * Time.deltaTime;
	}
}
	
	void ApplyJumping ()
{
	// Prevent jumping too fast after each other
	if (lastJumpTime + jumpRepeatTime > Time.time)
		return;

	if (IsGrounded()) {
	
		if (canJump && Time.time < lastJumpButtonTime + jumpTimeout) 
			{
			verticalSpeed = CalculateJumpVerticalSpeed (jumpHeight);
				
			SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
		}
	}
}
	
	
	void UpdateSmoothedMovementDirection ()
{
	
		
   var grounded = IsGrounded();
	
	

	var v = Input.GetAxisRaw("Vertical");
	var h = Input.GetAxisRaw("Horizontal");

	// Are we moving backwards or looking backwards
	
		if (v < -0.2)
		movingBack = true;
	
		else
		movingBack = false;
	
	var wasMoving = isMoving;
	isMoving = Mathf.Abs (h) > 0.1 || Mathf.Abs (v) > 0.1;
		
	// Target direction relative to the camera
	var targetDirection = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));//h * right + v * forward;
	
	// Grounded controls
	if (grounded)
	{

		if (targetDirection != Vector3.zero)
		{
			// If we are really slow, just snap to the target direction
			if (moveSpeed < walkSpeed * 0.9 && grounded)
			{
				moveDirection = targetDirection.normalized;
			}
			// Otherwise smoothly turn towards it
			else
			{
				moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
				
				moveDirection = moveDirection.normalized;
			}
		}
		
		// Smooth the speed based on the current target direction
		var curSmooth = speedSmoothing * Time.deltaTime;
		
		// Choose target speed
		//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
		var targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);
	
		_characterState = CharacterState.Idle;
		
		// Pick speed modifier
		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
		{
			targetSpeed *= runSpeed;
			_characterState = CharacterState.Running;
		}
		else if (Time.time - trotAfterSeconds > walkTimeStart)
		{
			targetSpeed *= trotSpeed;
			_characterState = CharacterState.Trotting;
		}
		else
		{
			targetSpeed *= walkSpeed;
			_characterState = CharacterState.Walking;
		}
		
		moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
		
		// Reset walk time start when we slow down
		if (moveSpeed < walkSpeed * 0.3)
			walkTimeStart = Time.time;
	}
	// In air controls
	else
	{
		// Lock camera while in air
		//if (jumping)
		//	lockCameraTimer = 0.0;

		if (isMoving)
			inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
	}
	

		
}
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
				
		
				
	}
	
	float CalculateJumpVerticalSpeed (float targetJumpHeight )
{
	// From the jump height and gravity we deduce the upwards speed 
	// for the character to reach at the apex.
	return Mathf.Sqrt(2 * targetJumpHeight * gravity);
}



	
	void Awake(){
	
		enemy=GameObject.Find("Enemy");
		player=this.transform.gameObject;
		
			_animation = GetComponent<Animation>();
	if(!_animation)
		Debug.Log("The character you would like to control doesn't have animations. Moving her might look weird.");

	
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
		
		if (!isControllable)
	{
		// kill all inputs if not controllable.
		Input.ResetInputAxes();
	}

	if (Input.GetButtonDown ("Jump"))
	{
		lastJumpButtonTime = Time.time;
	}
		
		
   UpdateSmoothedMovementDirection();
		
	ApplyGravity();
			
	ApplyJumping ();
		
		
		
		
		
		// Calculate actual motion
	var movement = moveDirection * moveSpeed + new Vector3(0, verticalSpeed, 0) + inAirVelocity;
	movement *= Time.deltaTime;
	
		
		
		
		//if (_characterState==CharacterState.Jumping)
			
		
		if(networkView.isMine)
		{
			// Move the controller
	CharacterController controller = GetComponent<CharacterController>();
	collisionFlags = controller.Move(movement);
			
			
		if(controller.velocity.sqrMagnitude < 0.1) {
				_animation.CrossFade(idleAnimation.name);
				
			
			}else{
				
				if(_characterState == CharacterState.Running) {
					_animation[runAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, runMaxAnimationSpeed);
					_animation.CrossFade(runAnimation.name);	
				}
				else if(_characterState == CharacterState.Trotting) {
					_animation[walkAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, trotMaxAnimationSpeed);
					_animation.CrossFade(walkAnimation.name);	
				}
				else if(_characterState == CharacterState.Walking) {
					_animation[walkAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, walkMaxAnimationSpeed);
					_animation.CrossFade(walkAnimation.name);	
				}
			}	
		}
		
		
	}
}
