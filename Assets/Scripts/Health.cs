using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int MaxHealth;
    [SerializeField]
    private int CurrentHealth;
    public UnityEvent OnDeath;
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;        
    }

    public void Damage(int amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
        {
            OnDeath.Invoke();
            Destroy(gameObject);
        }
    }
}
