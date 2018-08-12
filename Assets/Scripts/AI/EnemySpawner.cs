// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnemySpawner.cs" author="Lars" company="None">
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

using UnityEngine;

using Utilities.ObjectPool;

public class EnemySpawner : MonoBehaviour
{
    private const float SpawnDistance = 15f;

    private int _minEnemies = 1;

    private int _maxEnemies = 2;

    // Use this for initialization
    private IEnumerator Start()
    {
        var random = RandomProvider.Instance.Random;
        var pool = ObjectPools.Instance;
        var actorChoreographer = ActorChoreographer.Instance;

        while (GameController.Instance.IsPlaying)
        {
            var minEnemies = 1 + GameController.Instance.EnemiesKilled / 20;
            var maxEnemies = 1 + GameController.Instance.EnemiesKilled / 10;

            var enemyAmount = RandomProvider.Instance.Random.Next(minEnemies, maxEnemies + 1);

            for (var i = 0; i < enemyAmount; ++i)
            {
                // Spawn enemies.
                var enemy = pool.GetPooledObject<Enemy>();
                var enemyPos = GetPositionOnCircle(random.Next(0, 360), SpawnDistance);

                enemy.transform.position = enemyPos;
            }

            yield return new WaitUntil(() => actorChoreographer.Enemies.Count == 0);
        }
    }

    private Vector2 GetPositionOnCircle(float angleDegrees, float radius)
    {
        var angleRadians = angleDegrees * Mathf.PI / 180f;
        var x = radius * Mathf.Cos(angleRadians);
        var y = radius * Mathf.Sin(angleRadians);

        return new Vector2(x, y);
    }
}