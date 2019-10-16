using System;

public interface IDamageTaker
{
    void TakeDamage(int amount);
    event Action<IDamageTaker> Died;
}