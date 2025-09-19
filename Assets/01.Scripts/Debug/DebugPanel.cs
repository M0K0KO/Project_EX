using System;
using TMPro;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    public static DebugPanel instance;

    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private PlayerManager player;
    
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        Vector3 vel = player.rb.linearVelocity;
    
        debugText.text = $"X vel: {vel.x:F2} m/s\n" +
                         $"Y vel: {vel.y:F2} m/s\n" +
                         $"Z vel: {vel.z:F2} m/s\n\n" +
                         $"Speed: {vel.magnitude:F2} m/s";
    }

}
