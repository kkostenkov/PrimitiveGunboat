using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudScreen : UiScreen
{
    [SerializeField]
    private TextMeshProUGUI topScoreCounter;
    [SerializeField]
    private TextMeshProUGUI currentScore;
    [SerializeField]
    private Image projectileCooldown;
    [SerializeField]
    private TextMeshProUGUI projectileCommandsCounter;

    [SerializeField]
    private List<GameObject> healthMarks;
    private ISessionEventsProvider sessionEvents;

    internal void Initialize(ISessionEventsProvider sessionEvents)
    {
        this.sessionEvents = sessionEvents;
        sessionEvents.HealthChanged += OnHealthChange;
        sessionEvents.ScoreChanged += OnScoreChange;
    }

    public void Reset(int topScore, int hp)
    {
        topScoreCounter.text = topScore.ToString();
        currentScore.text = "0";
        projectileCommandsCounter.text = "0";
        projectileCooldown.fillAmount = 1f;
        OnHealthChange(hp);
    }

    private void OnHealthChange(int currentHealth)
    {
        for (int i = 0; i < healthMarks.Count; i++)
        {
            healthMarks[i].gameObject.SetActive(i < currentHealth);
        }
    }

    private void OnScoreChange(int score)
    {
        currentScore.text = score.ToString();
    }
}
