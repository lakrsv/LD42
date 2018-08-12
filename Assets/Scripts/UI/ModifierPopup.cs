// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModifierPopup.cs" author="Lars" company="None">
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

using DG.Tweening;

using TMPro;

using UnityEngine;

using Utilities.ObjectPool;

[RequireComponent(typeof(TextMeshPro))]
public class ModifierPopup : MonoBehaviour, IPoolable
{
    private TextMeshPro _text;

    public bool IsEnabled => gameObject.activeInHierarchy;

    private Sequence _popTween;

    public void Disable()
    {
        if (_text == null) _text = GetComponent<TextMeshPro>();
        gameObject.SetActive(false);
    }

    public void Enable()
    {
        if (_text == null) _text = GetComponent<TextMeshPro>();
        gameObject.SetActive(true);

        _text.color = Color.white;

    }

    public void DoAnimate(int modifier, string modifierDescription, Vector2 startPosition)
    {
        transform.position = startPosition;
        _text.text = $"x{modifier} {modifierDescription}";

        if (_popTween != null)
        {
            _popTween.Restart();
        }
        else
        {
            _popTween = DOTween.Sequence().SetAutoKill(false);
            _popTween.Append(transform.DOScale(0f, 0.25f).From().SetEase(Ease.OutBack));
            _popTween.AppendInterval(1f);
            _popTween.Append(_text.DOColor(Color.clear, 0.5f));
            _popTween.OnComplete(Disable);
        }
    }
}