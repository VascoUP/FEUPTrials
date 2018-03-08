using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    private Stack states = new Stack();

    [SerializeField]
    internal PrefabManager prefabManager;

    GameManager()
    {
        //Debug.Log("Start");
        //PushStateNoLoad(new MainMenu());
    }

    void Awake()
    {
        // Enforce the singleton pattern
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start ()
    {
        Debug.Log("Start");
        PushStateNoLoad(new MainMenu());
	}

	void Update ()
    {
        PeekState().Update();
	}

    internal void PushStateNoLoad(IGameState state)
    {
        states.Push(state);
    }

    internal void PushState(IGameState state)
    {
        states.Push(state);
        PeekState().LoadScene();
    }

    internal void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (StatesCount() <= 0)
            return;

        PeekState().OnEnter();
    }

    internal void PopState()
    {
        PeekState().OnExit();
        states.Pop();
    }

    internal void ChangeState(IGameState state)
    {
        if (states.Count != 0) PopState();
        PushState(state);
    }

    internal int StatesCount()
    {
        return states.Count;
    }

    IGameState PeekState()
    {
        return (IGameState)states.Peek();
    }
}
