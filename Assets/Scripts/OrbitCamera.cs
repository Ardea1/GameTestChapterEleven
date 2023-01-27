using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    // Ссылка на объект, вокруг которого производится облёт.
    [SerializeField]
    Transform target;

    // Скорость поворота камеры.
    public float rotSpeed = 1.5f;

    // Угол поворота.
    private float _rotY;

    // Положение камеры относительно цели.
    private Vector3 _offset;

    private void Start()
    {
        _rotY = transform.eulerAngles.y;

        // Сохранение начального смещения между
        // камерой и целью.
        // Переменна _offset содержит разность
        // в положении камеры и целевого объекта.
        _offset = target.position - transform.position;
    }

    private void LateUpdate()
    {
        // Меняем направление на обратное.
        _rotY -= Input.GetAxis("Horizontal") * rotSpeed;
        Quaternion rotation = Quaternion.Euler(0, _rotY, 0);
        transform.position = target.position - (rotation * _offset);
        transform.LookAt(target);
    }

    #region Старый метод private void LateUpdate()
    //private void LateUpdate()
    //{
    //    float horInput = Input.GetAxis("Horizontal");

    //    // Медленный поворот камеры при помощи клавиш
    //    // со стрелками.
    //    if (horInput != 0)
    //    {
    //        _rotY += horInput * rotSpeed;
    //    }
    //    else
    //    {
    //        // Или быстрый поворот с помощью мыши.
    //        _rotY += Input.GetAxis("Mouse X") * rotSpeed * 3;
    //    }

    //    Quaternion rotation = Quaternion.Euler(0, _rotY, 0);

    //    // Поддерживаем начальное смещение, сдвигаемое
    //    // в соответствии с поворотом камеры.
    //    // Умножаем вектор смещения на кватернион
    //    // для получения положения смещения после поворота.
    //    // Затем определяем положение камеры, вычитая смещение
    //    // после поворота из положения целевого объекта.

    //    // 1. Определяем положение камеры, чтобы выяснить её
    //    // смещение относительно персонажа.
    //    // 2. Умножаем вектор смещения на кватернион для
    //    // получения смещения в результате поворота.
    //    // 3. Вычитыаем их положения игрока, чтобы
    //    // определить смещение относительно игрока.
    //    transform.position = target.position - (rotation * _offset);

    //    // Камера всегда направлена на цель,
    //    // где бы относительно этой цели она
    //    // ни распологалась.
    //    transform.LookAt(target);

    //}
#endregion
}
