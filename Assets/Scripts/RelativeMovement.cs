using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Окружающие строки показывают контекст
// размещения метода RequireComponent().

// Метод RequireComponent() заставляет Unity
// проверять наличие у объекта GameObject
// компонента переданного в команду типа.
[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour
{
    // Сценарию нужна ссылка на объект,
    // относительно которого будет происходить перемещение.
    [SerializeField]
    Transform target;

    public float rotSpeed = 15.0f;
    public float moveSpeed = 6.0f;

    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;

    // Величина прилагаемой силы.
    public float pushForce = 3.0f;

    private float _vertSpeed;

    private CharacterController _charController;

    private Animator _animator;

    // Нужно для сохранения данных о столкновении
    // между функциями.
    private ControllerColliderHit _contact;

    private void Start()
    {
        // Инициализируем скорость по вертикали,
        // присваивая ей минимальную скорость
        // падения в начале существующей функции.
        _vertSpeed = minFall;

        // Используется для доступа к другим компонентам.
        _charController = GetComponent<CharacterController>();

        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Начинаем с вектора (0, 0, 0), непрерывно
        // добавляя компоненты движения.
        Vector3 movement = Vector3.zero;

        float horInput = Input.GetAxis("Horizontal");
        float verInput = Input.GetAxis("Vertical");

        // Движение обрабатывается только
        // при нажатии клавиш со стрелками.
        if (horInput != 0 || verInput != 0)
        {
            movement.x = horInput * moveSpeed;
            movement.z = verInput * moveSpeed;
            // Ограничиваем движение по диагонали
            // той же скоростью, что и движение вдоль оси.
            movement = Vector3.ClampMagnitude(movement, moveSpeed);

            // Сохраняем начальную ориентацию, чтобы
            // вернуться к ней после завершения работы
            // с целевым объектом.
            Quaternion tmp = target.rotation;

            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);

            // Преобразуем направление движения
            // из локальных в глобальные координаты.
            movement = target.TransformDirection(movement);

            target.rotation = tmp;

            // Метод LookRotation() вычисляет кватернион,
            // смотрящий в этом направлении.
            Quaternion direction = Quaternion.LookRotation(movement);

            // Метод Quaternion.Lerp()(linear interpolation) выполняет плавный поворот
            // из текущего положения в целевое (третий параметр контролирует
            // скорость вращения).
            // Плавный переход от отдного значения к другому,
            // называется интерполяцией.
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
        }

        _animator.SetFloat("Speed", movement.sqrMagnitude);

        bool hitGround = false;
        RaycastHit hit;

        // Проверяем, падает лди персонаж.
        if (_vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            // Расстояние, с которым производится сравнение
            // (слегка выходит за нижнюю часть капсулы).
            // Берём высоту контроллера персонажа (его рост без скруглённых углов)
            // и добавляем в ней скруглённые углы. Полученное значение делится
            // пополам, так как луч бросается из центра персонажа и проходит
            // расстояние до его стоп. Но мы хотим проверить чуть более
            // длинную дистанцию, чтобы учесть неболоьшие неточности
            // бросания луча, поэтому высота персонажа делатся на 1,9, а не на 2,
            // в итоге, луч проходит несколько большее расстояние.
            float check = (_charController.height + _charController.radius) / 1.9f;
            hitGround = hit.distance <= check;
        }

        // Свойство isGrounded компонента CharacterController
        // проверяет соприкасается ли контроллер
        // с поверхностью.

        // Вместо проверки свойства isGrounded
        // смотрим на результат бросания луча.
        if (hitGround)
        {
            // Реакция на кнопку Jump при нахождении
            // на поверхности.
            if (Input.GetButtonDown("Jump"))
            {
                _vertSpeed = jumpSpeed;
            }
            else
            {
                _vertSpeed = minFall; //-0.1f;
                _animator.SetBool("Jumping", false);
            }
        }
        // Если персонаж не стоит на поверхности, применяем
        // гравитацию, пока не будет достигнута предельная скорость.
        else
        {
            _vertSpeed += gravity * 5 * Time.deltaTime;

            if (_vertSpeed < terminalVelocity)
            {
                _vertSpeed = terminalVelocity;
            }
            // Не вводите в действие это значение
            // в самом начале уровня.
            if (_contact != null)
            {
                _animator.SetBool("Jumping", true);
            }
            // Метод бросания луча не обнаруживает поверхности,
            // но капсула с ней соприкасается.
            if (_charController.isGrounded)
            {
                // Реакция слегка меняется в зависимости от того,
                // смотрит ли персонаж в сторону точки контакта. 
                if (Vector3.Dot(movement, _contact.normal) < 0)
                {
                    movement = _contact.normal * moveSpeed;
                }
                else
                {
                    movement += _contact.normal * moveSpeed;
                }
            }
        }

        movement.y = _vertSpeed;

        movement *= Time.deltaTime;
        _charController.Move(movement);
    }

    // При распознавании столкновения данные этого
    // столкновения сохраняются в методе обратного вызова.
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contact = hit;

        // Проверка, есть ли у участвующего в столкновении
        // объекта компонень Rigidbody, обеспечивающий
        // реакцию на приложенную силу.
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body != null && !body.isKinematic)
        {
            // Назначение физическому телу скорости.
            body.velocity = hit.moveDirection * pushForce;
        }
    }
}
