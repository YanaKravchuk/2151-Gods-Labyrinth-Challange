using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private KeyType _keyType;
    public KeyType KeyType => _keyType;
}

public enum KeyType
{
    Yellow,
    Red,
    Green
}