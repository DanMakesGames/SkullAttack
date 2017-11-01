using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMissileMover : MonoBehaviour {
	public float speed = 0;
	// Use this for initialization
	void Start () {
		//GetComponent<Rigidbody>().i
		Destroy (gameObject, 10);
	}
	
	// Update is called once per frame
	void Update () {
		

		Vector3 eulerAngles = Mathf.Deg2Rad * transform.rotation.eulerAngles;
		Vector3 start = new Vector3 (0, 0, 1);
		transform.position += transform.rotation * start * Time.deltaTime * speed;
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
		expComp.damage = 10;
		expComp.size = 12;
		expComp.force = 20;
		Destroy (gameObject);
		// Create Explosion Object then destroy self.
	}
}
