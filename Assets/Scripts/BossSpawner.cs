using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [Header("Boss Settings")]
    public GameObject bossPrefab;        // ボスのPrefab
    public Transform spawnPoint;         // 出現位置
    public float spawnTime = 30f;        // 登場するまでの時間
    private bool bossSpawned = false;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (!bossSpawned && timer >= spawnTime)
        {
            SpawnBoss();
            bossSpawned = true;
        }
    }

    void SpawnBoss()
    {
        GameObject boss = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);

        // HPなどのパラメータを設定（Enemyスクリプトを使う場合）
        Enemy enemy = boss.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.hitPoints = 100;       // HPを100に
            enemy.fallSpeed = 0.5f;      // ゆっくり落ちる
        }

        Debug.Log("Boss登場！");
    }
}
