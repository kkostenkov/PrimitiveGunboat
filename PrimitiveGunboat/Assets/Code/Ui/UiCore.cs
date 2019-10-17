
using UnityEngine;

public class UiCore : MonoBehaviour
{
    [SerializeField]
    private IntroScreen introScreen;
    [SerializeField]
    private GameOverScreen gameOverScreen;
    [SerializeField]
    private HudScreen hudScreen;
    private ISessionPlayer sessionPlayer;
    private ISessionEventsProvider sessionEvents;

    private UiState currentState;

    private void Awake()
    {
        gameOverScreen.Clicked += OnGameOverClicked;
        introScreen.PlayClicked += OnPlayClicked;
    }

    public void Initialize(ISessionPlayer sessionPlayer)
    {
        this.sessionPlayer = sessionPlayer;
        sessionPlayer.GameOver += () => SetState(UiState.GameOver);
        this.sessionEvents = sessionPlayer.SessionEventsProvider;
        hudScreen.Initialize(sessionEvents);
        SetState(UiState.Intro);
    }

    public void SetState(UiState state)
    {
        HideAllScreens();
        currentState = state;
        switch (currentState)
        {
            case UiState.Intro:
                introScreen.Show();
                break;
            case UiState.Session:
                hudScreen.Reset(sessionPlayer.TopScore, sessionPlayer.CurrentHealth);
                hudScreen.Show();
                break;
            case UiState.GameOver:
                gameOverScreen.SetHighscore(sessionPlayer.LastScore);
                gameOverScreen.Show();
                break;
        }
    }

    public void HideAllScreens()
    {
        introScreen.Hide();
        gameOverScreen.Hide();
        hudScreen.Hide();
    }


    private void OnPlayClicked()
    {
        sessionPlayer.Play();
        SetState(UiState.Session);
    }

    private void OnGameOverClicked()
    {
        if (currentState == UiState.GameOver)
        {
            SetState(UiState.Intro);
        }
    }

    private void OnDestroy()
    {
        gameOverScreen.Clicked -= OnGameOverClicked;
        introScreen.PlayClicked -= OnPlayClicked;
    }
}

public enum UiState
{
    Intro,
    Session,
    GameOver,
}