using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    // Введите имя этого элемента
    // на панели Inspector.
    [SerializeField]
    string itemName;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Item collected: " + itemName);

        Managers.Inventory.AddItem(name);

        Destroy(this.gameObject);
        /* Метод.Destroy().должен.вызываться.для.параметра.this.gameObject,.а.не.this!.Не.
        путайте.эти.вещи;.ключевое.слово.this.ссылается.только.на.компонент.сценария,.в.то.время.как.вы-
        ражение.this.gameObject.ссылается.на.объект,.к.которому.присоединен.сценарий. */
    }
}
