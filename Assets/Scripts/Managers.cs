using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Гарантируем существование различных диспетчеров.
[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(InventoryManager))]
public class Managers : MonoBehaviour
{
    // Статические свойства, которыми остальной
    // код пользуется для доступа к диспетчерам.
    public static PlayerManager Player { get; private set; }
    public static InventoryManager Inventory { get; private set; }

    // Список диспетчеров, который просматривается
    // в цикле во время стартовой последовательности.
    private List<IGameManager> _startSequence;

    // Перед методом Start().
    private void Awake()
    {
        Player = GetComponent<PlayerManager>();
        Inventory = GetComponent<InventoryManager>();

        _startSequence = new List<IGameManager>();
        _startSequence.Add(Player);
        _startSequence.Add(Inventory);
        StartCoroutine(StartupManagers());
    }

    // Начинает работу всех диспетчеров.
    private IEnumerator StartupManagers()
    {
        // Просматривает список диспетчеров
        // и запускает для каждого метод Startup().
        // Создаются экземпляры объекта
        // NetworkService для вставки во
        // все диспетчеры.
        NetworkService network = new NetworkService();

        foreach (IGameManager manager in _startSequence)
        {
            // Во время загрузки диспетчеров
            // передаём им сетевую службу.
            manager.Startup(network);
        }

        yield return null;

        int numModuls = _startSequence.Count;
        int numReady = 0;

        // Продолжаем цикл, пока не начнут работать
        // все диспетчеры.
        while (numReady < numModuls)
        {
            int lastReady = numReady;
            numReady = 0;

            foreach (IGameManager manager in _startSequence)
            {
                if (manager.status == ManagerStatus.Started)
                {
                    numReady++;
                }
            }

            if (numReady > lastReady)
            {
                Debug.Log("Progress: " + numReady + "/" + numModuls);

                // Остановка на один кадр перед
                // следующей проверкой.
                yield return null;
            }
            Debug.Log("All managers started up");
        }
    }
}
