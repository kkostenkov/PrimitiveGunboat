using System;


public class SessionEvents : ISessionEventsListener, ISessionEventsProvider
{
    public event Action<int> ScoreChanged;
    public void ScoreSet(int score)
    {
        ScoreChanged?.Invoke(score);
    }

    public event Action<int> HealthChanged;
    public void HealthSet(int health)
    {
        HealthChanged?.Invoke(health);
    }


}

public interface ISessionEventsProvider
{
    event Action<int> HealthChanged;
    event Action<int> ScoreChanged;
}

public interface ISessionEventsListener
{
    void HealthSet(int health);
    void ScoreSet(int score);
}