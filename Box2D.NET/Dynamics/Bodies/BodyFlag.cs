using System;

namespace Box2D.NET.Dynamics.Bodies;

[Flags]
public enum BodyFlag : byte
{
    Island = 0x0001,
    Awake = 0x0002,
    AutoSleep = 0x0004,
    Bullet = 0x0008,
    FixedRotation = 0x0010,
    Enabled = 0x0020,
    TOI = 0x0040
}