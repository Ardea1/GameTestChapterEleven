using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Окружающие строки показывают контекст
// размещения метода RequireComponent().

// Метод RequireComponent() заставляет Unity
// проверять наличие у объекта GameObject
// компонента переданного в команду типа.
[RequireComponent(typeof(CharacterController))]
public class PointClickMovement : MonoBehaviour
{
    // Сценарию нужна ссылка на объект,
    // относительно которого будет происходить перемещение.
    [SerializeField]
    Transform target;

	public float moveSpeed = 6.0f;
	public float rotSpeed = 15.0f;
	public float jumpSpeed = 15.0f;
	public float gravity = -9.8f;
	public float terminalVelocity = -20.0f;
	public float minFall = -1.5f;
	public float pushForce = 3.0f;

	public float deceleration = 25.0f;
	public float targetBuffer = 1.5f;
	private float _curSpeed = 0f;
	private Vector3 _targetPos = Vector3.one;

	private float _vertSpeed;
	// Нужно для сохранения данных о столкновении
	// между функциями.
	private ControllerColliderHit _contact;

	private CharacterController _charController;
	private Animator _animator;

	// Use this for initialization
	void Start()
	{
		// Инициализируем скорость по вертикали,
		// присваивая ей минимальную скорость
		// падения в начале существующей функции.
		_vertSpeed = minFall;

		// Используется для доступа к другим компонентам.
		_charController = GetComponent<CharacterController>();

		_animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		// Начинаем с вектора (0, 0, 0), непрерывно
		// добавляя компоненты движения.
		Vector3 movement = Vector3.zero;

		// Задаём целевую точку
		// по щелчку мыши.
		if (Input.GetMouseButton(0) /*&& !EventSystem.current.IsPointerOverGameObject()*/)
		{
			// Искускаем луч в точку
			// щелчка мышью.
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit mouseHit;
			if (Physics.Raycast(ray, out mouseHit))
			{
				//GameObject hitObject = mouseHit.transform.gameObject;
				//if (hitObject.layer == LayerMask.NameToLayer("Ground"))
				//{
					// Устанавливаем цель в точке
					// падения луча.
					_targetPos = mouseHit.point;
					_curSpeed = moveSpeed;
				//}
			}
		}

		// Перемещаем при заданной целевой точке.
		if (_targetPos != Vector3.one)
		{
			//if (_curSpeed > moveSpeed * .5f)
			//{ 
				// Блокировка вращения при остановке
				Vector3 adjustedPos = new Vector3(_targetPos.x, transform.position.y, _targetPos.z);
				Quaternion targetRot = Quaternion.LookRotation(adjustedPos - transform.position);
				// Поворачиваем по направлению к цели.
				// Метод Quaternion.Slerp() выполняет поворот плавно.
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
			//}

			movement = _curSpeed * Vector3.forward;
			movement = transform.TransformDirection(movement);

			if (Vector3.Distance(_targetPos, transform.position) < targetBuffer)
			{
				// Снижаем скорость до нуля,
				// при приближении к цели.
				_curSpeed -= deceleration * Time.deltaTime;
				if (_curSpeed <= 0)
				{
					_targetPos = Vector3.one;
				}
			}
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

	// store collision to use in Update
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		_contact = hit;

		Rigidbody body = hit.collider.attachedRigidbody;
		if (body != null && !body.isKinematic)
		{
			body.velocity = hit.moveDirection * pushForce;
		}
	}
}

