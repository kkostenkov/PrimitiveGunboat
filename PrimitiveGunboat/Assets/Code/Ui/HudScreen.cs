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

    private float launchTime; // DateTime
    private bool isCooldownActive = false;

    internal void Initialize(ISessionEventsProvider sessionEvents)
    {
        this.sessionEvents = sessionEvents;
        sessionEvents.HealthChanged += OnHealthChange;
        sessionEvents.ScoreChanged += OnScoreChange;
        sessionEvents.ProjectileFired += OnProjectileFired;
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

    private void OnProjectileFired()
    {
        if (Settings.LauncherCooldown == 0)
        {
            return;
        }
        launchTime = Time.time;
        projectileCooldown.fillAmount = 0;
        isCooldownActive = true;
    }

    private void Update()
    {
        if (!isCooldownActive)
        {
            return;
        }

        var cooldownEndTime = launchTime + Settings.LauncherCooldown;
        if (cooldownEndTime > Time.time)
        {
            var timePassed = cooldownEndTime - Time.time;
            var fract = timePassed / Settings.LauncherCooldown;
            projectileCooldown.fillAmount = 1 - fract;
        }
        else 
        {
            projectileCooldown.fillAmount = 1;
            isCooldownActive = false;
        }
        
    }
}
