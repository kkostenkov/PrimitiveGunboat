using System;
using System.Collections.Generic;
using UnityEngine;


public class InputReader : ICommandSource
{
    private readonly Func<Vector3, Vector3> screenToWorld;
    private CommandType commandsThisFrame;

    private Dictionary<int, InputCommand> allInputs = new Dictionary<int, InputCommand>()
    {
        {(int)CommandType.None, new InputCommand()},
        {(int)CommandType.Fire, new InputCommand()},
    };

    public InputReader(Func<Vector3, Vector3> convertScreenToWorld)
    {
        this.screenToWorld = convertScreenToWorld;
    }

    public void ReadFrameInputs()
    {
        commandsThisFrame = CommandType.None;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            commandsThisFrame ^= CommandType.Fire;
            var mousePos = Input.mousePosition;
            var point = screenToWorld(mousePos);
            var fireCommand = new InputCommand()
            {
                Coords = point
            };
            allInputs[(int)CommandType.Fire] = fireCommand;
        }
    }

    public bool HasCommand(CommandType commandType)
    {
        return (commandsThisFrame & commandType) == commandType;
    }

    public InputCommand GetLastCommand(CommandType commandType)
    {
        return allInputs[(int) commandType];
    }
}

