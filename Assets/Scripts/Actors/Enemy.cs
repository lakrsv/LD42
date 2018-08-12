// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Enemy.cs" author="Lars" company="None">
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

using Utilities.ObjectPool;

public class Enemy : MonoBehaviour, IPoolable
{
    public float Health = 1f;

    private Rigidbody2D _rigidBody;

    private ChasePlayer _chasePlayer;

    private FaceTarget _faceTarget;

    [SerializeField]
    private Animator _walkAnimator;

    [SerializeField]
    private Animator _dieAnimator;

    [SerializeField]
    private Animator _expireAnimator;

    [SerializeField]
    private GameObject _pickupRadius;

    public bool IsEnabled => gameObject.activeInHierarchy;

    public bool TakeDamage(float damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Die();
            return true;
        }

        return false;
    }

    public void DoImpact(Vector2 force)
    {
        _rigidBody.AddForce(force, ForceMode2D.Impulse);

        var bloodSpray = ObjectPools.Instance.GetPooledObject<BloodSpray>();
        bloodSpray.transform.position = _rigidBody.position;
        bloodSpray.transform.rotation = Quaternion.LookRotation(Vector3.forward, force);
        ////bloodSpray.AddForce(force);
    }

    private void Die()
    {
        _chasePlayer.enabled = false;
        _faceTarget.enabled = false;
        _walkAnimator.gameObject.SetActive(false);
        _dieAnimator.gameObject.SetActive(true);
        _pickupRadius.gameObject.SetActive(true);

        GameController.Instance.EnemiesKilled++;

        ActorChoreographer.Instance.DeregisterEnemy(this);

        Debug.Log("I am dead.");

        Invoke(nameof(Expire), 2.0f);
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _chasePlayer = GetComponent<ChasePlayer>();
        _faceTarget = GetComponent<FaceTarget>();
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        ActorChoreographer.Instance.RegisterEnemy(this);
    }

    public void Disable()
    {
        CancelInvoke();

        _chasePlayer.enabled = true;
        _faceTarget.enabled = true;
        _walkAnimator.gameObject.SetActive(true);
        _dieAnimator.gameObject.SetActive(false);
        _pickupRadius.gameObject.SetActive(false);
        _expireAnimator.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }

    private void Expire()
    {
        _dieAnimator.gameObject.SetActive(false);
        _expireAnimator.gameObject.SetActive(true);
        Invoke(nameof(Disable), 2.2f);
    }
}