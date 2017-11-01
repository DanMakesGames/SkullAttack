using System;
using UnityEngine;


public class AngrySkull : Enemy
{
	private GameObject targetPlayer;
	public float maxMoveSpeed = 1;
	public float acceleration = 1;
	protected override void Initialize()
	{
		Health = 100;
		targetPlayer = GameObject.FindGameObjectWithTag ("Player");
		
	}


	protected override void Tick()
	{
		
		if (targetPlayer != null) {

			velocity += (targetPlayer.transform.position - transform.position).normalized * acceleration;
			velocity = velocity.normalized * Mathf.Min (velocity.magnitude, maxMoveSpeed);
			transform.position += velocity * Time.deltaTime;
			Vector3 rotateTo = Quaternion.FromToRotation (Vector3.forward, targetPlayer.transform.position - transform.position).eulerAngles;
			transform.rotation = Quaternion.Euler (new Vector3(rotateTo.x, rotateTo.y, 0));

		}
	}

}


