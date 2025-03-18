using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class GameStateCanvas : MonoBehaviour
{
    [SerializeField] private GameState _state;
    [SerializeField] private float _fadeInTime = 0.5f;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        if (Context.Instance.State != _state)
            FadeOutCanvas();
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
        if(state != _state)
        {
            StopAllCoroutines();

            FadeOutCanvas();
        }
        else
        {
            StartCoroutine(FadeInCanvasCoroutine());
        }
    }

    private IEnumerator FadeInCanvasCoroutine()
    {
        while(_canvasGroup.alpha < 1)
        {
            _canvasGroup.alpha += (1 / _fadeInTime) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    private void FadeOutCanvas()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}
