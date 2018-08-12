// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Crossbow.cs" author="Lars" company="None">
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

using System.Collections.Generic;

using JetBrains.Annotations;

using UnityEngine;

using Utilities.ObjectPool;

[RequireComponent(typeof(Animator))]
public class Crossbow : Weapon
{
    private const string FireTrigger = "Fire";

    private const string ReloadTrigger = "Reload";

    private Animator _crossbowAnimator;

    private float _lastFireTime;

    private Animator _stringAnimator;

    [SerializeField]
    private Transform _arrow;

    public override bool Fire(Vector2 cursorPosition)
    {
        var timePassed = Time.time - _lastFireTime;
        if (FireCooldown > timePassed) return false;

        var upDir = cursorPosition - (Vector2)transform.position;
        upDir.Normalize();

        ProcessHits(upDir);

        var trail = ObjectPools.Instance.GetPooledObject<ArrowTrail>();
        var startPos = new Vector2(_arrow.position.x - (upDir.x * 0.16f), _arrow.position.y - (upDir.y * 0.16f));
        trail.Move(startPos, 15 * upDir, 0.10f);

        _stringAnimator.SetTrigger(FireTrigger);
        _crossbowAnimator.SetTrigger(FireTrigger);
        _lastFireTime = Time.time;

        AudioPlayer.Instance.PlayOneShot(AudioPlayer.Instance.Shoot, 0.25f);

        Invoke(nameof(Reload), 0.2f);

        return true;
    }

    private void Reload()
    {
        _crossbowAnimator.SetTrigger(ReloadTrigger);
        _stringAnimator.SetTrigger(ReloadTrigger);
    }

    // Use this for initialization
    private void Start()
    {
        _crossbowAnimator = GetComponent<Animator>();
        _stringAnimator = transform.GetChild(3).GetComponent<Animator>();
    }

    private void ProcessHits(Vector2 upDir)
    {
        var hits = Physics2D.RaycastAll(transform.position, upDir);

        var hitTransforms = new HashSet<Transform>();

        var deathCount = 0;

        Vector2? firstHitPosition = null;

        foreach (var hit in hits)
        {
            if (hitTransforms.Contains(hit.transform)) continue;
            hitTransforms.Add(hit.transform);

            if (hit.transform.CompareTag("Enemy"))
            {
                if (!firstHitPosition.HasValue)
                {
                    firstHitPosition = hit.transform.position;
                }

                var enemy = hit.transform.GetComponent<Enemy>();
                var died = enemy.TakeDamage(1f);

                if (died) deathCount++;

                var force = enemy.transform.position - transform.position;
                force.Normalize();

                enemy.DoImpact(force * 10f);
            }
        }

        if (firstHitPosition.HasValue)
        {
            AudioPlayer.Instance.PlayOneShot(AudioPlayer.Instance.EnemyHit, 0.1f);
        }

        if (deathCount > 0)
        {
            var popupPos = firstHitPosition ?? transform.position;
            ScoreDisplay.Instance.AddScore(100, deathCount, "- Kills", popupPos);
        }
    }
}