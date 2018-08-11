// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrowTrail.cs" author="Lars" company="None">
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

[RequireComponent(typeof(TrailRenderer))]
public class ArrowTrail : MonoBehaviour, IPoolable
{
    private TrailRenderer _renderer;

    public bool IsEnabled => gameObject.activeInHierarchy;

    private Vector2 _targetPosition;

    private Vector2 _startPosition;

    private float _duration;

    private float _timePassed;

    public void Enable()
    {
        if (_renderer == null) _renderer = GetComponent<TrailRenderer>();

        gameObject.SetActive(true);
        _renderer.Clear();
        _timePassed = 0;
    }

    public void Disable()
    {
        if (_renderer == null) _renderer = GetComponent<TrailRenderer>();

        _renderer.Clear();
        gameObject.SetActive(false);
        _timePassed = 0;
    }

    public void Move(Vector2 startPosition, Vector2 targetPosition, float duration)
    {
        _startPosition = startPosition;
        _targetPosition = targetPosition;
        _duration = duration;


        transform.position = _startPosition;
        _renderer.Clear();

        transform.rotation = Quaternion.LookRotation(Vector3.forward, targetPosition - (Vector2)transform.position);

        Invoke(nameof(Disable), duration * 3f);
    }

    private void Update()
    {
        var newPos = Vector2.Lerp(_startPosition, _targetPosition, _timePassed / _duration);
        transform.position = newPos;

        _timePassed += Time.deltaTime;
    }
}