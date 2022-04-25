using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyPrefab {
    public EnemyType type;
    public GameObject prefab;
}

public class GameAssetManager : BasicGameAssetManager
{
    private static GameAssetManager _i;
    public static GameAssetManager i
    {
        get
        {
            if (_i == null)
            {
                _i = FindObjectOfType<GameAssetManager>();
            }
            return _i;
        }
    }
    [ArrayElementTitle("type")]
    public List<EnemyPrefab> enemyPrefabs;
}
