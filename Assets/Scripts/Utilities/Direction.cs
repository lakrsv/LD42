// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Direction.cs" company="Temporalis">
//    Copyright (c) 2018, Lars-Kristian Svenøy. All rights reserved
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

[Flags]
public enum Direction
{
    North = 1,
    West = 2,
    East = 4,
    South = 8
}