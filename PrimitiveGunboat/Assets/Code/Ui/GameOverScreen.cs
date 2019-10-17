using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class GameOverScreen : UiScreen, IPointerClickHandler
{
    [SerializeField]
    private TextMeshProUGUI additionalInfo;
    public event Action Clicked;

    private const string highscoreTemplate = "You've scored {0}";

    public void SetHighscore(int newScore)
    {
        if (additionalInfo)
        {
            additionalInfo.text = string.Format(highscoreTemplate, newScore);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Clicked?.Invoke();
    }

    private void OnDestroy()
    {
        Clicked = null;
    }
}
