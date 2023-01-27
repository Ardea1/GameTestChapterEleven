using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceTrigger : MonoBehaviour
{
    // Список целевых объектов, которые
    // будет активировать данный триггер.
    [SerializeField]
    GameObject[] targets;

    [SerializeField]
    public bool requireKey;

    // Метод OnTriggerEnter() вызывается при попадании
    // объекта в зону триггера.
    private void OnTriggerEnter(Collider other)
    {
        // Условная инструкция для поиска
        // подготовленного к использованию ключа.
        if (requireKey && Managers.Inventory.equippedItem != "key")
        {
            return;
        }

        foreach (GameObject target in targets)
        {
            target.SendMessage("Activate");
        }
    }

    // Метод OnTriggerExit() вызывается при выходе
    // из зоны триггера.
    private void OnTriggerExit(Collider other)
    {
        foreach (GameObject target in targets)
        {
            target.SendMessage("Deactivate");
        }
    }
}
