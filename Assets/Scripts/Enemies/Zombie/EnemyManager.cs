using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int totalEnemies;
    public Door door;

    private int deadEnemies = 0;

    public void EnemyDied()
    {
        deadEnemies++;

        if (deadEnemies >= totalEnemies)
        {
            door.OpenDoor();
        }
    }
}
