using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    [Header("Config")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int maxShield = 100;
    [SerializeField] private float shieldRegen = 25;
    [SerializeField] private float shieldRegenDelay = 1.5f;
    [SerializeField] private float brokenShieldRegenDelay = 3f;

    [Header("Debug")]
    [SerializeField] private int shield;
    [SerializeField] private int health;
    // Use this for initialization
    void OnEnable () {
        health = maxHealth;
        shield = maxShield;
	}

    /// <summary>
    /// Regenerate the shield after a short delay.
    /// </summary>
    /// <param name="broken">Whether or not the shield has been broken</param>
    private IEnumerator RegenShield(bool broken)
    {        
        if (broken)
        {
            yield return new WaitForSeconds(brokenShieldRegenDelay);
        }
        else
        {
            yield return new WaitForSeconds(shieldRegenDelay);
        }

        float shieldActual = shield;
        while(shieldActual < maxShield)
        {
            yield return new WaitForEndOfFrame();
            shieldActual += shieldRegen * Time.deltaTime;
            shield = Mathf.RoundToInt(shieldActual);
        }
        shield = maxShield;
    }
    private Coroutine shieldRegeneration;
    public void TakeDamage(int damage)
    {
        // If shield absorbs all damage, set damage to 0
        if (shield >= damage)
        {

            shield -= damage;
            damage = 0;
        }
        // If shield doesnt not absorb all damage, lessen damage value by value absorbed by shield
        else
        {
            damage -= shield;
            shield = 0;
        }
        // Restart shield regen coroutine
        if (shieldRegeneration != null)
        {
            StopCoroutine(shieldRegeneration);            
        }
        shieldRegeneration = StartCoroutine(RegenShield(!(shield > 0)));


        // Damaging health
        if (health > 0) {
            health -= damage;

            if (health <= 0)
            {
                Death();
            }
        }
    }

    void Death()
    {
        this.gameObject.SetActive(false);
    }
}
