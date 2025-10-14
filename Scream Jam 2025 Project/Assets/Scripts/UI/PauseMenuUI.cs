using UnityEngine;
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

        });

        if (rootElement.visible)
        {
            ToggleVisibility();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ToggleVisibility()
    {
        rootElement.visible = !rootElement.visible;
    }
}
