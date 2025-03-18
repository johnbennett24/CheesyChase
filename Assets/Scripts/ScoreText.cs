using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreText : MonoBehaviour
{
    private TextMeshProUGUI _textMesh;

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
        _textMesh.text = 0.ToString();
    }

    private void OnEnable()
    {
        Context.Instance.StateChanged += OnStateChanged;
        Context.Instance.ScoreIncreased += OnScoreIncreased;
    }

    private void OnDisable()
    {
        Context.Instance.StateChanged -= OnStateChanged;
        Context.Instance.ScoreIncreased -= OnScoreIncreased;
    }

    private void OnStateChanged(GameState state)
    {
        if (state == GameState.Playing)
            _textMesh.text = 0.ToString();
    }

    private void OnScoreIncreased(int score)
    {
        _textMesh.text = score.ToString();
    }
}