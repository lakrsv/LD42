using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilities.ObjectPool;

[RequireComponent(typeof(Animator))]
public class BloodSpray : MonoBehaviour, IPoolable
{

    private Animator _bloodAnimator;

    private Rigidbody2D _rigidBody;

    public bool IsEnabled => gameObject.activeInHierarchy;

    public void Enable()
    {
        if (_bloodAnimator == null) _bloodAnimator = GetComponent<Animator>();
        if (_rigidBody == null) _rigidBody = GetComponent<Rigidbody2D>();


        gameObject.SetActive(true);
    }

    public void Disable()
    {
        if (_bloodAnimator == null) _bloodAnimator = GetComponent<Animator>();
        if (_rigidBody == null) _rigidBody = GetComponent<Rigidbody2D>();

        gameObject.SetActive(false);
    }

    public void AddForce(Vector2 force)
    {
        _rigidBody.AddForce(force, ForceMode2D.Impulse);
    }
}
