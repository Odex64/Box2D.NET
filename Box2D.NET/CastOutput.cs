// SPDX-FileCopyrightText: 2023 Erin Catto
// SPDX-FileCopyrightText: 2025 Carmine Pietroluongo
// SPDX-License-Identifier: MIT

using System.Numerics;

namespace Box2D.NET;

public struct CastOutput
{
    /// <summary>
    /// The surface normal at the hit point
    /// </summary>
	public Vector2 Normal;

    /// <summary>
    /// The surface hit point
    /// </summary>
    public Vector2 Point;

    /// <summary>
    /// The fraction of the input translation at collision
    /// </summary>
    public float Fraction;

    /// <summary>
    /// The number of iterations used
    /// </summary>
    public int Iterations;

    /// <summary>
    /// Did the cast hit?
    /// </summary>
    public bool Hit;
}