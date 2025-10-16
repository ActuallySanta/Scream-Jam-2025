using UnityEngine;
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

    private void TriggerPause()
    {
        Debug.Log("ts should pause");
        GameManager.instance.TogglePause();
    }
}
