using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Scriptables/Player/PlayerConfig", order = 1)]
public class PlayerConfig : ScriptableObject
{
    [Header("Locomotion Configs")] 
    public float runSpeed = 5f;
    public float sprintSpeed = 12f;
    public float rotateSpeed = 10f;
}
