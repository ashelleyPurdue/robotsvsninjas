using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(RectTransform))]
public class PlayerHealthDrawer : MonoBehaviour
{
	public const float ORB_SEPARATION = 1f;
	
	private LifeOrbHealthPoints playerHealth;
	private Stack<Image> orbs = new Stack<Image>();
	
	
	//Events
	void Start()
	{
		//Get the healthPoints
		playerHealth = PlayerBehaviour.instance.GetComponent<LifeOrbHealthPoints>();
	}
	
	void Update()
	{
		//Make sure the number of orbs equals the player's current orb count.
		while (orbs.Count < playerHealth.CurrentLifeOrbs)
		{
			AddOrb();
		}
		while (orbs.Count > playerHealth.CurrentLifeOrbs)
		{
			RemoveOrb();
		}
		
		//Update the current orb's health
		Image currentOrb = orbs.Peek();
		
		if (currentOrb == null)
		{
			return;
		}
		
		ChangeOrbHealth(currentOrb, playerHealth.CurrentOrbHealth / playerHealth.maxOrbHealth);
	}
	
	//Misc methods
	private void AddOrb()
	{
		//Adds a new orb.
		
		Image oldOrb = orbs.Peek();
		
		//Restore the old orb to full health.
		if (oldOrb != null)
		{
			ChangeOrbHealth(oldOrb, 1);
		}
		
		//Get the new position
		Vector3 orbPos = Vector3.zero;
		
		if (oldOrb != null)
		{
			orbPos = oldOrb.rectTransform.localPosition;
		}
		
		orbPos.x += ORB_SEPARATION;
		
		//Create the new orb
		GameObject newOrb = Instantiate((GameObject)Resources.Load("lifeOrb_image_prefab"));
		RectTransform newOrbTrans = newOrb.GetComponent<RectTransform>();
		newOrbTrans.localPosition = orbPos;
		newOrbTrans.SetParent(transform);
		
		//Push the orb onto the stack.
		orbs.Push(newOrb.GetComponent<Image>());
	}
	
	private void RemoveOrb()
	{
		//Removes the current orb.
		
		Image currentOrb = orbs.Pop();
		
		if (currentOrb != null)
		{
			GameObject.Destroy(currentOrb.gameObject);
		}
	}
	
	private void ChangeOrbHealth(Image orb, float percentLeft)
	{
		//Updates the specified orb image to have the given percent of its health left.
		//TODO: Do something.
	}
}
