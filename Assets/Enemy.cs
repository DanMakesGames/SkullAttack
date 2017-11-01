using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
	private float health;
	protected Vector3 velocity;
	public float Health
	{
		get {
			return health;
		}
		protected set {
			health = value;
		}
	}
	// Use this for initialization
	void Start () {
		Initialize ();
	}
	
	// Update is called once per frame
	void Update () {
		Tick ();
		if (Health <= 0) 
		{
			OnDeath ();
			Destroy (gameObject);
		}
	}

	protected abstract void Tick ();
	protected abstract void Initialize ();
	protected void OnDeath () {
	}
	public void Hurt(float damage, Vector3 impact)
	{
		Debug.Log ("Got Hurt. Health: " + Health);
		Health -= damage;
		velocity += impact;
	}
}
