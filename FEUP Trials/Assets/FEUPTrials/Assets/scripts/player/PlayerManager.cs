using UnityEngine;

public delegate void SetPlayer(GameObject bike);

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerPrefab;
    [SerializeField]
    private GameObject _bikePrefab;

    private Checkpoint _checkpoint;
    public GameObject activeBike;
    
    public SetPlayer NewPlayer;

    public float timeCounter = 0f;
    public bool isFinished = false;

    private void Start()
    {
        // Get first checkpoint
        GameObject tmp = Utils.FilterTaggedObjectByParentAndName("Checkpoint", "Checkpoints", transform.parent.name);
        foreach(Transform child in tmp.transform)
        {
            Checkpoint checkpoint = child.GetComponent<Checkpoint>();
            if(checkpoint != null)
            {
                checkpoint.SetPlayerManager(this);
            }

            if (child.name == "Checkpoint 0")
            {
                _checkpoint = checkpoint;
            }
        }
        if (_checkpoint == null)
            Debug.LogError("Checkpoint null");

        // Get ground root object
        GameObject groundRoot = GameObject.Find("Ground Root");
        GroundManager gManager = groundRoot.GetComponent<GroundManager>();
        if(transform.parent.name == "Player 1")
            NewPlayer += new SetPlayer(gManager.SetPlayerOneObject);
        else
            NewPlayer += new SetPlayer(gManager.SetPlayerTwoObject);

        // Get Checkpoint for this object


        Restart();
    }

    private void Update ()
    {
        bool isRestart = InputManager.IsRestart(this.transform);
        if (isRestart)
            Restart();

        if(!isFinished)
            timeCounter += Time.deltaTime;
	}

    public void FinishGame()
    {
        isFinished = true;
    }

    private void Restart()
    {
        // Destroy both player and bike game objects
        GameObject tmp = Utils.FilterTaggedObjectByParentAndName("Body", "Player Body", transform.parent.name);
        if (tmp != null)
            Destroy(tmp);
        tmp = Utils.FilterTaggedObjectByParent("Bike", transform.parent.name);
        if (tmp != null)
            Destroy(tmp);

        // Instantiate bike
        activeBike = Instantiate(_bikePrefab);
        activeBike.name = "Bike";
        activeBike.transform.parent = transform.parent;
        activeBike.transform.position = _checkpoint.BikeNewPosition();
        Utils.SetLayer(activeBike.transform, gameObject.layer);

        // Instantiate player
        tmp = Instantiate(_playerPrefab);
        tmp.name = "Player Body";
        tmp.transform.parent = transform.parent;
        tmp.transform.position = _checkpoint.BikeNewPosition();
        Utils.SetLayer(tmp.transform, gameObject.layer);
        
        if(NewPlayer != null)
            NewPlayer(activeBike);
    }

    public void SetCheckpoint(Checkpoint checkpoint)
    {
        _checkpoint = checkpoint;
    }

    public static bool IsPlayerOne(Transform transform)
    {
        string parentName = transform.parent.name;
        return (parentName == "Player 1");
    }
}
