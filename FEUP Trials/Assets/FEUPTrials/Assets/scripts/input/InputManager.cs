using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager {
    private enum ButtonType { DOWN, UP, PRESSED };

    public static float GetAxis(Transform trans, string axis)
    {
        string parentName = trans.parent.name;
        bool isPlayerOne = PlayerManager.IsPlayerOne(trans);
        axis = GetButtonName(isPlayerOne, axis);
        return Input.GetAxis(axis);
    }

    private static string GetButtonName(bool isPlayerOne, string button)
    {
        return (isPlayerOne ? "P1_" : "P2_") + button;
    }

    private static bool GetButton(bool isPlayerOne, string button, ButtonType buttonType)
    {
        button = GetButtonName(isPlayerOne, button);
        switch(buttonType)
        {
        case ButtonType.DOWN:
            return Input.GetButtonDown(button);
        case ButtonType.UP:
            return Input.GetButtonUp(button);
        case ButtonType.PRESSED:
            return Input.GetButton(button);

        }
        return Input.GetButton(button);
    }

    private static bool GetButton(Transform trans, string button, ButtonType buttonType)
    {
        bool isPlayerOne = PlayerManager.IsPlayerOne(trans);
        return GetButton(isPlayerOne, button, buttonType);
    }

    public static bool IsInteracting(bool isPlayerOne)
    {
        return GetButton(isPlayerOne, "Interaction", ButtonType.PRESSED);
    }

    public static bool IsInteracting(Transform trans)
    {
        return GetButton(trans, "Interaction", ButtonType.PRESSED);
    }

    public static bool IsRestart(Transform trans)
    {
        return GetButton(trans, "Restart", ButtonType.DOWN);
    }

    public static bool IsNext()
    {
        return Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
    }
}
