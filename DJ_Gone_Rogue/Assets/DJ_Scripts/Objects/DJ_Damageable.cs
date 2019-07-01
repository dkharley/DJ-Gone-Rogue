using UnityEngine;
using System.Collections;

/// <summary>
/// DJ_Damageable. A component that contains the variables
/// necessary for an entity to take damage.
/// 
/// @author Donnell Lu 1/16/2014
/// </summary>

public class DJ_Damageable: MonoBehaviour
{
	
	public virtual void Awake() {
		damageOfLastHit = 0;
		isAlive = true;
		//invulnerable = false; 
		hurtRecently = false;
        deathBy = DJ_Death.NONE;
		initHealth();
	}
	
	public virtual void Update(){
		if (!invulnerable && !hurtRecently) { 
			if (healedAmount > 0) {
				hp += healedAmount;

				// Can't over heal maximum hp
				if (hp > maxHp) {
						hp = maxHp;
				}
					healedAmount = 0;
			}

			if (damageOfLastHit > 0) {
				hp -= damageOfLastHit;
				damageOfLastHit = 0;
			}
		}

		if(hp <= 0.0f) 
			isAlive = false;

	}

	//initializes the health
	void initHealth(){
		hp = maxHp;

		if(hp <= 0)
			throw new UnityException("Someone pucked up and didn't assign a maxhp value in the unity inspector for this prefab.");
	}

	//Deals damage to the object
	public void DoDamage(float currentDamage){
		damageOfLastHit = currentDamage;
	}

	/// <summary>
	/// Component variables for dealing damage
	/// to other entities
	/// </summary>

	// Is the entity alive
	public bool isAlive;

	// hp of the entity
	public float hp;

	// max hp of the entity
	public float maxHp;

	// amount of last damage taken 
	public float damageOfLastHit;

	// amount the entity should be healed
	public float healedAmount;

	// allows the entity to not get hurt.
	public bool invulnerable;

	// allows a little bit of leeway.
	// used for double breaking tiles. land = breaks, beat then breaks.
	public bool hurtRecently;

    public DJ_Death deathBy;
}

