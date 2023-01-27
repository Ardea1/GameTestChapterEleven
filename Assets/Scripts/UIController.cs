using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // ������ �� UI-������ � �����.
    [SerializeField]
    Text healthLabel;

    [SerializeField]
    InventoryPopup popup;

    // ����� ���������� ��� ������� ���������� ��������.
    private void Awake()
    {
        Messenger.AddListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
    }

    // ��������� ������� �������� ������� ��� ���������� ����� health.
    private void OnHealthUpdated()
    {
        string message = "Health: " + Managers.Player.health + "/" +
        Managers.Player.maxHealth;
        healthLabel.text = message;
    }

    private void Start()
    {
        // ����� ������� ������� ��� ��������.
        OnHealthUpdated();

        // ����������� ���� ����������������
        // ��� �������.
        popup.gameObject.SetActive(false);
    }

    private void Update()
    {
        // ����������� ������������ ����
        // �������� ������� �.
        if (Input.GetKeyDown(KeyCode.M))
        {
            bool isShowing = popup.gameObject.activeSelf;
            popup.gameObject.SetActive(!isShowing);
            popup.Refresh();
        }    
    }
}
