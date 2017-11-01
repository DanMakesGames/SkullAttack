using System;
using System.Collections.Generic;
using MyItem;

namespace MyInventory
{
	public class Inventory
	{
		private LinkedList<Item> storage;
		private LinkedListNode<Item> equipped;
		private PlayerController owner;



		public Inventory (PlayerController inOwner)
		{
			storage = new LinkedList<Item>();
			owner = inOwner;
		}

		public void Give(string itemCode)
		{
			switch (itemCode) 
			{

			case "javlin":
				storage.AddLast (new Javlin ()).Value.Setup(owner);
				break;

			case "RPG":
				storage.AddLast (new RPG ()).Value.Setup(owner);
				break;

			case "LAW":
				storage.AddLast (new LAW ()).Value.Setup(owner);
				break;
			
			}
			if (equipped == null && storage.Count > 0) 
			{
				equipped = storage.Last;
			}

		}

		/**
		 * Takes currently equiped item out of the inventory 
		 */
		public void Take()
		{	
			if (equipped != null) 
			{
				LinkedListNode<Item> ItemToRemove = equipped;
				if (storage.Count >= 2)
					equipped = equipped.Next;
				else
					equipped = null;
				
				storage.Remove (ItemToRemove);
			}
		}

		public void EquipNext() 
		{
			if (equipped != null) 
			{
				LinkedListNode<Item> nextNode = equipped.Next;
				if (nextNode == null) 
				{
					equipped = storage.First;
				} 
				else 
				{
					equipped = nextNode;
				}

			}
		}

		public void EquipPrevious()
		{
			if (equipped != null) {
				
				LinkedListNode<Item> previousNode = equipped.Previous;
				if (previousNode == null) 
				{
					equipped = storage.Last;
				} 
				else 
				{
					equipped = previousNode;
				}
			}
		}

		public Item GetEquipedItem()
		{
			if (equipped == null)
				return null;
			
			return equipped.Value;
		}

		public int GetCount()
		{
			return storage.Count;
		}
	}
}

