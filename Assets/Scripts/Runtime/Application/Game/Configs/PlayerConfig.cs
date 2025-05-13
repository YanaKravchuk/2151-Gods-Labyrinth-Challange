using Core;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/PlayerConfig")]
public class PlayerConfig : BaseSettings
{
    public float speed;
    public float moveRange;
}