using UnityEngine;

[CreateAssetMenu(fileName = "ShooterDefinition", menuName = "SO/Shared/ShooterDefinition")]
public class ShooterDefinition : ScriptableObject
{
    public float shootRate;
    public float bulletSpeed;
    public float bulletLifetime;
    public GameObject bullet;
}