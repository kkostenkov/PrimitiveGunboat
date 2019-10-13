using System;
using UnityEngine;


public class InputCommand
{
    public Vector3 Coords;
}

[Flags]
public enum CommandType
{
    None = 0,
    Fire,

}