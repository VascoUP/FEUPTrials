using System;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class Intro : IGameState
{
    

    public void LoadScene()
    {
        SceneManager.LoadScene("Intro");
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void Update()
    {
    }
}