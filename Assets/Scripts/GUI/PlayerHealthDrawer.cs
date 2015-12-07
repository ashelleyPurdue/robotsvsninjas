using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(RectTransform))]
public class PlayerHealthDrawer : MonoBehaviour
{
	public const float ORB_SEPARATION = 10f;
	
	private LifeOrbHealthPoints playerHealth;
	private Stack<LifeOrbGUI> orbs = new Stack<LifeOrbGUI>();
	
	
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
		LifeOrbGUI currentOrb = orbs.Peek();

        currentOrb.ChangeHealth(playerHealth.CurrentOrbHealth / playerHealth.maxOrbHealth);
	}
	
	//Misc methods
	private void AddOrb()
	{
		//Adds a new orb.
		
		LifeOrbGUI oldOrb;
		
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
            oldOrb.ChangeHealth(1);
		}
		
		//Create the new orb
		GameObject newOrbObj = Instantiate((GameObject)Resources.Load("lifeOrb_image_prefab"));
		RectTransform newOrbTrans = newOrbObj.GetComponent<RectTransform>();
		LifeOrbGUI newOrb = newOrbObj.GetComponent<LifeOrbGUI>();
		
		//Push the orb onto the stack.
		orbs.Push(newOrb);
		
		//Set the new orb's parent.
		newOrbTrans.SetParent(transform);
		
		//Get the new position
		Vector2 orbPos = Vector2.zero;
		
		if (oldOrb != null)
		{
			orbPos = oldOrb.GetComponent<RectTransform>().anchoredPosition;
		}

        //Space the orb out.
        orbPos.x += newOrbTrans.rect.width; //* newOrb.background.canvas.scaleFactor / 2;
		
		newOrbTrans.anchoredPosition = orbPos;
	}
	
	private void RemoveOrb()
	{
		//Removes the current orb.
		
		if (orbs.Count <= 0)
		{
			return;
		}
		
		LifeOrbGUI currentOrb = orbs.Pop();
		
		if (currentOrb != null)
		{
			GameObject.Destroy(currentOrb.gameObject);
		}
	}
}
