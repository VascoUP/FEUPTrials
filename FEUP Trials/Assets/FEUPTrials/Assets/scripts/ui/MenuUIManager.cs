using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour {
    private enum MenuState { MAIN_MENU, INSTRUCTIONS };
    private MenuState state = MenuState.MAIN_MENU;

    [SerializeField]
    private GameObject _mainMenuObject;
    [SerializeField]
    private GameObject _instructionsMenuObject;

    [SerializeField]
    private Button _spButton;
    [SerializeField]
    private Button _mpButton;
    [SerializeField]
    private Button _instructionsButton;
    [SerializeField]
    private Button _exitButton;
    [SerializeField]
    private Button _backButton;

    // Use this for initialization
    private void Start () {
        _spButton.onClick.AddListener(() => 
        {
            GameManager.instance.ChangeState(new Intro());
        });
        _mpButton.onClick.AddListener(() => 
        {
            GameManager.instance.ChangeState(new Game(true));
        });
        _instructionsButton.onClick.AddListener(() =>
        {
            state = MenuState.INSTRUCTIONS;
            _mainMenuObject.SetActive(false);
            _instructionsMenuObject.SetActive(true);
        });
        _backButton.onClick.AddListener(() =>
        {
            state = MenuState.MAIN_MENU;
            _mainMenuObject.SetActive(true);
            _instructionsMenuObject.SetActive(false);
        });
        _exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(state == MenuState.MAIN_MENU)
                Application.Quit();
            else
            {
                state = MenuState.MAIN_MENU;
                _mainMenuObject.SetActive(true);
                _instructionsMenuObject.SetActive(false);
            }
        }
    }
}
