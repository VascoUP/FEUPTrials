using UnityEngine;

public delegate void SetPlayer(GameObject bike);
public delegate void PlayerFinish(PlayerStats stats /*float time, int faults*/);

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerPrefab;
    [SerializeField]
    private GameObject _bikePrefab;

    [SerializeField]
    private Time time;

    private Checkpoint _checkpoint;
    public GameObject activeBike;
    
    public SetPlayer NewPlayer;
    public PlayerFinish GameOver;

    public float timeCounter = 0f;
    private int _faults = -1;
    public bool isFinished = false;

    private void Start()
    {
        NewPlayer += new SetPlayer(NewPlayerFinish);
        GameOver += new PlayerFinish(FinishGame);

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
        
        Restart();
    }

    private void Update ()
    {
        if (isFinished)
            return;

        bool isRestart = InputManager.IsRestart(this.transform);
        if (isRestart)
            Restart();
        
        timeCounter += Time.deltaTime;
	}

    private void FinishGame(PlayerStats stats)
    {
        isFinished = true;

        // detach bike and player
    }

    private void NewPlayerFinish(GameObject bike)
    {
        BikeController bikeController = bike.GetComponent<BikeController>();
        GameOver += new PlayerFinish(bikeController.FinishedGame);
    }

    public void FinalCheckpoint()
    {
        GameOver(new PlayerStats(timeCounter, _faults));
    }

    private void Restart()
    {
        _faults++;

        // Destroy both player and bike game objects
        GameObject tmp = Utils.FilterTaggedObjectByParentAndName("Body", "Player Body", transform.parent.name);
        if (tmp != null)
            Destroy(tmp);

        if (activeBike != null)
        {
            BikeController bikeController = activeBike.GetComponent<BikeController>();
            GameOver -= bikeController.FinishedGame;
            Destroy(activeBike);
        }

        // Instantiate bike
        activeBike = Instantiate(_bikePrefab);
        activeBike.name = "Bike";
        activeBike.transform.parent = transform.parent;
        activeBike.transform.position = _checkpoint.BikeNewPosition();
        Utils.SetLayer(activeBike.transform, gameObject.layer + 1);

        // Instantiate player
        tmp = Instantiate(_playerPrefab);
        tmp.name = "Player Body";
        tmp.transform.parent = transform.parent;
        tmp.transform.position = _checkpoint.BikeNewPosition();
        Utils.SetLayer(tmp.transform, gameObject.layer + 1);
        
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
