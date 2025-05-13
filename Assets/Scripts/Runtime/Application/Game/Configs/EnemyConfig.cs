using Core;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Config/EnemyConfig")]
public class EnemyConfig : BaseSettings
{
    public float speed;
    public float captureRadius;
    public float detectionRadius;
    public float rotationSpeed;
}