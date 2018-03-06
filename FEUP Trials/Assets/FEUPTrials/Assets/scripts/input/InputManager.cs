using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager {
    public static float GetAxis(Transform trans, string axis)
    {
        string parentName = trans.parent.name;
        bool isPlayerOne = PlayerManager.IsPlayerOne(trans);
        axis = (isPlayerOne ? "P1_" : "P2_") + axis;
        return Input.GetAxis(axis);
    }

    private static bool GetButton(bool isPlayerOne, string button)
    {
        button = (isPlayerOne ? "P1_" : "P2_") + button;
        return Input.GetButton(button);
    }

    private static bool GetButton(Transform trans, string button)
    {
        string parentName = trans.parent.name;
        bool isPlayerOne = PlayerManager.IsPlayerOne(trans);
        return GetButton(isPlayerOne, button);
    }

    public static bool IsInteracting(bool isPlayerOne)
    {
        return GetButton(isPlayerOne, "Interaction");
    }

    public static bool IsInteracting(Transform trans)
    {
        return GetButton(trans, "Interaction");
    }

    public static bool IsRestart(Transform trans)
    {
        return GetButton(trans, "Restart");
    }
}
