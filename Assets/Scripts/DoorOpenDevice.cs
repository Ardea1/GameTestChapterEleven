using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenDevice : MonoBehaviour
{
    // Смещение, применяемое при открывании двери.
    [SerializeField]
    Vector3 dPos;

    // Переменная типа Boolean для слежения
    // за открытым состоянием двери.
    bool _open;

    public void Operate()
    {
        // Открываем или закрываем дверь в зависимости
        // от ей состояния.
        if (_open)
        {
            Vector3 pos = transform.position - dPos;
            transform.position = pos;
        }
        else
        {
            Vector3 pos = transform.position + dPos;
            transform.position = pos;
        }
        _open = !_open;
    }

    public void Activate()
    {
        // Дверь открывается только если она
        // пока не открыта.
        if (!_open)
        {
            Vector3 pos = transform.position + dPos;
            transform.position = pos;
            _open = true;
        }
    }

    public void Deactivate()
    {
        // Закрывает дверь только при условии,
        // что она не закрыта.
        if (_open)
        {
            Vector3 pos = transform.position - dPos;
            transform.position = pos;
            _open = false;
        }
    }
}
