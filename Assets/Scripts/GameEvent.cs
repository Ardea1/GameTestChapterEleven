using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Этот сценарий задаст константу для пары сообщений о событиях,
// что позволяет систематизировать сообщения, одновременно избавляя
// вас от необходимости вводить строку сообщения в разных местах.
public class GameEvent : MonoBehaviour
{
    public const string ENEMY_HIT = "ENEMY_HIT";
    public const string SPEED_CHANGED = "SPEED_CHANGED";
    public const string WEATHER_UPDATED = "WEATHER_UPDATED";
}
