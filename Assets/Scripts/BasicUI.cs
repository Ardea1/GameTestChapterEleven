using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Сценарий, отображающий инвентарь.
public class BasicUI : MonoBehaviour
{
    private void OnGUI()
    {
        int posX = 10;
        int posY = 10;
        int width = 100;
        int height = 30;
        int buffer = 10;

        List<string> itemList = Managers.Inventory.GetItemList();

        // Отображает сообщение, информирующее
        // об отсутствии инвентаря.
        if (itemList.Count == 0)
        {
            GUI.Box(new Rect(posX, posY, width, height), "No Items");
        }
        foreach (string item in itemList)
        {
            int count = Managers.Inventory.GetItemCount(item);

            // Метод, загружающий ресурсы из папки Resources.
            Texture2D image = Resources.Load<Texture2D>("Icons/" + item);

            GUI.Box(new Rect(posX, posY, width, height), new GUIContent("(" + count + ")", image));

            // При каждом прохождении цикла
            // сдвигаемся в сторону.
            posX += width + buffer;
        }

        string equipped = Managers.Inventory.equippedItem;

        // Отображение подготовленного элемента.
        if (equipped != null)
        {
            posX = Screen.width - (width + buffer);
            Texture2D image = Resources.Load("Icons/" + equipped) as Texture2D;
            GUI.Box(new Rect(posX, posY, width, height), new GUIContent("Equipped", image));
        }

        posX = 10;
        posY += height + buffer;

        // Просмотр всех элементов в цикле
        // для создания кнопок.
        foreach (string item in itemList)
        {
            // Запуск вложенного кода при щелчке на кнопке.
            if (GUI.Button(new Rect(posX, posY, width, height), "Equip " + item))
            {
                Managers.Inventory.EquipItem(item);
            }

            // Новый код для здоровья.
            if (item == "health")
            {
                // Запуск вложенного кода при щелчке на кнопке.
                if (GUI.Button(new Rect(posX, posY + height + buffer, width, height), "Use Health"))
                {
                    Managers.Inventory.ConsumeItem("health");
                    Managers.Player.ChangeHealth(25);
                }
            }
            posX += width + buffer;
        }
    }
}
