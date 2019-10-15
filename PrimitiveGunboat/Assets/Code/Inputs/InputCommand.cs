using System;
using UnityEngine;


public struct InputCommand
{
    public Vector3 Coords;
}

[Flags]
public enum CommandType
{
    None = 0,
    Fire,
    
}