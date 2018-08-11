// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Temporalis">
//    Copyright (c) 2018, Lars-Kristian Svenøy. All rights reserved
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using UnityEngine;

using Random = System.Random;

public static class Extensions
{
    public static void ForEach<T>([NotNull] this IEnumerable<T> ie, [NotNull] Action<T> action)
    {
        foreach (var i in ie) action(i);
    }

    /// <summary>
    ///     Shuffle an Array
    /// </summary>
    /// <param name="array">The array to shuffle</param>
    /// <param name="random"></param>
    public static void Shuffle<T>([NotNull] this T[] array, [NotNull] Random random)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (var t = 0; t < array.Length; t++)
        {
            var tmp = array[t];
            var r = random.Next(t, array.Length - 1);
            array[t] = array[r];
            array[r] = tmp;
        }
    }

    /// <summary>
    ///     Shuffle a List
    /// </summary>
    /// <param name="list">The list to shuffle</param>
    /// <param name="random"></param>
    public static void Shuffle<T>([NotNull] this List<T> list, [NotNull] Random random)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = random.Next(n + 1);
            var value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static Direction ToDirection(this Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            return Direction.North;
        }

        if (direction == Vector2Int.down)
        {
            return Direction.South;
        }

        if (direction == Vector2Int.left)
        {
            return Direction.West;
        }

        if (direction == Vector2Int.right)
        {
            return Direction.East;
        }

        return Direction.North;
    }

    public static Vector2Int ToVector2Int(this Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return Vector2Int.up;
            case Direction.South:
                return Vector2Int.down;
            case Direction.West:
                return Vector2Int.left;
            case Direction.East:
                return Vector2Int.right;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction));
        }
    }
}