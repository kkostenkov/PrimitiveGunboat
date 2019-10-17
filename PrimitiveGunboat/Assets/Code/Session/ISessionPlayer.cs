using System;


public interface ISessionPlayer
{
    event Action GameOver;
    void Play();

    ISessionEventsProvider SessionEventsProvider { get; }

    int TopScore { get; }
    int CurrentHealth { get; }
}
