using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyInventory;
using MyItem;

public class PlayerController : MonoBehaviour {
	private MovementController moveComp;
	private Inventory playerInv;
	public MeshFilter itemFilter;
	public GameObject currentItem;
	private Camera playerCamera;
	public Texture reticleTex;

	// If this is true the player takes no damage.
	private bool bInvincible = false;
	[SerializeField]
	private float hitITime = 4;

	// Player Health
	private float health;
	public float Health {
		get {
			return health;
		}
		protected set {
			health = value;
		}
	}
	// Maximum Health the player can have. Can public so it can be set from the editor.
	public float maxHealth = 100;

	void Awake() {
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Start () {
		moveComp = GetComponent<MovementController> ();
		playerCamera = GetComponentInChildren<Camera> ();
		playerInv = new Inventory (this);
		GivePlayerItem ("RPG");
		GivePlayerItem ("LAW");
		GivePlayerItem ("javlin");
		health = maxHealth;
	}

	private Texture GetNumberTex(int num) {
		switch (num) {
		case 0:
			return (Texture)Resources.Load ("Alphabet/0");
		case 1:
			return (Texture)Resources.Load ("Alphabet/1");
		case 2:
			return (Texture)Resources.Load ("Alphabet/2");
		case 3:
			return (Texture)Resources.Load ("Alphabet/3");
		case 4:
			return (Texture)Resources.Load ("Alphabet/4");
		case 5:
			return (Texture)Resources.Load ("Alphabet/5");
		case 6:
			return (Texture)Resources.Load ("Alphabet/6");
		case 7:
			return (Texture)Resources.Load ("Alphabet/7");
		case 8:
			return (Texture)Resources.Load ("Alphabet/8");
		case 9:
			return (Texture)Resources.Load ("Alphabet/9");
		}
		return null;
	}

	void OnGUI () {
		// Allow the current equpied item to draw to the GUI.
		playerInv.GetEquipedItem ().GUITick ();

		// Draw the reticle. Reticle texture is controlled by the currently equipped weapon.
		if (reticleTex != null) {

			Rect reticleRect = new Rect (Screen.width / 2 - reticleTex.width / 2, (Screen.height / 2) - reticleTex.height / 2, reticleTex.width, reticleTex.height);
			GUI.DrawTexture (reticleRect, reticleTex);

			//Draw Health

				int healthHolder = (int)health;
				const int numberDim = 50;
				const int numberSpace = 5;
				int startX = Screen.width;
				int startY = Screen.height - numberDim - numberSpace;

			if ((int)health > 0) {
				while (healthHolder != 0) {
					int num = healthHolder % 10;

					startX -= numberDim + numberSpace;

					Rect numRect = new Rect (startX, startY, numberDim, numberDim);

					GUI.DrawTexture (numRect, GetNumberTex (num));

					healthHolder = (healthHolder - num) / 10;
				}

				Texture healthTex = (Texture)Resources.Load ("Health");
				startX -= healthTex.width + numberSpace;
				Rect healthRect = new Rect (startX, startY, healthTex.width, healthTex.height);
				GUI.DrawTexture (healthRect, healthTex);

			} 
			else {
				startX = Screen.width - numberDim - numberSpace;
				Rect numRect = new Rect (startX, startY, numberDim, numberDim);
				GUI.DrawTexture (numRect, GetNumberTex (0));
				Texture healthTex = (Texture)Resources.Load ("Health");
				startX -= healthTex.width + numberSpace;
				Rect healthRect = new Rect (startX, startY, healthTex.width, healthTex.height);
				GUI.DrawTexture (healthRect, healthTex);
			}

		}

	}
		

	void Update () {
		// if the player health is less than 0 kill them.
		if (health <= 0) {
			Application.Quit ();
			//Destroy (gameObject);

		}

		// Process the player input.
		ProcessInput ();

		// Tick the currently equipped item.
		playerInv.GetEquipedItem ().Tick ();
	}


	private void ProcessInput()
	{
		if (Input.GetKeyDown (KeyCode.E)) 
		{
			Debug.Log ("E-Down");
			NextItem ();
		}
		if (Input.GetKeyDown (KeyCode.Q)) {
			PreviousItem ();
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}


	public void GivePlayerItem(string itemCode) 
	{
		Item oldItem = playerInv.GetEquipedItem();
		playerInv.Give (itemCode);
		if( oldItem == null && playerInv.GetCount() > 0)
		{
			OnNewItemEquipped ();
		}

	}


	private void NextItem()
	{
		playerInv.EquipNext ();
		OnNewItemEquipped ();
	}

	private void PreviousItem()
	{
		playerInv.EquipPrevious ();
		OnNewItemEquipped ();
	}

	private void OnNewItemEquipped()
	{
		Unequip ();
		Equip (playerInv.GetEquipedItem());
	}


	private void Unequip()
	{
		
		Destroy(currentItem);
		currentItem = null;
	}


	private void Equip(Item newItem)
	{
		if (newItem == null)
			return;
		
		currentItem = (GameObject) Instantiate (Resources.Load(newItem.prefabName),playerCamera.transform);
		currentItem.transform.localPosition = newItem.MeshOffset;
		currentItem.transform.localRotation = newItem.MeshRotation;

	}


	public Quaternion GetLookRotation()
	{
		return transform.rotation *  playerCamera.transform.localRotation;
	}


	public Vector3 GetHeadLocation()
	{
		return playerCamera.transform.position;
	}

	// HUrt the player and make them take damage.
	public void Hurt(float damage) {
		if (bInvincible)
			return;
		Debug.Log ("PLayer is HUrt");
		health -= damage;
		StartCoroutine ("StartHurtState");

	}

	IEnumerator StartHurtState() {
		bInvincible = true;
		yield return new WaitForSeconds(hitITime);
		bInvincible = false;
	}

}
