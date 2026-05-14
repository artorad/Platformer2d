using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    [SerializeField] private int _damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.TryGetComponent(out PlayerController player))
        {
            player.TakeDamage(_damageAmount, transform.position);
        }
    }
}

