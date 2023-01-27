using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Базовый интерфейс, который будут реализовывать
// диспетчеры данных.
public interface IGameManager
{
    // Это перечисление, которое нам нужно определить.
    // Свойство status сообщает остальной части когда,
    // завершил ли модуль инициализацию.
    ManagerStatus status { get; }

    // Метод Startup() предназначен для обработки процесса 
    // инициализации диспетчера, поэтому в нём выполняются
    // все связанные с этим процессом задания, а функция
    // задаёт состояние диспетчера.
    void Startup(NetworkService service);
}
