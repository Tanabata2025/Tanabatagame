using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;

    [Header("HP Text Settings")]
    public GameObject hpTextPrefab;      // HP Text用Prefab
    public Transform canvasTransform;     // CanvasのTransform

    [Header("Spawn Settings")]
    public float spawnInterval = 1.5f;
    public float spawnY = 8f;

    private float xRange;
    private float timer = 0f;

    void Start()
    {
        // カメラの横幅を計算して、少し余裕を持たせる
        float camHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        xRange = camHalfWidth * 0.9f; // 90%の範囲内に制限
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        float rx = Random.Range(-xRange, xRange);
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(rx, spawnY, 0f), Quaternion.identity);

        // EnemyスクリプトにCanvasとHP Text Prefabを渡す
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.canvasTransform = canvasTransform;
            enemyScript.hpTextPrefab = hpTextPrefab;
        }
        else
        {
            Debug.LogWarning("EnemyPrefabにEnemyスクリプトがアタッチされていません");
        }
    }
}
