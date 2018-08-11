// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaceCursor.cs" author="Lars" company="None">
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
public class FaceCursor : MonoBehaviour
{
    private Transform _transform;

    private Camera _mainCamera;

    private Vector2 _lastCursorPos;

    // Use this for initialization
    private void Start()
    {
        _transform = GetComponent<Transform>();
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        var mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        _transform.rotation = Quaternion.LookRotation(Vector3.forward, mouseWorldPos - _transform.position);
        _lastCursorPos = mouseWorldPos;
    }

    public Vector2 GetLastCursorPosition()
    {
        return _lastCursorPos;
    }
}