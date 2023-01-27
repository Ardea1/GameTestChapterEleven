using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : MonoBehaviour
{
    // Эти два поля добавляются перд любыми методами
    // как и в сценарии SceneController.
    [SerializeField]
    GameObject fireballPrefab;

    GameObject _fireball;

    // Базовая скорость, меняемая в соответствии
    // с положением ползунка.
    [SerializeField]
    const float baseSpeed = 3.0f;

    // Значения для скорости движения и расстояния,
    // с которого мначинается реакция на препятствие.
    [SerializeField]
    float speed = 3.0f;

    // Расстояние, на котором враг начинает реагировать
    // на препятствия.
    [SerializeField]
    float obstacleRange = 5.0f;

    // Логическая переменная для отслеживания
    // состояния персонажа.
    bool _alive;

    private void Start()
    {
        // Инициализация этой переменной.
        _alive = true;
    }

    // Объявляем какой метод отвечает на событие SPEED_CHANGED.
    private void Awake()
    {
        Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    // Удаляем подписчика.
    private void OnDestroy()
    {
        Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    // Метод, объявленный в подписчике для события SPEED_CHANGED.
    private void OnSpeedChanged(float value)
    {
        speed = baseSpeed * value;
    }

    // Открытый метод, позволяющий внешнему коду
    // воздействовать на "живое" состояние.
    public void SetAlive(bool alive)
    {
        _alive = alive;
    }

    private void Update()
    {
        if (_alive)
        {
            // Непрерывно движемся вперёд в каждом кадре, несмотря на повороты.
            transform.Translate(0, 0, speed * Time.deltaTime);

            // Луч находится в том же положении и нацеливается в том
            // же направлении, что и персонаж.
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            // Бросаем луч с описанной вокруг него окружностью.
            if (Physics.SphereCast(ray, 0.75f, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;

                // Игрок распознаётся тем же способом,
                // что и мишень в сценарпии RayShooter.
                if (hitObject.GetComponent<PlayerCharacter>())
                {
                    if (_fireball == null)
                    {
                        _fireball = Instantiate(fireballPrefab) as GameObject;
                        // Поместим огненный шар перед врагом и нацелим в направлении его движения.
                        _fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                        _fireball.transform.rotation = transform.rotation;
                    }
                }
                else if (hit.distance < obstacleRange)
                {
                    // Поворот с наполовину случайным выбором нового направления.
                    float angle = Random.Range(-110, 110);
                    transform.Rotate(0, angle, 0);
                }
            }
        }
    }
}
