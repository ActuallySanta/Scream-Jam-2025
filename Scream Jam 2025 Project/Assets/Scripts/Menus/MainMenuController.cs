using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenuController : MonoBehaviour
{
    public UIDocument uiDocument;
    public AudioMixer audioMixer;

    private VisualElement root;

    void Start()
    {
        root = uiDocument.rootVisualElement;

        // Main Menu Buttons
        root.Q<Button>("playButton").clicked += () => SceneManager.LoadScene("Game");
        root.Q<Button>("loadButton").clicked += ShowPasswordPrompt;
        root.Q<Button>("howToButton").clicked += () => ShowInfo("How to Play", "Use arrow keys to move.\nPress Z to shoot.\nAvoid spikes!");
        root.Q<Button>("creditsButton").clicked += () => ShowInfo("Credits", "Game by Emma Duprey\nArt by PixelPal\nMusic by ChiptuneMaster");
        //root.Q<Button>("quitButton").clicked += Application.Quit;

        // Settings Sliders
        var gameSlider = root.Q<Slider>("gameVolume");
        var musicSlider = root.Q<Slider>("musicVolume");

        if (gameSlider != null)
        {
            gameSlider.RegisterValueChangedCallback(evt =>
                audioMixer.SetFloat("GameVolume", Mathf.Log10(Mathf.Max(evt.newValue, 0.0001f)) * 20));
        }

        if (musicSlider != null)
        {
            musicSlider.RegisterValueChangedCallback(evt =>
                audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(evt.newValue, 0.0001f)) * 20));
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
        var scrollView = root.Q<ScrollView>("infoText");

        titleLabel.text = title;
        scrollView.Clear();
        scrollView.Add(new Label(content));

        overlay.style.display = DisplayStyle.Flex;

        root.Q<Button>("closeInfo").clicked += () =>
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