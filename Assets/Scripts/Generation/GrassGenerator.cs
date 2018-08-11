// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GrassGenerator.cs" author="Lars" company="None">
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

using JetBrains.Annotations;

using UnityEngine;

public class GrassGenerator : MonoBehaviour
{
    private const int MapHeight = 20;

    private const int MapWidth = 20;

    [NotNull]
    [SerializeField]
    private SpriteRenderer _grassPrefab;

    [NotNull]
    [SerializeField]
    private Sprite[] _grassSprites;

    private void CreateGrass()
    {
        for (var x = -MapWidth / 2; x < MapWidth / 2; x++)
        {
            for (var y = -MapHeight / 2; y < MapHeight / 2; y++)
            {
                var grassSprite = _grassSprites[RandomProvider.Instance.Random.Next(0, _grassSprites.Length)];
                var grassTile = Instantiate(_grassPrefab, new Vector3(x, y, 0), Quaternion.identity);
                grassTile.sprite = grassSprite;
            }
        }
    }

    // Use this for initialization
    private void Start()
    {
        CreateGrass();
    }
}