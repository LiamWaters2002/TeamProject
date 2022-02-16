using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public void takeDamage(float damage)
    {
        healthBar.fillAmount -= damage;
    }
}
