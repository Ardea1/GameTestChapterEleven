using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField]
    float speed = 10.0f;

    [SerializeField]
    int damage = 1;
    
    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    // Эта функция вызывается, когда с триггером
    // сталкивается другой объект.
    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        // Проверяем, является ли этот другой объект объектом PlayerCharacter.
        if (player != null)
        {
            player.Hurt(damage);
        }
        Destroy(this.gameObject);
    }
}
