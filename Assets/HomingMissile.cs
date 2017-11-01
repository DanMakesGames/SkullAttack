using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour {
	private const float MAX_HEADING_CORRECT = 1.0f;
	private GameObject target;
	public float speed = 1;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, 10);
	}
	 public void SetTarget(GameObject inTarget) {
		target = inTarget;
	}
	
	// Update is called once per frame
	void Update () {
		if (target == null) {
			Destroy (gameObject);
			return;
		}

		// Set Rotation
		//Vector3 rocketHeading = transform.rotation * Vector3.forward;
		Vector3 enemyRot= Quaternion.FromToRotation (Vector3.forward, target.transform.position - transform.position).eulerAngles;
		Quaternion enemyHeading = Quaternion.Euler (new Vector3(enemyRot.x, enemyRot.y, 0));

		Quaternion rocketHeading = Quaternion.Euler (new Vector3 (transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0));
		//Quaternion rocketHeading = transform.rotation;
		//rocketHeading = Quaternion.RotateTowards (rocketHeading, enemyHeading, MAX_HEADING_CORRECT);
		rocketHeading = Quaternion.Slerp (rocketHeading,enemyHeading,0.03f);

		transform.rotation = rocketHeading;

		transform.position += transform.rotation * Vector3.forward * speed * Time.deltaTime;
	}


	void OnTriggerEnter(Collider hit) {

		Enemy hitEnemy = hit.GetComponent<Enemy> ();
		if (hitEnemy != null) {
			Explode ();
			return;
		}

		if (hit.gameObject.tag != "Player") {
			Debug.Log (hit.gameObject.name);
			Explode ();
		} 

	}

	void Explode()
	{
		GameObject exp = (GameObject) Instantiate (Resources.Load("Explosion"),transform.position,transform.rotation);
		Explosion expComp = exp.GetComponent<Explosion> ();
		expComp.damage = 5;
		expComp.size = 1;
		expComp.force = 10;
		Destroy (gameObject);
		// Create Explosion Object then destroy self.
	}
}
