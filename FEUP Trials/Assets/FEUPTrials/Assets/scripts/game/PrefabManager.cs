using UnityEngine;

internal class PrefabManager : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    public new GameObject camera;
    
    public GameObject playerManager;
}