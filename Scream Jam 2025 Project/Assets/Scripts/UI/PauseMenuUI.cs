using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private UIDocument uiDoc;
    [SerializeField] public Button pauseButton;

    private VisualElement rootElement;
    private Button resumeButton;
    private Button mainMenuButton;
    private Button quitButton;

    //private Button pauseButton;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.instance.OnPause += ToggleVisibility;
        rootElement = uiDoc.rootVisualElement;

        resumeButton = rootElement.Q<Button>("ResumeGameButton");
        mainMenuButton = rootElement.Q<Button>("MainMenuButton");
        quitButton = rootElement.Q<Button>("QuitButton");

        //pauseButton = rootElement.Q<Button>("pasueButton");

        resumeButton.RegisterCallback<ClickEvent>(e =>
        {
            ResumeGame();
        });

        mainMenuButton.RegisterCallback<ClickEvent>(e =>
        {
            ReturnToMainMenu();
        });

        quitButton.RegisterCallback<ClickEvent>(e =>
        {
            QuitGame();
        });
        
        pauseButton?.RegisterCallback<ClickEvent>(e => TriggerPause());

        if (rootElement.visible)
        {
            ToggleVisibility();
        }
    }

    private void ToggleVisibility()
    {
        rootElement.visible = !rootElement.visible;
    }

    private void ResumeGame()
    {
        GameManager.instance.TogglePause();
    }

    private void ReturnToMainMenu()
    {
        Debug.Log("Main menuing!");
        SceneManager.LoadScene("Main Menu");
    }

    private void QuitGame()
    {
        Debug.Log("Quitting!");
        Application.Quit();
    }
    
    // can replace w diff quitting mechanism if needbe
    private void TriggerPause()
    {
        Debug.Log("ts should pause");
        GameManager.instance.TogglePause();
    }
}
