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

public class Enemy : MonoBehaviour
{
    private float _health = 1f;

    private Rigidbody2D _rigidBody;

    private ChasePlayer _chasePlayer;

    private FaceTarget _faceTarget;

    public bool TakeDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Die();
            return true;
        }

        return false;
    }

    public void AddForce(Vector2 force)
    {
        _rigidBody.AddForce(force, ForceMode2D.Impulse);
    }

    private void Die()
    {
        _chasePlayer.enabled = false;
        _faceTarget.enabled = false;
        Debug.Log("I am dead.");
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _chasePlayer = GetComponent<ChasePlayer>();
        _faceTarget = GetComponent<FaceTarget>();
    }
}