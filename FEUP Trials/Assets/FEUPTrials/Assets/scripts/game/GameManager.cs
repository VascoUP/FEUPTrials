using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    private Stack _states = new Stack();

    [SerializeField]
    internal PrefabManager prefabManager;

    [SerializeField]
    internal float spTimeLimit;
    [SerializeField]
    internal int spFaultLimit;

    GameManager()
    {
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
        PushState(new MainMenu());
	}

	void Update ()
    {
        PeekState().Update();
	}

    internal void PushStateNoLoad(IGameState state)
    {
        _states.Push(state);
    }

    internal void PushState(IGameState state)
    {
        _states.Push(state);
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
        _states.Pop();
    }

    internal void ChangeState(IGameState state)
    {
        if (_states.Count != 0) PopState();
        PushState(state);
    }

    internal int StatesCount()
    {
        return _states.Count;
    }

    IGameState PeekState()
    {
        return (IGameState)_states.Peek();
    }
}
