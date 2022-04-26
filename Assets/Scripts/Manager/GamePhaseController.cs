using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public List<EnemyPrefab> enemyPrefabs;
}

[System.Serializable]
public struct EnemyPrefab {
    public EnemyType type;
    public GameObject PF_Enemy;
    public int enemyNumber;
    public float spawnIntervalTimer;

    public IEnumerator SpawnEnemyCoroutine() {
        int i = 0;
        while (i< enemyNumber) {
            GameObject enemy = null;
            enemy = GameObject.Instantiate(PF_Enemy);
            enemy.GetComponent<Unit_Enemy>().InitStartPoint();
            yield return new WaitForSeconds(spawnIntervalTimer);
            i += 1;
        }
    }
}

public class GamePhaseController : MonoBehaviour
{
    public List<Wave> waves;
    [SerializeField] private int currentWaveIndex;


    public void StartGame()
    {
        for (int j = 0; j < waves[currentWaveIndex].enemyPrefabs.Count; j++)
        {
            var enemy = waves[currentWaveIndex].enemyPrefabs[j];
            StartCoroutine(enemy.SpawnEnemyCoroutine());
        }
        
    }
}
