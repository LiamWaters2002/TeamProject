using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;

    /// <summary>
    /// Deducts the health from the player.
    /// </summary>
    /// <param name="damage">Amount of damage</param>
    public void takeDamage(float damage)
    {
        healthBar.fillAmount -= damage;
    }
}
