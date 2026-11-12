using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerDamageSource : MonoBehaviour
{
    [SerializeField,Tag] private string _playerTag = "Player";
    public DamageInfo BaseDamageInfo;

    public DamageInfo CurrentDamageInfo;
    public UnityEvent OnHit;
    public UnityEvent OnBlockedHit;
    
    private GameObject _parent;
    private List<GameObject> _hitBlackList = new();

    public void Start()
    {
        _parent = transform.parent.gameObject;
        CurrentDamageInfo = BaseDamageInfo;
        _hitBlackList.Add(_parent);
    }

    private void OnDisable()
    {
        _hitBlackList.Clear();
        _hitBlackList.Add(_parent);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_playerTag))
        {
            if (!_hitBlackList.Contains(collision.gameObject))
            {
                if (collision.gameObject.TryGetComponent(out PlayerHealthController playerHealthController))
                {
                    DamageInfo newDamageInfo = new DamageInfo()
                    {
                        Damage = CurrentDamageInfo.Damage,
                        KnockbackStrength =  CurrentDamageInfo.Damage,
                        Knockback =  collision.gameObject.transform.position - _parent.transform.position
                    };
                    newDamageInfo.Knockback += CurrentDamageInfo.Knockback;
                    if (playerHealthController.TakeDamage(newDamageInfo))
                    {
                        OnHit.Invoke();
                    }
                    else
                    {
                        OnBlockedHit.Invoke();
                    }
                    _hitBlackList.Add(collision.gameObject);
                }
            }
        }
    }
}
