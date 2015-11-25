using UnityEngine;
using System.Collections;

public class LifeOrbHealthPoints : AbstractHealthPoints
{
	public int maxLifeOrbs;
	public float maxOrbHealth;		//How much damage a single life orb can take before shattering.
	public float lifeOrbRegenRate;	//How fast the current lifeOrb heals itself
	public float lifeOrbRegenDelay;	//How long you have to wait without taking damage before the current lifeOrb will start regenerating.

	protected float currentOrbHealth;	//When this reaches zero, the current orb shatters
	protected int currentLifeOrbs;		//When this reaches zero, death occurs

	protected float timer = 0f;


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
		if (timer >= lifeOrbRegenDelay)
		{
			currentOrbHealth += lifeOrbRegenRate * Time.deltaTime;

			if (currentOrbHealth > maxOrbHealth)
			{
				currentOrbHealth = maxOrbHealth;
			}
		}
		else
		{
			timer += Time.deltaTime;
		}
	}


	//Interface

	public override void DealDamage(float amount)
	{
		//Damage the current orb.
		currentOrbHealth -= amount;

		//Reset the timer
		timer = 0f;

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
