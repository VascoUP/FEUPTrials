using System.Collections;
using UnityEngine;

public class Interactable : MonoBehaviour {
    private enum InteractionStates { INTERACTABLE, CHANNELING, COMPLETED }
    private InteractionStates state = InteractionStates.INTERACTABLE;

    private bool[] _playerColliders = new bool[2];
    private bool _channelIsPlayerOne;

    public GameObject interactionObject;
    private IInteract _interaction;
    private Animator _animator;
    
    [SerializeField]
    private GameObject _interactionTextObject;
    [SerializeField]
    private Vector2 _offset;
    [SerializeField]
    private float _rotation;
    private GameObject[] _instantiatedTextObjects;

    [SerializeField]
    private float _channelingTime;

    private Coroutine _runningThread;

    private void Start()
    {
        _interaction = (IInteract) interactionObject.GetComponent(typeof(IInteract));
        _animator = GetComponent<Animator>();
        _instantiatedTextObjects = new GameObject[2];
    }

    private IEnumerator ChannelingTime()
    {
        state = InteractionStates.CHANNELING;

        if (_animator != null)
            _animator.Play("ChannelingAnimation");

        yield return new WaitForSeconds(_channelingTime);

        if(state == InteractionStates.CHANNELING)
        {
            state = InteractionStates.COMPLETED;

            DestroyText(0);
            DestroyText(1);

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

        PlayerOnTop(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Bike" || state == InteractionStates.COMPLETED)
            return;

        PlayerExit(collision.gameObject);
    }

    private void PlayerOnTop(GameObject player)
    {
        if(state == InteractionStates.COMPLETED)
        {
            return;
        }

        bool isPlayerOne = PlayerManager.IsPlayerOne(player.transform);
        _playerColliders[isPlayerOne ? 0 : 1] = true;

        int index = isPlayerOne ? 0 : 1;
        string text = state == InteractionStates.CHANNELING ? "Wait" : "Interact";
        InstantiateText(index, text);
    }

    private void PlayerExit(GameObject player)
    {
        bool isPlayerOne = PlayerManager.IsPlayerOne(player.transform);
        _playerColliders[isPlayerOne ? 0 : 1] = false;
        
        int index = isPlayerOne ? 0 : 1;
        DestroyText(index);

        if (state == InteractionStates.CHANNELING)
        {
            if ((isPlayerOne && _channelIsPlayerOne) || (!isPlayerOne && !_channelIsPlayerOne))
                // Cancel channeling
                StopChanneling();
        }
    }

    private void InstantiateText(int index, string text)
    {
        if(_instantiatedTextObjects[index] == null)
        {
            _instantiatedTextObjects[index] = Instantiate(_interactionTextObject, transform);
            _instantiatedTextObjects[index].transform.Rotate(Vector3.forward * _rotation);
            _instantiatedTextObjects[index].transform.localPosition = new Vector3(_offset.x, _offset.y, 0);
            Utils.SetLayer(_instantiatedTextObjects[index].transform, 8 + index * 2);
            SetText(index, text);
        }
    }

    private void DestroyText(int index)
    {
        if (_instantiatedTextObjects[index] != null)
        {
            Destroy(_instantiatedTextObjects[index]);
            _instantiatedTextObjects[index] = null;
        }
    }

    private void SetText(int index, string text)
    {
        if(_instantiatedTextObjects[index] != null)
        {
            _instantiatedTextObjects[index].GetComponent<TextMesh>().text = text;
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

        state = InteractionStates.INTERACTABLE;
        if (_animator != null)
            _animator.Rebind();

        SetText(0, "Interact");
        SetText(1, "Interact");
    }

    private void StartChanneling(bool isPlayerOne)
    {
        _channelIsPlayerOne = isPlayerOne;
        _runningThread = StartCoroutine("ChannelingTime");

        if(isPlayerOne)
        {
            SetText(0, "Channeling");
            SetText(1, "Wait");
        } else
        {
            SetText(0, "Wait");
            SetText(1, "Channeling");
        }
    }
    
    private void Update () {
        if (state == InteractionStates.INTERACTABLE)
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
