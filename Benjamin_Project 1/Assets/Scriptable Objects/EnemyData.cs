using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    //Enemy
    public int health = 3;
    public int maxHealth = 3;
    public float detectionRange = 5;

    //NavMeshAgent
    public float speed = 3.5f;
    public float acceleration = 8f;
    public float angularSpeed = 120f;
    public float stoppingDistance = 0.5f;
    public float radius = 0.5f;
    public float height = 2f;
}
