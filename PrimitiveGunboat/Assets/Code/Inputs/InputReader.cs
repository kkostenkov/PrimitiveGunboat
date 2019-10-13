using System.Collections.Generic;
using UnityEngine;


public class InputReader : ICommandSource
{
    private Camera sessionCamera;
    private float sessionPlaneHeight;
    private CommandType commandsThisFrame;

    private Dictionary<int, InputCommand> allInputs = new Dictionary<int, InputCommand>()
    {
        {(int)CommandType.None, new InputCommand()},
        {(int)CommandType.Fire, new InputCommand()},
    };

    public InputReader(Camera sessionCamera, float heightCoord)
    {
        this.sessionCamera = sessionCamera;
        sessionPlaneHeight = heightCoord;
    }

    public void ReadFrameInputs()
    {
        commandsThisFrame = CommandType.None;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            commandsThisFrame ^= CommandType.Fire;
            var mousePos = Input.mousePosition;
            var point = sessionCamera.ScreenToWorldPoint(
                new Vector3(mousePos.x, mousePos.y, sessionPlaneHeight));
            allInputs[(int)CommandType.Fire].Coords = point;
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

