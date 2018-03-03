using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour {
    private enum InteractionStates { NON_INTERACTABLE, INTERACTABLE, CHANNELING, COMPLETED }
    private InteractionStates state = InteractionStates.NON_INTERACTABLE;

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
        
        state = InteractionStates.INTERACTABLE;

        //Show UI E
        _interactionText.text = "E";
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Bike" || state == InteractionStates.COMPLETED)
            return;
        
        state = InteractionStates.NON_INTERACTABLE;

        // Show UI nothing
        _interactionText.text = "";

        // Cancel channeling
        StopChanneling();
    }

    private void StopChanneling()
    {
        if (_runningThread != null)
            StopCoroutine(_runningThread);
        _runningThread = null;
        _interaction.Cancel();
    }

    private void StartChanneling()
    {
        _runningThread = StartCoroutine("ChannelingTime");
    }

    // Update is called once per frame
    void Update () {
        if(state == InteractionStates.INTERACTABLE)
        {
            if(Input.GetKey(KeyCode.E))
            {
                StartChanneling();

                // Start channeling animation
            }
        }
        else if(state == InteractionStates.CHANNELING)
        {
            if(!Input.GetKey(KeyCode.E))
            {
                state = InteractionStates.INTERACTABLE;

                // Cancel channeling
                StopChanneling();
            }
        }
	}
}
