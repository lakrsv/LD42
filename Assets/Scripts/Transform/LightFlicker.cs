// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LightFlicker.cs" author="Lars" company="None">
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

[RequireComponent(typeof(Transform))]
public class LightFlicker : MonoBehaviour
{
    private const int NoiseMax = 255;

    [SerializeField]
    private float _flickerStrength = 0.1f;

    private int _currentTick;

    private float[] _noiseValues;

    private Vector2 _targetScale;

    private Vector2 _scale = new Vector2();

    private Transform _transform;

    // Use this for initialization
    private void Start()
    {
        _noiseValues = Simplex.Noise.Calc1D(1000, 0.1f);
        _transform = GetComponent<Transform>();
        _targetScale = _transform.localScale;
    }

    // Update is called once per frame
    private void Update()
    {
        var noise = (_noiseValues[_currentTick] / NoiseMax) * _flickerStrength;
        _scale.Set(_targetScale.x + noise, _targetScale.y + noise);
        _transform.localScale = _scale;

        _currentTick++;
        if (_currentTick >= _noiseValues.Length) _currentTick = 0;
    }
}