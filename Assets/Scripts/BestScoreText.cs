using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class BestScoreText : MonoBehaviour
{
    private TextMeshProUGUI _textMesh;

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();

        UpdateText();
    }

    private void OnEnable()
    {
        Context.Instance.StateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        Context.Instance.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(GameState state)
    {
        if(state == GameState.Title || state == GameState.GameOver)
            UpdateText();
    }

    private void UpdateText()
    {
        _textMesh.text = "Best Score: " + Context.Instance.GetBestScore();
    }
}