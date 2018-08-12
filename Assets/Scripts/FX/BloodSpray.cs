using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;

using Utilities.ObjectPool;

[RequireComponent(typeof(Animator))]
public class BloodSpray : MonoBehaviour, IPoolable
{

    private Animator _bloodAnimator;

    private Rigidbody2D _rigidBody;

    private SpriteRenderer _renderer;

    public bool IsEnabled => gameObject.activeInHierarchy;

    public void Enable()
    {
        if (_bloodAnimator == null) _bloodAnimator = GetComponent<Animator>();
        if (_rigidBody == null) _rigidBody = GetComponent<Rigidbody2D>();
        if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();

        _renderer.color = Color.white;
        gameObject.SetActive(true);

        _renderer.DOColor(Color.clear, 10f).OnComplete(Disable);
    }

    public void Disable()
    {
        if (_bloodAnimator == null) _bloodAnimator = GetComponent<Animator>();
        if (_rigidBody == null) _rigidBody = GetComponent<Rigidbody2D>();
        if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();

        _renderer.color = Color.white;
        gameObject.SetActive(false);
    }

    public void AddForce(Vector2 force)
    {
        _rigidBody.AddForce(force, ForceMode2D.Impulse);
    }
}
