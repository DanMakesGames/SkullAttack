using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {
	enum MOVEMENT_STATE {
		WALKING,
		FALLING
	}
	private static float MIN_GROUND_DIST = 0.01f;

	public float maxSpeed = 1;
	public float mouseSensitivity = 1;
	[HideInInspector]
	public bool bCanRotate = true;
	private MOVEMENT_STATE moveState = MOVEMENT_STATE.FALLING;
	private CapsuleCollider collisionShape;
	public Camera playerCamera;
	private Vector3 moveInput = Vector3.zero;
	private Vector3 velocity = Vector3.zero;
	private Quaternion rotationDelta;




	// Use this for initialization
	void Start () {
		collisionShape = GetComponent<CapsuleCollider> ();
	}


	// Update is called once per frame
	void Update () {
		ProcessInput ();


		if (moveState == MOVEMENT_STATE.FALLING) {
			RaycastHit hit;

			bool bDidHit = Physics.CapsuleCast (new Vector3(0, collisionShape.height / 2, 0) + transform.position, 
						   new Vector3(0, -(collisionShape.height / 2), 0) + transform.position, collisionShape.radius, 
						   new Vector3(0,-1,0), out hit);

			if (bDidHit) {
				transform.position = hit.point + new Vector3(0, collisionShape.height / 2.0f + MIN_GROUND_DIST, 0);
				moveState = MOVEMENT_STATE.WALKING;
			}
		}
		if (moveState == MOVEMENT_STATE.WALKING) 
		{
			Vector3 currentRot = Mathf.Deg2Rad * transform.rotation.eulerAngles;
			Vector3 rotatedInput = new Vector3 (moveInput.x * Mathf.Cos (currentRot.y) + moveInput.z * Mathf.Sin (currentRot.y), 0, 
				                      		 	moveInput.x * -Mathf.Sin (currentRot.y) + moveInput.z * Mathf.Cos (currentRot.y));
			velocity += rotatedInput * maxSpeed;
			//velocity += moveInput * maxSpeed;
			transform.position += velocity * Time.deltaTime;

			//Debug.Log ("Move: " + currentRot.y );
			velocity = Vector3.zero;
		}

		moveInput = Vector3.zero;



	}


	private void ProcessInput() 
	{
		moveInput += new Vector3(0,0,Input.GetAxis ("Vertical"));
		moveInput += new Vector3(Input.GetAxis ("Horizontal"), 0, 0);
		moveInput.Normalize ();

		float bodyPitch = Input.GetAxis ("Mouse X") ;
		float cameraRoll =  - Input.GetAxis ("Mouse Y");
		/*
		Vector3 heading = new Vector3 (Mathf.Sin(rollPitch), 0, Mathf.Cos(rollPitch));
		rotationDelta = new Quaternion (heading.x * Mathf.Sin(0),
			heading.y * Mathf.Sin(0),
			heading.z * Mathf.Sin(0),
			Mathf.Cos(0));

		transform.rotation = transform.rotation * rotationDelta;
		*/
		transform.Rotate (new Vector3(0,bodyPitch,0));
		playerCamera.transform.Rotate (new Vector3(cameraRoll, 0, 0));

	}
}
