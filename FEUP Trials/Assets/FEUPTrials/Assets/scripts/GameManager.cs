using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    private Stack states = new Stack();

    [SerializeField]
    internal PrefabManager prefabManager;

    void Awake()
    {
        // Enforce the singleton pattern
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

	void Start () {
        PushState(new MainMenu());
	}

	void Update () {
        PeekState().Update();
	}

    internal void PushState(IGameState state)
    {
        states.Push(state);
        PeekState().OnEnter();
    }

    internal void PopState()
    {
        states.Pop();
    }

    internal void ChangeState(IGameState state)
    {
        if (states.Count == 0) PopState();
        PushState(state);
    }

    IGameState PeekState()
    {
        return (IGameState)states.Peek();
    }
}
