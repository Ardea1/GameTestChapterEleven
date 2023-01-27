using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour
{
    // Метод, вызванный сценарием стрельбы.
    public void ReactToHit()
    {
        WanderingAI behavior = GetComponent<WanderingAI>();

        // Проверяем, присоединён ли к персонажу сценарий WanderingAI;
        // он может и отсутствовать. 
        if (behavior != null)
        {
            behavior.SetAlive(false);
        }
        StartCoroutine(Die());
    }

    // Опрокидываем врага, ждём 1,5 секунды и уничтожаем его.
    private IEnumerator Die()
    {
        this.transform.Rotate(-75, 0, 0);

        yield return new WaitForSeconds(1.5f);

        // Объект может уничтожать сам себя.

        // Такие выражения как this.gameObject, относятся к объекту
        // GameObject, к которому присоединён данный сценарий.
        Destroy(this.gameObject);
    }
}
