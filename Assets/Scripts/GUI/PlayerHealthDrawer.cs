using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(RectTransform))]
public class PlayerHealthDrawer : MonoBehaviour
{
	public const float ORB_SEPARATION = 10f;
	
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
		
		//Don't go on if we're out of health
		if (orbs.Count <= 0)
		{
			return;
		}
		
		//Update the current orb's health
		Image currentOrb = orbs.Peek();
		
		ChangeOrbHealth(currentOrb, playerHealth.CurrentOrbHealth / playerHealth.maxOrbHealth);
	}
	
	//Misc methods
	private void AddOrb()
	{
		//Adds a new orb.
		
		Image oldOrb;
		
		if (orbs.Count <= 0)
		{
			oldOrb = null;
		}
		else
		{
		 	oldOrb = orbs.Peek();
		}
		
		//Restore the old orb to full health.
		if (oldOrb != null)
		{
			ChangeOrbHealth(oldOrb, 1);
		}
		
		//Create the new orb
		GameObject newOrbObj = Instantiate((GameObject)Resources.Load("lifeOrb_image_prefab"));
		RectTransform newOrbTrans = newOrbObj.GetComponent<RectTransform>();
		Image newOrb = newOrbObj.GetComponent<Image>();
		
		//Push the orb onto the stack.
		orbs.Push(newOrb);
		
		//Set the new orb's parent.
		newOrbTrans.SetParent(transform);
		
		//Get the new position
		Vector2 orbPos = Vector2.zero;
		
		if (oldOrb != null)
		{
			orbPos = oldOrb.rectTransform.anchoredPosition;
		}
		
		//Space the orb out.
		orbPos.x += newOrbTrans.rect.width * newOrb.canvas.scaleFactor / 2;
		
		newOrbTrans.anchoredPosition = orbPos;
	}
	
	private void RemoveOrb()
	{
		//Removes the current orb.
		
		if (orbs.Count <= 0)
		{
			return;
		}
		
		Image currentOrb = orbs.Pop();
		
		if (currentOrb != null)
		{
			GameObject.Destroy(currentOrb.gameObject);
		}
	}
	
	private void ChangeOrbHealth(Image orb, float percentLeft)
	{
		//Updates the specified orb image to have the given percent of its health left.
		orb.rectTransform.localScale = new Vector2(percentLeft, percentLeft);
	}
}
