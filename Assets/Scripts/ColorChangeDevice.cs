using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeDevice : MonoBehaviour
{
    // ќбъ€вление метода с таким же цветом,
    // как в сценарии дл€ двери.
    public void Operate()
    {
        // Ёти числа представл€ют собой RGB-значени€
        // в диапазоне от 0 до 1.
        Color randomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        // ÷вет задаЄтс€ в назначенном объекту материале.
        GetComponent<Renderer>().material.color = randomColor;
    }
}
