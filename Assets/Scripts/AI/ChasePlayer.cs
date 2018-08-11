// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChasePlayer.cs" author="Lars" company="None">
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
[RequireComponent(typeof(FaceTarget))]
public class ChasePlayer : MonoBehaviour
{
    public enum Acceleration
    {
        Slowest = 10,
        Slow = 20,
        Normal = 30,
        Fast = 40,
        Faster = 50,
        Fastest = 60
    }

    private const float MaxVelocity = 3f;

    private readonly float _acceleration = (float)Acceleration.Normal;

    private Vector2 _movement;

    private Rigidbody2D _player;

    private Rigidbody2D _rigidBody;

    // Update is called once per frame
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _movement = _player.position - _rigidBody.position;
        _movement.Normalize();

        _rigidBody.AddForce(_movement * _acceleration);
    }

    // Use this for initialization
    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _player = GameObject.Find("Player").GetComponent<Rigidbody2D>();

        GetComponent<FaceTarget>().SetTarget(_player);
    }
}