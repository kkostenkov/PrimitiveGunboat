using System;
using UnityEngine;
using UnityEngine.UI;

public class IntroScreen : UiScreen
{
    public event Action PlayClicked;

    [SerializeField]
    private Button playButton;

    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayClicked);
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveAllListeners();
    }

    private void OnPlayClicked()
    {
        PlayClicked?.Invoke();
    }
}
