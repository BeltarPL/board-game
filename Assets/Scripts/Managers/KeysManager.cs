using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeysManager : Singleton<KeysManager>
{
    public UnityEvent OnKeyPicked;
    public List<Vector2> keySpawnPoint = new();
    public GameObject key;
    void Start()
    {
        OnKeyPicked.AddListener(SetKeyPosition);
        SetKeyPosition();
    }

    private void SetKeyPosition()
    {
        if (keySpawnPoint.Count == 0)
        {
            key.SetActive(false);
            return;
        }

        int randomSpawnPoint = Random.Range(0, keySpawnPoint.Count);
        EventsGenerator.SetPositionState((int)keySpawnPoint[randomSpawnPoint].x, (int)keySpawnPoint[randomSpawnPoint].y, true);
        key.transform.SetPositionAndRotation(keySpawnPoint[randomSpawnPoint], Quaternion.identity);
        keySpawnPoint.RemoveAt(randomSpawnPoint);
    }

    public bool CheckIfPositionReserved(Vector2 position)
    {
        if (keySpawnPoint.Contains(position))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
