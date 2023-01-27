using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Ссылка на UI-объект в сцене.
    [SerializeField]
    Text healthLabel;

    [SerializeField]
    InventoryPopup popup;

    // Задаём подписчика для события обновления здоровья.
    private void Awake()
    {
        Messenger.AddListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
    }

    // Подписчик события вызывает функцию для обновления метки health.
    private void OnHealthUpdated()
    {
        string message = "Health: " + Managers.Player.health + "/" +
        Managers.Player.maxHealth;
        healthLabel.text = message;
    }

    private void Start()
    {
        // Вызов функции вручную при загрузке.
        OnHealthUpdated();

        // Всплывающее окно инициализируется
        // как скрытое.
        popup.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Отображение всплывающего окна
        // нажатием ткнопки М.
        if (Input.GetKeyDown(KeyCode.M))
        {
            bool isShowing = popup.gameObject.activeSelf;
            popup.gameObject.SetActive(!isShowing);
            popup.Refresh();
        }    
    }
}
