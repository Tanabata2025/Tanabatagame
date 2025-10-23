using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Boss Settings")]
    public int maxHP = 100;
    private int currentHP;
    public float stopY = 3.5f;       // 停滞位置（画面上部）
    public float moveSpeed = 2f;

    [Header("Summon Settings")]
    public GameObject enemyPrefab;    // 召喚する敵
    public float summonInterval = 3f; // 召喚間隔
    public Transform[] summonPoints;  // 召喚位置（複数可能）

    private float summonTimer = 0f;
    private bool reachedPosition = false;

    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {
        // ① 停滞位置まで移動
        if (!reachedPosition)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(transform.position.x, stopY),
                moveSpeed * Time.deltaTime
            );

            if (Mathf.Abs(transform.position.y - stopY) < 0.01f)
            {
                reachedPosition = true;
            }
        }
        else
        {
            // ② 一定間隔で敵を召喚
            summonTimer += Time.deltaTime;
            if (summonTimer >= summonInterval)
            {
                SummonEnemy();
                summonTimer = 0f;
            }
        }
    }

    void SummonEnemy()
    {
        foreach (Transform point in summonPoints)
        {
            Instantiate(enemyPrefab, point.position, Quaternion.identity);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            currentHP--;
            Destroy(other.gameObject);

            if (currentHP <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
