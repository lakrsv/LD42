// --------------------------------------------------------------------------------------------------------------------
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
    private const float MaxLightSourceScale = 2.0f;

    private readonly List<Animator> _activeBurnupSprites = new List<Animator>();

    private float _decreaseLightStrength = 0.05f;

    [SerializeField]
    private List<Animator> _enemyBurnupSprites;

    [SerializeField]
    private Transform _fire;

    [SerializeField]
    private Transform _lightSource;

    private readonly Vector3 _lightSourceIncreaseAmount = new Vector3(0.09f, 0.09f, 0.09f);

    public void AddFuel(bool addPoints = true)
    {
        _fire.DOShakeScale(0.25f, 2.0f);

        var lightAreaScale = Vector2.ClampMagnitude(
            _lightSource.transform.localScale + _lightSourceIncreaseAmount,
            MaxLightSourceScale);

        _lightSource.DOScale(lightAreaScale, 0.25f).SetEase(Ease.OutBack);

        if (_enemyBurnupSprites.Count > 0)
        {
            var burnupSprite = _enemyBurnupSprites[RandomProvider.Instance.Random.Next(0, _enemyBurnupSprites.Count)];
            _enemyBurnupSprites.Remove(burnupSprite);

            burnupSprite.gameObject.SetActive(true);
            burnupSprite.SetTrigger("Reset");

            _activeBurnupSprites.Add(burnupSprite);

            StartCoroutine(ReturnBurnupSpriteToPool(burnupSprite));

            var playerPos = ActorChoreographer.Instance.Player.transform.position;

            var throwSequence = DOTween.Sequence().OnStart(
                () => AudioPlayer.Instance.PlayOneShot(AudioPlayer.Instance.ThrowCorpse, 0.10f));
            throwSequence.Append(burnupSprite.transform.DOMove(playerPos, 0.5f).From().SetEase(Ease.OutSine));
            throwSequence.Insert(0f, burnupSprite.transform.DOScale(4.0f, 0.25f).SetEase(Ease.OutSine));
            throwSequence.Insert(0.125f, burnupSprite.transform.DOScale(2.0f, 0.25f).SetEase(Ease.InSine));
            throwSequence.OnComplete(() =>
                {
                    if (addPoints)
                    {
                        var popupPos = burnupSprite.targetPosition;
                        popupPos.y -= 0.25f;
                        ScoreDisplay.Instance.AddScore(100, 1, "- Light", popupPos);

                        AudioPlayer.Instance.PlayOneShot(AudioPlayer.Instance.CorpseBurn, 0.10f);
                    }

                    burnupSprite.SetTrigger("Animate");
                });
        }

        GameController.Instance.EnemiesBurned++;
    }

    private IEnumerator ReturnBurnupSpriteToPool(Animator burnupSprite)
    {
        yield return new WaitForSeconds(1.0f);

        burnupSprite.SetTrigger("Reset");
        _activeBurnupSprites.Remove(burnupSprite);
        _enemyBurnupSprites.Add(burnupSprite);
        burnupSprite.gameObject.SetActive(false);
    }

    private void Update()
    {
        _lightSource.localScale = Vector2.MoveTowards(
            _lightSource.localScale,
            Vector2.zero,
            _decreaseLightStrength * Time.deltaTime);

        var fireScale = 5f * (_lightSource.localScale.magnitude / MaxLightSourceScale);
        _fire.localScale = Vector2.MoveTowards(
            _fire.localScale,
            new Vector2(fireScale, fireScale),
            5f * Time.deltaTime);
    }

    public void Extinquish()
    {
        _decreaseLightStrength = 0.6f;
    }
}