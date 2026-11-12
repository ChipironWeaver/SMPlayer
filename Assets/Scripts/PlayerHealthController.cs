using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerHealthController : MonoBehaviour
{
    
    [SerializeField] private float _maxHealth;
    [SerializeField,ReadOnly] private float _currentHealth;
    
    public float CurrentHealth => _currentHealth;
    public UnityEvent<DamageInfo> OnDamage = new();
    public UnityEvent OnDeath = new();
    [FormerlySerializedAs("CanTakeDamage")] public bool Invulnerable;
    [FormerlySerializedAs("IsShielded")] public bool IsBlocking;

    public void Start()
    {
        _currentHealth = _maxHealth;
    }

    public bool TakeDamage(DamageInfo damageInfo)
    {
        if (IsBlocking)
        {
            return false;
        }

        if (Invulnerable) return true;
        
        _currentHealth -= damageInfo.Damage;
        
        
        if (_currentHealth <= 0)
        {
            Death();
        }
        else
        {
            OnDamage?.Invoke(damageInfo);
        }
        return true;
    }
    
    public void Death()
    {
        OnDeath?.Invoke();
        Invulnerable = true;
    }
}