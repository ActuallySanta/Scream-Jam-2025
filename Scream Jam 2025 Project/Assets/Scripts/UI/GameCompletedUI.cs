using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameCompletedUI : MonoBehaviour
{
    [SerializeField] private UIDocument uiDoc;

    private Button mainMenuButton;
    private Button quitButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VisualElement root = uiDoc.rootVisualElement;

        mainMenuButton = root.Q<Button>("mainMenuButton");
        quitButton = root.Q<Button>("quitButton");

        mainMenuButton.clicked += GoToMainMenu;
        quitButton.clicked += () => Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
