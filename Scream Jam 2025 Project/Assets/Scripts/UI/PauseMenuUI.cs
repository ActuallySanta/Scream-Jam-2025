using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private UIDocument uiDoc;

    private VisualElement rootElement;
    private Button resumeButton;
    private Button mainMenuButton;
    private Button quitButton;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.instance.OnPause += ToggleVisibility;
        rootElement = uiDoc.rootVisualElement;

        resumeButton = rootElement.Q<Button>("ResumeGameButton");
        mainMenuButton = rootElement.Q<Button>("MainMenuButton");
        quitButton = rootElement.Q<Button>("QuitButton");

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
        //SceneManager.LoadScene("");
    }

    private void QuitGame()
    {
        Debug.Log("Quitting!");
        Application.Quit();
        
        // can replace w diff quitting mechanism if needbe
    }
}
