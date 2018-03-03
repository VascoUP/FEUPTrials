﻿using UnityEngine;

public class BodyPartController : MonoBehaviour
{
    private bool _isBroken = false;
    private BikeController _bikeController;

    private void Start()
    {
        GameObject gameMan = GameObject.Find("Game Manager");
        if (gameMan == null)
            return;

        GameManager gameManScript = gameMan.GetComponent<GameManager>();
        if (gameManScript == null)
        {
            return;
        }

        GameObject bike = gameManScript.activeBike;
        if(bike == null)
        {
            return;
        }

        _bikeController = bike.GetComponent<BikeController>();
    }

    private void OnJointBreak2D(Joint2D joint)
    {
        _isBroken = true;
        _bikeController.StopMotion();
    }

    public bool IsBroken()
    {
        return _isBroken;
    }
}
