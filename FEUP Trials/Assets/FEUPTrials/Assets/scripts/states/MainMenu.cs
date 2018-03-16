using UnityEngine;
using UnityEngine.SceneManagement;

internal class MainMenu : IGameState
{
    public void LoadScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnEnter()
    {}

    public void Update()
    {}

    public void OnExit()
    {}
}