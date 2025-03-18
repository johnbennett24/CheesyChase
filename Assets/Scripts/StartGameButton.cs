using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartGameButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(Context.Instance.StartGame);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Context.Instance.StartGame);
    }
}
