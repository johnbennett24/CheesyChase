using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Context : MonoBehaviour
{
    private const string ScorePrefsKey = "BestScore";

    public event Action<GameState> StateChanged;
    public event Action<int> ScoreIncreased;

    private static Context _instance;

    public static Context Instance => _instance;

    private AudioSource _audioSource;

    public GameState State { get; private set; }

    public int Score { get; private set; }

    public bool IsPlaying => State == GameState.Playing;

    public void PlaySoundOneShot(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

    public void ShowTitleScreen()
    {
        SetGameState(GameState.Title);
    }

    public void StartGame()
    {
        ResetScore();
        SetGameState(GameState.Playing);
    }

    public void FinishGame()
    {
        TryWriteBestScore();
        SetGameState(GameState.GameOver);
    }

    public void IncreaseScore()
    {
        Score++;
        ScoreIncreased?.Invoke(Score);
    }

    public void ResetScore()
    {
        Score = 0;
    }

    public int GetBestScore()
    {
        return PlayerPrefs.GetInt(ScorePrefsKey, 0);
    }

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);

            throw new System.Exception("Theres already another instance on scene");
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        _audioSource = GetComponent<AudioSource>();
    }

    private void SetGameState(GameState state)
    {
        State = state;
        StateChanged?.Invoke(State);
    }

    private void TryWriteBestScore()
    {
        int bestScore = PlayerPrefs.GetInt(ScorePrefsKey, 0);

        if (bestScore < Score)
            PlayerPrefs.SetInt(ScorePrefsKey, Score);
    }
}