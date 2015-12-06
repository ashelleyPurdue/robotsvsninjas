using UnityEngine;
using System.Collections;

public class LifeOrbHealthPoints : AbstractHealthPoints
{
	public int maxLifeOrbs;
	public float maxOrbHealth;		//How much damage a single life orb can take before shattering.
	public float lifeOrbRegenRate;	//How fast the current lifeOrb heals itself
	public float lifeOrbRegenDelay;	//How long you have to wait without taking damage before the current lifeOrb will start regenerating.

	public float CurrentOrbHealth {get {return currentOrbHealth;}}
	public int CurrentLifeOrbs{get {return currentLifeOrbs;}}

	protected float currentOrbHealth;	//When this reaches zero, the current orb shatters
	protected int currentLifeOrbs;		//When this reaches zero, death occurs

	protected float regenDelayTimer = 0f;


	//Events

	void Awake()
	{
		//Start out at full health
		currentLifeOrbs = maxLifeOrbs;
		currentOrbHealth = maxOrbHealth;
	}

	void FixedUpdate()
	{
		//Heal the current orb if we're past the delay.  Else, increment the timer.
		if (regenDelayTimer >= lifeOrbRegenDelay)
		{
			currentOrbHealth += lifeOrbRegenRate * Time.deltaTime;

			if (currentOrbHealth > maxOrbHealth)
			{
				currentOrbHealth = maxOrbHealth;
			}
		}
		else
		{
			regenDelayTimer += Time.deltaTime;
		}
	}


	//Interface

	public override void DealDamage(float amount)
	{
		//Damage the current orb.
		currentOrbHealth -= amount;

		//Reset the regen timer
		regenDelayTimer = 0f;

		//If the orb is out of health, break it.
		if (currentOrbHealth <= 0)
		{
			currentOrbHealth = maxOrbHealth;
			currentLifeOrbs--;
		}

		//If there are no orbs left, die.
		if (currentLifeOrbs <= 0)
		{
			Die();
		}
	}
}
