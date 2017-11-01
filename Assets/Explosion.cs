using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
	public float size = 1;
	public float force = 1;
	public float damage = 1;
	// Use this for initialization
	void Start () {
		
		ParticleSystem expPart = GetComponent<ParticleSystem> ();
		/*This is where future explosion size effecting the particle effect is implemented*/
		expPart.Play();

		Collider[] hitObjects = Physics.OverlapSphere (transform.position, size);

		//Loop through and call Hurt on all hit that are of type enemy
		for (int index = 0; index < hitObjects.Length; index ++)
		{
			Enemy enemyComp = hitObjects [index].gameObject.GetComponent<Enemy> ();
			if (enemyComp != null) {
				
				enemyComp.Hurt (damage, (hitObjects[index].gameObject.transform.position - transform.position).normalized * force);
			}
		}

		Destroy(gameObject, expPart.main.duration);
	}
}
