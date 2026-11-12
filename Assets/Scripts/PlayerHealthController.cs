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
    public bool IsShielded;

    public void Start()
    {
        _currentHealth = _maxHealth;
    }

    public bool TakeDamage(DamageInfo damageInfo)
    {
        if (IsShielded)
        {
            return false;
        }

        if (Invulnerable) return true;
        
        _currentHealth -= damageInfo.Damage;
        OnDamage?.Invoke(damageInfo);
        
        if (_currentHealth <= 0)
        {
            Death();
        }
        return true;
    }
    
    public void Death()
    {
        OnDeath?.Invoke();
        Invulnerable = true;
    }
}