// SPDX-FileCopyrightText: 2025 Carmine Pietroluongo
// SPDX-License-Identifier: MIT

using System.Numerics;

namespace Box2D.NET.Extensions;

public static class Vector2Extension
{
    extension(Vector2 v)
    {
        public bool IsValid
        {
            get
            {
                if (float.IsNaN(v.X) || float.IsNaN(v.Y))
                {
                    return false;
                }

                if (float.IsInfinity(v.X) || float.IsInfinity(v.Y))
                {
                    return false;
                }

                return true;
            }
        }
    }
}