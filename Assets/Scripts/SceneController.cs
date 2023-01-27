using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��������� ���������� ������.
public class SceneController : MonoBehaviour
{
    // ��������������� ���������� ��� �����
    // � ��������-��������.
    [SerializeField]
    GameObject enemyPrefab;

    //// ������� ��������, �������� � ������������
    //// � ���������� ��������.
    //[SerializeField]
    //const float baseSpeed = 3.0f;

    //// �������� ��� �������� �������� � ����������,
    //// � �������� ����������� ������� �� �����������.
    //[SerializeField]
    //float speed = 3.0f;

    // �������� ���������� ��� �������� �� ����������� ����� � �����.
    GameObject _enemy;

    //// ��������� ����� ����� �������� �� ������� SPEED_CHANGED.
    //private void Awake()
    //{
    //    Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    //}

    //// ������� ����������.
    //private void OnDestroy()
    //{
    //    Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    //}

    //// �����, ����������� � ���������� ��� ������� SPEED_CHANGED.
    //private void OnSpeedChanged(float value)
    //{
    //    speed = baseSpeed * value;
    //}

    // ��������� ������ �����, ������ ���� ����� � ����� �����������.
    void Update()
    {
        if (_enemy == null)
        {
            // �����, ���������� ������-������.
            _enemy = Instantiate(enemyPrefab) as GameObject;
            _enemy.transform.position = new Vector3(0, 2, 0);
            float angle = Random.Range(0, 360);
            _enemy.transform.Rotate(0, angle, 0);
        }
    }
}
