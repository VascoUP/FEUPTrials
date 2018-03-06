using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour {
    private enum InteractionStates { INTERACTABLE, CHANNELING, COMPLETED }
    private InteractionStates state = InteractionStates.INTERACTABLE;

    private bool[] _playerColliders = new bool[2];
    private bool _channelIsPlayerOne;

    public GameObject interactionObject;
    private IInteract _interaction;

    [SerializeField]
    private Text _interactionText;
    [SerializeField]
    private float _channelingTime;

    private Coroutine _runningThread;

    void Start()
    {
        _interaction = (IInteract) interactionObject.GetComponent(typeof(IInteract));
    }

    IEnumerator ChannelingTime()
    {
        state = InteractionStates.CHANNELING;

        yield return new WaitForSeconds(_channelingTime);

        if(state == InteractionStates.CHANNELING)
        {
            state = InteractionStates.COMPLETED;
            _interactionText.text = "";

            // Notify object
            _interaction.Completed();

            // Start completed animation
        }

        _runningThread = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Bike" || state == InteractionStates.COMPLETED)
            return;

        bool isPlayerOne = PlayerManager.IsPlayerOne(collision.transform);
        _playerColliders[isPlayerOne ? 0 : 1] = true;

        //Show UI E
        _interactionText.text = "!";
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Bike" || state == InteractionStates.COMPLETED)
            return;
        
        state = InteractionStates.INTERACTABLE;

        bool isPlayerOne = PlayerManager.IsPlayerOne(collision.transform);
        _playerColliders[isPlayerOne ? 0 : 1] = false;

        // Show UI nothing
        _interactionText.text = "";

        if (state == InteractionStates.CHANNELING)
        {
            if ((isPlayerOne && _channelIsPlayerOne) || (!isPlayerOne && !_channelIsPlayerOne))
                // Cancel channeling
                StopChanneling();
        }
    }

    private bool PlayerStoppedChanneling(bool playerOne)
    {
        return playerOne ? (_channelIsPlayerOne && !InputManager.IsInteracting(true)) : (!_channelIsPlayerOne && !InputManager.IsInteracting(false));
    }

    private void StopChanneling()
    {
        if (_runningThread != null)
            StopCoroutine(_runningThread);
        _runningThread = null;
        _interaction.Cancel();
    }

    private void StartChanneling(bool isPlayerOne)
    {
        _channelIsPlayerOne = isPlayerOne;
        _runningThread = StartCoroutine("ChannelingTime");
    }

    // Update is called once per frame
    void Update () {
        if(state == InteractionStates.INTERACTABLE)
        {
            bool pOne = _playerColliders[0] && InputManager.IsInteracting(true);
            if (pOne || (_playerColliders[1] && InputManager.IsInteracting(false)))
            {
                StartChanneling(pOne);

                // Start channeling animation
            }
        }
        else if(state == InteractionStates.CHANNELING)
        {
            if (PlayerStoppedChanneling(true) || PlayerStoppedChanneling(false))
            {
                state = InteractionStates.INTERACTABLE;

                // Cancel channeling
                StopChanneling();
            }
        }
	}


}
