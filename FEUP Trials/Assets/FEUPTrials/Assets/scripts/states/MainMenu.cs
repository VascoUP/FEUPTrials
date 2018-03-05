using UnityEngine;

internal class MainMenu : IGameState
{
    public void OnEnter()
    {
        Debug.Log("On Enter");
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
        Debug.Log("Update");
    }
}