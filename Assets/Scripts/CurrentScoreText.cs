using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CurrentScoreText : MonoBehaviour
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
        if (state == GameState.GameOver)
            UpdateText();
    }

    private void UpdateText()
    {
        _textMesh.text = "Current Score: " + Context.Instance.Score;
    }
}