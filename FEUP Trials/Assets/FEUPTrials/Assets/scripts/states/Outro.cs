using System;
using UnityEngine;
using UnityEngine.SceneManagement;


internal class Outro : IGameState
{
    public void LoadScene()
    {
        SceneManager.LoadScene("Outro");
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

