using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField]
    private Image[] _hearts;

    private int _currentHealth = 8;

    public int CurrentHealth => _currentHealth;

    public void RemoveHealth()
    {
        _currentHealth--;
        _currentHealth = Mathf.Max(0, _currentHealth);
        _hearts[_currentHealth].gameObject.SetActive(false);
    }

    public void AddHealth()
    {
        _currentHealth++;
        _currentHealth = Mathf.Min(_hearts.Length, _currentHealth);
        _hearts[_currentHealth - 1].gameObject.SetActive(true);
    }
}
