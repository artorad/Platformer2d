using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private Image[] _hearts;

    private void Start()
    {
        _player.Health.OnTakeDamage += UpdateHearts;
        UpdateHearts();
    }

    private void UpdateHearts()
    {
        int hp = _player.Health.CurrentHP;
        for (int i = 0; i < _hearts.Length; i++)
        {
            _hearts[i].enabled = i < hp;
        }
    }

    private void OnDestroy()
    {
        if (_player != null) _player.Health.OnTakeDamage -= UpdateHearts;
    }
}