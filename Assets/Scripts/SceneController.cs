using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Порождает экземпляры врагов.
public class SceneController : MonoBehaviour
{
    // Сериализованная переменная для связи
    // с объектом-шаблоном.
    [SerializeField]
    GameObject enemyPrefab;

    //// Базовая скорость, меняемая в соответствии
    //// с положением ползунка.
    //[SerializeField]
    //const float baseSpeed = 3.0f;

    //// Значения для скорости движения и расстояния,
    //// с которого мначинается реакция на препятствие.
    //[SerializeField]
    //float speed = 3.0f;

    // Закрытая переменная для слежения за экземпляром врага в сцене.
    GameObject _enemy;

    //// Объявляем какой метод отвечает на событие SPEED_CHANGED.
    //private void Awake()
    //{
    //    Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    //}

    //// Удаляем подписчика.
    //private void OnDestroy()
    //{
    //    Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    //}

    //// Метод, объявленный в подписчике для события SPEED_CHANGED.
    //private void OnSpeedChanged(float value)
    //{
    //    speed = baseSpeed * value;
    //}

    // Порождаем нового врага, только если враги в сцене отсутствуют.
    void Update()
    {
        if (_enemy == null)
        {
            // Метод, копирующий объект-шаблон.
            _enemy = Instantiate(enemyPrefab) as GameObject;
            _enemy.transform.position = new Vector3(0, 2, 0);
            float angle = Random.Range(0, 360);
            _enemy.transform.Rotate(0, angle, 0);
        }
    }
}
