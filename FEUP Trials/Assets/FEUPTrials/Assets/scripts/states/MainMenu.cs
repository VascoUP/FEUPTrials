using UnityEngine;
using UnityEngine.SceneManagement;

internal class MainMenu : IGameState
{
    public void LoadScene()
    {
        SceneManager.LoadScene("Start");
    }

    public void OnEnter()
    {

    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            GameManager.instance.ChangeState(new Game());
        }
        if (Input.GetKey(KeyCode.S))
        {
            GameManager.instance.ChangeState(new Game(true));
        }
    }

    public void OnExit()
    {

    }
}