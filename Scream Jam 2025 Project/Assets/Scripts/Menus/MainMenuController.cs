using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenuController : MonoBehaviour
{
    public UIDocument uiDocument;
    public AudioMixer audioMixer;

    void Start()
    {
        var root = uiDocument.rootVisualElement;

        root.Q<Button>("playButton").clicked += () => SceneManager.LoadScene("Game");
        root.Q<Button>("loadButton").clicked += ShowPasswordPrompt;
        //settings may be a moot point
        root.Q<Button>("settingsButton").clicked += () => TogglePanel("settingsPanel");
        root.Q<Button>("howToButton").clicked += () => ShowInfo("How to Play", "Use arrows to move, Z to shoot...");
        root.Q<Button>("creditsButton").clicked += () => ShowInfo("Credits", "Game by... \n Assets used...");
        root.Q<Button>("quitButton").clicked += () => Application.Quit();

        //If we wanted to do music controls:
        //var gameSlider = root.Q<Slider>("gameVolume");
        //var musicSlider = root.Q<Slider>("musicVolume");

        //gameSlider?.RegisterValueChangedCallback(evt => audioMixer.SetFloat("GameVolume", Mathf.Log10(evt.newValue) * 20));
        //musicSlider?.RegisterValueChangedCallback(evt => audioMixer.SetFloat("MusicVolume", Mathf.Log10(evt.newValue) * 20));

        root.Q<Button>("backButton")?.RegisterCallback<ClickEvent>(evt => TogglePanel("mainMenu"));
    }

    void TogglePanel(string panelName)
    {
        foreach (var panel in uiDocument.rootVisualElement.Children())
            panel.style.display = DisplayStyle.None;

        uiDocument.rootVisualElement.Q<VisualElement>(panelName).style.display = DisplayStyle.Flex;
    }

    void ShowInfo(string title, string content)
    {
        TogglePanel("infoPanel");
        uiDocument.rootVisualElement.Q<Label>("infoTitle").text = title;
        uiDocument.rootVisualElement.Q<ScrollView>("infoText").Clear();
        uiDocument.rootVisualElement.Q<ScrollView>("infoText").Add(new Label(content));
    }

    void ShowPasswordPrompt()
    {
        // Add password logic here
    }
}