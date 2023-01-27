using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceOperator : MonoBehaviour
{
    // Расстояние, с которого персонаж сможет активировать 
    // устройства.
    public float radius = 1.5f;

    private void Update()
    {
        // Реакция на кнопку ввода, заданную
        // в настройках ввода в Unity.
        if (Input.GetButtonDown("Fire3"))
        {
            // Метод OverlapSphere() возвращает список ближайщих объектов.
            // Передаём в него текущее местоположение персонажа и переменную
            // радиус.
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

            foreach (Collider hitCollider in hitColliders)
            {
                // Код, позволяющий открывать дверь, только стоя к ней лицом.
                // Мы вычитаем координаты игрока из координат объекта,
                // проверяя направление от персонажа к объекту.
                Vector3 direction = hitCollider.transform.position - transform.position;

                // Сообщение отправляется только при корректной
                // ориентации персонажа.
                if (Vector3.Dot(transform.forward, direction) > 0.5f)
                {
                    // Метод SendMessage() пытается вызвать именованную
                    // функцию независимо от типа целевого объекта.
                    hitCollider.SendMessage("Operate", SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }
}
