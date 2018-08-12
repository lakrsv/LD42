// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerInput.cs" author="Lars" company="None">
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
// and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerInput : MonoBehaviour
{
    private const float MaxVelocity = 3f;

    private readonly float _acceleration = 50f;

    private Vector2 _movement = new Vector2();

    private Rigidbody2D _rigidBody;

    [SerializeField]
    private Weapon[] _equippedWeapons;

    private bool _equippedEnemy;

    private Pyre _pyreInReach;

    private Enemy _enemyInReach;

    private FaceCursor _faceCursor;

    [SerializeField]
    private Animator _walkAnimator;

    [SerializeField]
    private Transform _enemyEquipPosition;

    // Update is called once per frame
    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        FireWeapon();
        HandleEnemy();
    }

    private void Move()
    {
        var inputH = Input.GetAxisRaw("Horizontal");
        var inputV = Input.GetAxisRaw("Vertical");

        _movement.Set(inputH, inputV);
        _movement.Normalize();

        _rigidBody.AddForce(_movement * _acceleration);
        _rigidBody.velocity = Vector2.ClampMagnitude(_rigidBody.velocity, MaxVelocity);

        _walkAnimator.SetFloat("WalkSpeed", _rigidBody.velocity.magnitude);
    }

    private void FireWeapon()
    {
        // TODO - Make accessible for controller
        if (Input.GetMouseButton(0)) _equippedWeapons.ForEach(x => x.Fire(_faceCursor.GetLastCursorPosition()));
    }

    private void HandleEnemy()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        if (_enemyInReach == null && !_equippedEnemy) return;

        if (!_equippedEnemy)
        {
            _enemyInReach.Disable();

            _equippedEnemy = true;
            _enemyInReach = null;

            _enemyEquipPosition.gameObject.SetActive(true);

        }
        else
        {
            if (_pyreInReach != null)
            {
                _enemyEquipPosition.gameObject.SetActive(false);
                _equippedEnemy = false;

                _pyreInReach.AddFuel();
                _pyreInReach = null;
            }
        }
    }

    // Use this for initialization
    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _faceCursor = GetComponent<FaceCursor>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_equippedEnemy)
        {
            if (_pyreInReach == null)
            {
                if (other.CompareTag("Pyre"))
                {
                    _pyreInReach = other.GetComponent<Pyre>();
                }
            }
        }
        else
        {
            if (_enemyInReach == null)
            {
                if (other.CompareTag("Enemy"))
                {
                    _enemyInReach = other.transform.GetComponentInParent<Enemy>();
                }
            }
        }
    }
}