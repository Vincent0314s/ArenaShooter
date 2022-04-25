using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public int enemyNumber;
    public float spawnIntervalTimer;
}

public class GamePhaseController : MonoBehaviour
{
    public List<Wave> waves;
    [SerializeField] private int currentWaveIndex;
    

    public void StartGame() {
        //waves[_index]
        StartCoroutine(SpawnEnemyCoroutine(0));
    }

    IEnumerator SpawnEnemyCoroutine(int _index) {
        int i = 0;
        while (i < waves[_index].enemyNumber) {
            GameObject enemy = null;
            enemy = Instantiate(GameAssetManager.i.enemyPrefabs[0].prefab);
           enemy.GetComponent<Unit_Enemy>().InitStartPoint();
            yield return new WaitForSeconds(waves[_index].spawnIntervalTimer);
            i += 1;
        }
    }
}
