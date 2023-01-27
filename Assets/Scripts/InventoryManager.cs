using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager
{
    private NetworkService _network;

    // Свойство читается откуда угодно,
    // нозадаётся только в этом сценариии.
    public ManagerStatus status { get; private set; }

    public string equippedItem { get; private set; }

    private Dictionary<string, int> _items;

    public void Startup(NetworkService service)
    {
        // Сюда идут все задачи запуска с долгим
        // временем выполнения.
        Debug.Log("Inventory manager starting. . .");

        _network = service;

        // Инициализируем пустой список элементов.
        _items = new Dictionary<string, int>();

        // Для задач с долгим временем выполнения
        // используем состояние "Initializing".
        status = ManagerStatus.Started;
    }

    // Возвращаем список всех ключей словаря.
    public List<string> GetItemList()
    {
        List<string> list = new List<string>(_items.Keys);
        return list;
    }

    // Возвращаем количество указанных элементов
    // в инвентаре.
    public int GetItemCount(string name)
    {
        if (_items.ContainsKey(name))
        {
            return _items[name];
        }
        return 0;
    }

    // Вывод на консоль сообщения о текущем
    // инвентаре.
    private void DisplayItems()
    {
        string itemDisplay = "Items: ";

        foreach (KeyValuePair<string, int> item in _items)
        {
            itemDisplay += item.Key + "(" + item.Value + ") ";
        }
        Debug.Log(itemDisplay);
    }

    // Другие сценарии не могут напрямую управлять
    // списком элементов, но могут вызывать этот метод.
    public void AddItem(string name)
    {
        // Проверка существующих записей перед
        // вводом новых данных.

        // Если перед нами новый элемент, счет начинается
        // с единицы, если же такой элемент уже
        // существует, на единицу увеличивается сохраненное значение.
        if (_items.ContainsKey(name))
        {
            _items[name] += 1;
        }
        else
        {
            _items[name] = 1;
        }

        DisplayItems();
    }

    public bool EquipItem(string name)
    {
        // Проверяем наличие в инвентаре указанного
        // элемента и тот факт, что он ещё не подготовлен
        // к использованию.
        if (_items.ContainsKey(name) && equippedItem != name)
        {
            equippedItem = name;
            Debug.Log("Equipped " + name);
            return true;
        }
        equippedItem = null;
        Debug.Log("Unequipped");
        return false;
    }

    // Проверяет наличие указанного элемента в инвентаре
    // и уменьшает значение, если таковой не обнаружен. 
    public bool ConsumeItem(string name)
    {
        // Проверка наличия элемента среди инвентаря.
        if (_items.ContainsKey(name))
        {
            _items[name]--;
            // Удаление записи, если количество становится
            // равным нулю.
            if (_items[name] == 0)
            {
                _items.Remove(name);
            }
        }
        // Реакция с лучае отсутствия в инвентаре
        // нужного элемента.
        else
        {
            Debug.Log("Cannot consume " + name);
            return false;
        }
        DisplayItems();
        return true;
    }
}
