using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDevice : MonoBehaviour
{
    public float radius = 3.5f;

    private void OnMouseDown()
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        // Проверяем расстояние до персонажа.
        if (Vector3.Distance(player.position, transform.position) < radius)
        {
            Vector3 direction = transform.position - player.position;
            // С помощью скалярного произведения определяем,
            // повёрнут ли персонаж в сторону устройства.
            if (Vector3.Dot(player.forward, direction) > .5f)
            {
                // Вызов метода Operate(), если персонаж
                // находится рядом и повернут лицом
                // к устройству.
                Operate();
            }
        }
    }

    // Ключевое слово virtual указывает на метод,
    // который можно переопределить после наследования.
    public virtual void Operate()
    {
        // поведение конкретного устройства
    }
}

/* Скалярное произведение (Dot Product)
Скалярное произведение получает 2 вектора и возвращает скаляр.
Этот скаляр равен произведению величин этих векторов, умноженному
на косинус угла между ними. Когда оба вектора - нормированные,
косинус по сути дела утверждает, как далеко первый вектор простирается
в направлении второго (или наоборот - порядок параметров роли не играет). */