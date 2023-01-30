using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPopup : MonoBehaviour
{
    // Массивы для ссылки на четыре
    // изображения и текстовые метки.
    [SerializeField]
    Image[] itemIcons;

    [SerializeField]
    Text[] itemLabels;

    [SerializeField]
    Text curItemLabel;

    [SerializeField]
    Button equipButton;

    [SerializeField]
    Button useButton;

    private string _curItem;

    public void Refresh()
    {
        List<string> itemList = Managers.Inventory.GetItemList();

        int len = itemIcons.Length;

        for (int i = 0; i < len; i++)
        {
            // Проверка списка инвентаря в процессе
            // циклического просмотра элементов UI.
            if (i < itemList.Count)
            {
                itemIcons[i].gameObject.SetActive(true);
                itemLabels[i].gameObject.SetActive(true);
                string item = itemList[i];

                // Загрузка спрайта из папки Resources.
                Sprite sprite = Resources.Load<Sprite>("Icons/" + item);
                itemIcons[i].sprite = sprite;
                // Изменение размеров изображения под исходный размер спрайта.
                itemIcons[i].SetNativeSize();

                int count = Managers.Inventory.GetItemCount(item);
                string message = "x" + count;
                if (item == Managers.Inventory.equippedItem)
                {
                    // На метке может появиться не только количество
                    // элементов, но и слово "Equipped".
                    message = "Equipped\n" + message;
                }

                itemLabels[i].text = message;

                EventTrigger.Entry entry = new EventTrigger.Entry();
                // Превращаем значки в интерактивные объекты.
                entry.eventID = EventTriggerType.PointerClick;

                // Лямбда-функция, позволяющая по-разному
                // активировать каждый элемент.
                entry.callback.AddListener((BaseEventData data) => {
                    OnItem(item);
                });

                EventTrigger trigger = itemIcons[i].GetComponent<EventTrigger>();

                // Сброс подписчика, чтобы начать с чистого листа.
                trigger.triggers.Clear();

                // Добавление функции-подписчика к классу  EventTrigger.
                trigger.triggers.Add(entry);
            }
            else
            {
                // Скрываем изображение/текст при отсутствии
                // элементов для отображения.
                itemIcons[i].gameObject.SetActive(false);
                itemLabels[i].gameObject.SetActive(false);
            }
        }
        if (!itemList.Contains(_curItem))
        {
            _curItem = null;
        }

        // Скрываем кнопки при отсутствии выделенных элементов.
        if (_curItem == null)
        {
            curItemLabel.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(false);
        }
        // Отображение выделенного в данный момент элемента.
        else
        {
            curItemLabel.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(true);
            if (_curItem == "health")
            {
                useButton.gameObject.SetActive(true);
            }
            else
            {
                useButton.gameObject.SetActive(false);
            }
            curItemLabel.text = _curItem + ":";
        }
    }

    // Функция, вызываемая подписчиком события щелчка мыши.
    public void OnItem(string item)
    {
        _curItem = item;

        // Актуализируем изображение инвентаря
        // после внесения изменений.
        Refresh();
    }

    public void OnEquip()
    {
        Managers.Inventory.EquipItem(_curItem);
        Refresh();
    }

    public void OnUse()
    {
        Managers.Inventory.ConsumeItem(_curItem);
        if (_curItem == "health")
        {
            Managers.Player.ChangeHealth(25);
        }
        Refresh();
    }
}
