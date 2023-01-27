using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ManagerStatus
{
    // Перечисляем возможные состояния диспетчеров,
    // принудительно устанавливая для свойства status
    // одно из указанных значений.
    Shutdown,
    Initializing,
    Started
}
