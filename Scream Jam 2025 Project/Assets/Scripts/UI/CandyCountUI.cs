using UnityEngine;
using UnityEngine.UIElements;
public class CandyCountUI : MonoBehaviour
{
    [SerializeField] private UIDocument uiDoc;

    private Label scoreLabel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VisualElement root = uiDoc.rootVisualElement;

        scoreLabel = root.Q<Label>("CandyCount");
        UpdateLabel();

        GameManager.instance.OnAddCandy += UpdateLabel;
    }

    private void UpdateLabel()
    {
        scoreLabel.text = $"Candy Collected: {GameManager.instance.TotalCandyAccquired + 1}";
    }
}
