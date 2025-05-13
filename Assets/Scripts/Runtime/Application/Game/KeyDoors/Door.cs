using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    [SerializeField] private KeyType _openKeyType;
    [SerializeField] private GameObject _finaleZone;

    private NavMeshObstacle _obstacle;

    private void Awake()
    {
        _obstacle = GetComponent<NavMeshObstacle>();
    }

    public void TryOpenDoor(List<KeyType> keys)
    {
        if (keys.Contains(_openKeyType))
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        _obstacle.enabled = false;
        _finaleZone.SetActive(true);
    }
}