using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameManager
{
    private NetworkService _network;

    // Свойство читается откуда угодно,
    // нозадаётся только в этом сценариии.
    public ManagerStatus status { get; private set; }

    public int health { get; private set; }
    public int maxHealth { get; private set; }

    public void Startup(NetworkService service)
    {
        Debug.Log("Player manager starting. . .");

        _network = service;

        // Эти значения могут быть инициализированы
        // сохранёнными данными.
        health = 50;
        maxHealth = 100;

        status = ManagerStatus.Started;
    }

    // Другие сценарии не могут напрямую
    // задавать переменную health, но могут
    // вызывать эту функцию.
    public void ChangeHealth(int value)
    {
        health += value;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health < 0)
        {
            health = 0;
        }
        Debug.Log("Health: " + health + "/" + maxHealth);
    }
}
