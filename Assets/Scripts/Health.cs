using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    protected int MaxHealth;
    [SerializeField]
    protected int CurrentHealth;
    [SerializeField]
    private bool destroy;
    private int startingHealth;
    public UnityEvent OnDeath;
    // Start is called before the first frame update
    void Start()
    {
        startingHealth = MaxHealth;
        CurrentHealth = MaxHealth;        
    }
    public void AdjustHealthRange(float factor)
    {
        MaxHealth = Mathf.CeilToInt(MaxHealth * factor);
        if (startingHealth > MaxHealth)
            startingHealth = MaxHealth;
    }
    public virtual void Damage(int amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
        {
            OnDeath.Invoke();
            if (destroy)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);
        }
    }
    public void Respawn()
    {
        CurrentHealth = Random.Range(startingHealth, MaxHealth);
    }
}
