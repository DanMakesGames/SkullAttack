using System;
using UnityEngine;
using System.Collections.Generic;

namespace MyItem
{
	
	public abstract class Item
	{
		 
		private Vector3 meshOffset;
		protected PlayerController ownerPlayer;
		public Vector3 MeshOffset
		{
			get {
				return meshOffset;
			}
		}

		private Quaternion meshRotation;
		public Quaternion MeshRotation
		{
			get {
				return meshRotation;
			}
		}
			
		public string prefabName;


		protected bool Initialize(string inPrefabName, Vector3 offset, Quaternion inMeshRotation)
		{
			
			prefabName = inPrefabName;
			meshOffset = offset;
			meshRotation = inMeshRotation;

			return true;
		}


		public abstract void Tick();
		public virtual void GUITick () {}

		public void OnEquip()
		{
		}


		public void OnUnequip ()
		{
		}


		public void Setup(PlayerController inOwner)
		{
			ownerPlayer = inOwner;
		}
	}


	public abstract class MissileLauncher : Item
	{
		
		protected string missileType;
		public MissileLauncher ()
		{

		}

		public void InitLauncher(string inMissileType)
		{
			missileType = inMissileType;	
		}

		public override void Tick()
		{
			if (Input.GetKeyDown (KeyCode.Mouse0)) {
				
				GameObject.Instantiate (Resources.Load(missileType), ownerPlayer.GetHeadLocation(), ownerPlayer.GetLookRotation());
			}
		}
	}

	public class Javlin : MissileLauncher 
	{
		public const int MAX_LOCK_ON = 10;
		public const float LOCK_ON_SPEED = 0.7f;
		public const float FIRE_SPEED = 0.08f;
		private float timeSinceLastLock = 0;
		private float timeSinceLastShot = 0;
		private int missilesPerTarget = 5;
		private int missileShot = 0;
		private Enemy[] found;
		private LinkedList<Enemy> targets;



		enum WEAPON_STATE {START, GATHER_TARGETS, FIRE};
		WEAPON_STATE wState = WEAPON_STATE.START;


		public Javlin ()
		{
			Initialize ("Jav3", new Vector3(0,-2,-3), Quaternion.Euler(0,-90,0));
		}

		public override void GUITick() {
			

			Camera ownerCamera = ownerPlayer.GetComponentInChildren<Camera> ();
			if (targets != null) {
				LinkedListNode<Enemy> current = targets.First;
				int texWidth = 100;
				int texHeight = 100;

				for (int index = 0; index < targets.Count; index++) {
				
					Vector3 posOnScreen = ownerCamera.WorldToScreenPoint (current.Value.transform.position);
					posOnScreen.y = Screen.height - posOnScreen.y;
					Texture tex = (Texture)Resources.Load ("Untitled");
					Rect myRect = new Rect (posOnScreen.x - texWidth / 2, posOnScreen.y - texHeight / 2, 100, 100);

					Graphics.DrawTexture (myRect, tex);
					current = current.Next;
				}
			}

		}

		public override void Tick()
		{	if (wState == WEAPON_STATE.START) {
				if (Input.GetKeyDown (KeyCode.Mouse0)) {
					Debug.Log ("Entering GATHER TARGETS");

					wState = WEAPON_STATE.GATHER_TARGETS;

					Tick ();
					return;
				}
			}
			if (wState == WEAPON_STATE.GATHER_TARGETS) {
				if (!Input.GetKey (KeyCode.Mouse0)) {
					wState = WEAPON_STATE.FIRE;
					found = null;
					timeSinceLastLock = 0;

					Tick ();
					return;
				}

				timeSinceLastLock += Time.deltaTime;

				if (found == null) {
					
					targets = new LinkedList<Enemy> ();

				} 
				found = (Enemy[])UnityEngine.Object.FindObjectsOfType (typeof(Enemy)) ;
				if(timeSinceLastLock > LOCK_ON_SPEED) {
					timeSinceLastLock = 0;

					Camera ownerCamera = ownerPlayer.GetComponentInChildren<Camera> ();

					if (ownerCamera != null && targets.Count < MAX_LOCK_ON) {
						


						for (int index = 0; index < found.Length; index++) {
							Vector3 posOnScreen = ownerCamera.WorldToScreenPoint (found[index].transform.position);
							if (posOnScreen.z >= 0 && posOnScreen.x <= ownerCamera.pixelWidth && posOnScreen.x >= 0 && posOnScreen.y <= ownerCamera.pixelHeight && posOnScreen.y >= 0) {
								if (!targets.Contains (found [index])) {
									targets.AddLast (found [index]);
									Debug.Log ("LOCKED " + targets.Count + " : " + found[index].gameObject.name);
									break;
								}

							}
					
						}
					}
				}
			}
			if (wState == WEAPON_STATE.FIRE) {
				timeSinceLastShot += Time.deltaTime;

				if (timeSinceLastShot >= FIRE_SPEED) {
					timeSinceLastShot = 0;
					if (targets.Count > 0) {
						GameObject missile = (GameObject) GameObject.Instantiate (Resources.Load("HomingMissile"), ownerPlayer.GetHeadLocation(), ownerPlayer.GetLookRotation());
						if(targets.First.Value != null)
							missile.GetComponent<HomingMissile> ().SetTarget (targets.First.Value.gameObject);
						//GameObject.Instantiate (Resources.Load("BasicMissile"), ownerPlayer.GetHeadLocation(), ownerPlayer.GetLookRotation());
						//missile.SetTarget (targets.First.Value.gameObject);
						//targets.RemoveFirst ();
						missileShot++;
						if (missileShot >= missilesPerTarget) {
							missileShot = 0;
							targets.RemoveFirst ();
						}

					} 
					else {
						wState = WEAPON_STATE.START;
					}

				}



			}
		}


	}
	public class RPG : MissileLauncher 
	{
		public RPG ()
		{
			Initialize ("RPG 1", new Vector3(1,0,0), Quaternion.Euler(-90,0,0));
			InitLauncher ("BasicMissile");
		}


	}

	public class LAW : MissileLauncher
	{
		float timeSinceLastShot = 0;
		public LAW ()
		{
			//prefabName = "3ds file (Normal)";
			Initialize ("3ds file (Normal)", new Vector3(1,-1,1), Quaternion.Euler(0,0,0));
			InitLauncher ("BasicMissile");
			//Initialize ("Mesh06", new Vector3(1,0,2), Quaternion.Euler(0,0,0));
		}
		public override void Tick()
		{
			timeSinceLastShot += Time.deltaTime;
			if (Input.GetKey (KeyCode.Mouse0) && timeSinceLastShot > 0.1) {
				timeSinceLastShot = 0;
				GameObject.Instantiate (Resources.Load(missileType), ownerPlayer.GetHeadLocation(), ownerPlayer.GetLookRotation());
			}
		}
	}
}

