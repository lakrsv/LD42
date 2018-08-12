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

    private const float NoLightDieTimer = 3.0f;

    private readonly float _acceleration = 50f;

    [SerializeField]
    private Animator _dieAnimator;

    private bool _disableInput;

    [SerializeField]
    private Transform _enemyEquipPosition;

    private Enemy _enemyInReach;

    private bool _equippedEnemy;

    [SerializeField]
    private Weapon[] _equippedWeapons;

    private FaceCursor _faceCursor;

    [SerializeField]
    private HealthDisplay _healthDisplay;

    private bool _inLight = true;

    private float _lightExitTime;

    private Vector2 _movement = new Vector2();

    [SerializeField]
    private GameObject _playerHat;

    [SerializeField]
    private Pyre _pyre;

    private Pyre _pyreInReach;

    private Rigidbody2D _rigidBody;

    [SerializeField]
    private GameObject _superEffects;

    [SerializeField]
    private Animator _walkAnimator;

    public void TakeDamage(float amount)
    {
        _healthDisplay.RemoveHealth();
    }

    private void Die()
    {
        _disableInput = true;
        _dieAnimator.gameObject.SetActive(true);
        _walkAnimator.gameObject.SetActive(false);
        _playerHat.gameObject.SetActive(false);
        _faceCursor.enabled = false;
        _equippedWeapons.ForEach(x => x.gameObject.SetActive(false));
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void DisableSuperMode()
    {
        _superEffects.gameObject.SetActive(false);

        GameController.Instance.SuperCounter = 10;
        _equippedWeapons[0].FireCooldown = 1.0f;
        _equippedWeapons[1].FireCooldown = 1.0f;

        _equippedWeapons[1].gameObject.SetActive(false);
    }

    private void EnableSuperMode()
    {
        Debug.Log("SUPER SAYAN");

        _equippedWeapons[0].FireCooldown = 0.5f;
        _equippedWeapons[1].FireCooldown = 0.75f;
        _equippedWeapons[1].gameObject.SetActive(true);

        _healthDisplay.AddHealth();
        _healthDisplay.AddHealth();

        _pyre.AddFuel();

        var enemies = ActorChoreographer.Instance.Enemies.ToArray();
        enemies.ForEach(
            x =>
                {
                    if (!x.enabled) return;
                    var impactDir = transform.position - x.transform.position;
                    x.DoImpact(impactDir.normalized * 10f);
                    x.TakeDamage(RandomProvider.Instance.Random.Next(0, 2));
                });

        _superEffects.gameObject.SetActive(true);

        Invoke(nameof(DisableSuperMode), 8.5f);
    }

    private void FireWeapon()
    {
        // TODO - Make accessible for controller
        if (Input.GetMouseButton(0))
            _equippedWeapons.ForEach(
                x =>
                    {
                        if (x.gameObject.activeInHierarchy)
                        {
                            x.Fire(_faceCursor.GetLastCursorPosition());
                        }
                    });
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Move();
    }

    private void HandleEnemy()
    {
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Light"))
        {
            Debug.Log("Entered light");
            _inLight = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Light"))
        {
            Debug.Log("Left light");
            _inLight = false;
            _lightExitTime = Time.time;
        }
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

    // Use this for initialization
    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _faceCursor = GetComponent<FaceCursor>();
    }

    private void Update()
    {
        if (_disableInput) return;

        FireWeapon();
        HandleEnemy();

        if (GameController.Instance.SuperCounter <= 0)
        {
            GameController.Instance.SuperCounter = 100000000;
            EnableSuperMode();
        }

        if (_healthDisplay.CurrentHealth <= 0)
        {
            Die();
        }

        if (!_inLight)
        {
            if (NoLightDieTimer >= Time.time - _lightExitTime)
            {
                for (var i = 0; i < 8; ++i)
                {
                    _healthDisplay.RemoveHealth();
                }

                Die();
            }
        }
    }
}