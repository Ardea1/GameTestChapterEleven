using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : MonoBehaviour
{
    // ��� ��� ���� ����������� ���� ������ ��������
    // ��� � � �������� SceneController.
    [SerializeField]
    GameObject fireballPrefab;

    GameObject _fireball;

    // ������� ��������, �������� � ������������
    // � ���������� ��������.
    [SerializeField]
    const float baseSpeed = 3.0f;

    // �������� ��� �������� �������� � ����������,
    // � �������� ����������� ������� �� �����������.
    [SerializeField]
    float speed = 3.0f;

    // ����������, �� ������� ���� �������� �����������
    // �� �����������.
    [SerializeField]
    float obstacleRange = 5.0f;

    // ���������� ���������� ��� ������������
    // ��������� ���������.
    bool _alive;

    private void Start()
    {
        // ������������� ���� ����������.
        _alive = true;
    }

    // ��������� ����� ����� �������� �� ������� SPEED_CHANGED.
    private void Awake()
    {
        Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    // ������� ����������.
    private void OnDestroy()
    {
        Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    // �����, ����������� � ���������� ��� ������� SPEED_CHANGED.
    private void OnSpeedChanged(float value)
    {
        speed = baseSpeed * value;
    }

    // �������� �����, ����������� �������� ����
    // �������������� �� "�����" ���������.
    public void SetAlive(bool alive)
    {
        _alive = alive;
    }

    private void Update()
    {
        if (_alive)
        {
            // ���������� �������� ����� � ������ �����, �������� �� ��������.
            transform.Translate(0, 0, speed * Time.deltaTime);

            // ��� ��������� � ��� �� ��������� � ������������ � ���
            // �� �����������, ��� � ��������.
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            // ������� ��� � ��������� ������ ���� �����������.
            if (Physics.SphereCast(ray, 0.75f, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;

                // ����� ����������� ��� �� ��������,
                // ��� � ������ � ��������� RayShooter.
                if (hitObject.GetComponent<PlayerCharacter>())
                {
                    if (_fireball == null)
                    {
                        _fireball = Instantiate(fireballPrefab) as GameObject;
                        // �������� �������� ��� ����� ������ � ������� � ����������� ��� ��������.
                        _fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                        _fireball.transform.rotation = transform.rotation;
                    }
                }
                else if (hit.distance < obstacleRange)
                {
                    // ������� � ���������� ��������� ������� ������ �����������.
                    float angle = Random.Range(-110, 110);
                    transform.Rotate(0, angle, 0);
                }
            }
        }
    }
}
