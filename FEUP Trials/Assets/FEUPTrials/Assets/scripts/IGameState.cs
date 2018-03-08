using UnityEngine.SceneManagement;

internal interface IGameState
{
    void LoadScene();

    void OnEnter();

    void Update();

    void OnExit();
}