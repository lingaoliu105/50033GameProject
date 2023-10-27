using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public struct EnemyWave {
    public int enemyCount;
    public GameObject[] enemyPrefab;
    public Vector2[] spawnPosition;
    public float spawnInterval;
    public float tillNextWave;
}
public class EnemyManager : Singleton<EnemyManager> {
    public EnemyGeneration[] enemyGenerations;
    public EnemyGeneration enemyGeneration;
    private GameObject[] enemies;
    public int currentLevel = 0;
    
    public void GameStart() {
        enemyGeneration = enemyGenerations[currentLevel];
        StartCoroutine(StartGenerationAfterTime(3));
    }

    public void GameEnd() {
        StopAllCoroutines();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            Destroy(enemy);
        }
    }

    public void GameRestart() {
        StopAllCoroutines();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            Destroy(enemy);
        }
        StartCoroutine(StartGenerationAfterTime(3));
    }

    private IEnumerator StartGenerationAfterTime(float time) {
        yield return new WaitForSeconds(time);
        StartCoroutine(GenerateEnemyWaves());
    }
    private IEnumerator GenerateEnemyWaves() { 
        foreach (EnemyWave enemyWave in enemyGeneration.enemyWaves) {
            yield return StartCoroutine(GenerateAWave(enemyWave));
            yield return new WaitForSeconds(enemyWave.tillNextWave);
        }
    }
    private IEnumerator GenerateAWave(EnemyWave enemyWave) {
        for (int i = 0; i < enemyWave.enemyCount; i++) {
            GameObject enemy = Instantiate(enemyWave.enemyPrefab[i], enemyWave.spawnPosition[i], Quaternion.identity);
            yield return new WaitForSeconds(enemyWave.spawnInterval);
        }
    }
}

