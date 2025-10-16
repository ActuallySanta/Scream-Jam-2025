using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenuController : MonoBehaviour
{
    public UIDocument uiDocument;
    ///public AudioMixer audioMixer;

    private VisualElement root;

    void Start()
    {
        root = uiDocument.rootVisualElement;

        // Main Menu Buttons
        root.Q<Button>("playButton").clicked += () => SceneManager.LoadScene("Game");
        root.Q<Button>("loadButton").clicked += ShowPasswordPrompt;
        root.Q<Button>("howToButton").clicked += () => ShowInfo("How to Play", "A and D, Left and Right Arrow Keys: Move left and right\nSpacebar: Jump\nLeft Click or J: Throw Skull\nR or K: Respawn\nP or Escape: Pause Game");

        //TODO: Update credits
        root.Q<Button>("creditsButton").clicked += () => ShowInfo("Credits", "Project Lead: Santiago Benitez-Fernandez\nMain Artist: Taylor Fraizer\nProgrammers: Cooper Fleishman, Kyle Benjamin, Emma Duprey, Santiago Benitez-Fernandez\nMain Level Designer: Kyle Benjamin\nUI Assets from BarelyApes\nBackground Music from Dark Fantasy Studio");
        //root.Q<Button>("quitButton").clicked += Application.Quit;

        // Settings Sliders
        var gameSlider = root.Q<Slider>("gameVolume");
        var musicSlider = root.Q<Slider>("musicVolume");

        if (gameSlider != null)
        {
            //gameSlider.RegisterValueChangedCallback(evt =>
                //audioMixer.SetFloat("GameVolume", Mathf.Log10(Mathf.Max(evt.newValue, 0.0001f)) * 20));
        }

        if (musicSlider != null)
        {
            //musicSlider.RegisterValueChangedCallback(evt =>
                //audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(evt.newValue, 0.0001f)) * 20));
        }

        // Return Buttons
        //root.Q<Button>("backButton")?.clicked += () => TogglePanel("mainMenu");
    }

    void TogglePanel(string panelName)
    {
        foreach (var panel in root.Children())
        {
            panel.style.display = DisplayStyle.None;
        }

        var targetPanel = root.Q<VisualElement>(panelName);
        if (targetPanel != null)
        {
            targetPanel.style.display = DisplayStyle.Flex;
        }
    }

    void ShowInfo(string title, string content)
    {
        var overlay = root.Q<VisualElement>("infoOverlay");
        var titleLabel = root.Q<Label>("infoTitle");
        var contentLabel = root.Q<Label>("contentText");

        titleLabel.text = title;
        contentLabel.text = content;

        overlay.style.display = DisplayStyle.Flex;

        root.Q<Button>("backButton").clicked += () =>
        {
            overlay.style.display = DisplayStyle.None;
        };
    }

    void ShowPasswordPrompt()
    {
        // Stubbed logic — you can replace this with a password input field and validation
        Debug.Log("Password prompt triggered. Implement your password logic here.");
    }
}