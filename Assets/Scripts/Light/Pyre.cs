﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Pyre.cs" author="Lars" company="None">
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

using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;

public class Pyre : MonoBehaviour
{
    [SerializeField]
    private Transform _fire;

    [SerializeField]
    private Transform _lightSource;

    [SerializeField]
    private List<Transform> _enemyBurnupSprites;

    private List<Transform> _activeBurnupSprites = new List<Transform>();

    private Vector3 _lightSourceIncreaseAmount = new Vector3(0.135f, 0.135f, 0.135f);

    private float _decreaseLightStrength = 0.05f;

    public void AddFuel()
    {
        _fire.DOShakeScale(0.25f, 2.0f);

        var scale = Vector2.ClampMagnitude(_lightSource.transform.localScale + _lightSourceIncreaseAmount, 1.2f);

        _lightSource.DOScale(scale, 0.25f)
            .SetEase(Ease.OutBack);

        if (_enemyBurnupSprites.Count > 0)
        {
            var burnupSprite = _enemyBurnupSprites[RandomProvider.Instance.Random.Next(0, _enemyBurnupSprites.Count)];
            _enemyBurnupSprites.Remove(burnupSprite);

            burnupSprite.gameObject.SetActive(true);
            _activeBurnupSprites.Add(burnupSprite);

            StartCoroutine(ReturnBurnupSpriteToPool(burnupSprite));
        }

        GameController.Instance.EnemiesBurned++;
    }

    private void Update()
    {
        _lightSource.transform.localScale = Vector2.MoveTowards(
            _lightSource.transform.localScale,
            Vector2.zero,
            _decreaseLightStrength * Time.deltaTime);
    }

    private IEnumerator ReturnBurnupSpriteToPool(Transform burnupSprite)
    {
        yield return new WaitForSeconds(1.0f);

        _activeBurnupSprites.Remove(burnupSprite);
        _enemyBurnupSprites.Add(burnupSprite);
        burnupSprite.gameObject.SetActive(false);
    }
}