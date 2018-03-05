using UnityEngine;

internal class PrefabManager : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField]
    public new GameObject camera;

    [SerializeField]
    public GameObject playerManager;
}