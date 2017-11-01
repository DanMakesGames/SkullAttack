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
	// Use this for initialization
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

	


	}
	void test()
	{
		Rect myRect = new Rect (10,10,500,500);
		Graphics.DrawTexture (myRect,(Texture)Resources.Load ("M72_D"));
	}

	void OnGUI ()
	{
		playerInv.GetEquipedItem ().GUITick ();
	}


	// Update is called once per frame
	void Update () {
		Rect myRect = new Rect (10,10,500,500);
		Graphics.DrawTexture (myRect,(Texture)Resources.Load ("M72_D"));
		ProcessInput ();
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
		/*
		itemFilter.mesh = newItem.ItemMesh;

		itemFilter.transform.localPosition = newItem.MeshOffset;
		itemFilter.transform.localRotation = newItem.MeshRotation;
		*/
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


}
