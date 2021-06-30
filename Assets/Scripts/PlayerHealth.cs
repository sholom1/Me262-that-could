using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private Slider slider;
    public override void Damage(int amount)
    {
        base.Damage(amount);
        healthText.text = $"Health: {CurrentHealth}/{MaxHealth}";
        slider.value = (float)CurrentHealth / MaxHealth;
    }
}
