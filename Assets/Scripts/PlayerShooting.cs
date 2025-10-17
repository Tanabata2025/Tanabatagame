using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletParent;
    public float bulletSpeed = 9f;
    public float shootInterval = 0.05f;
    float timer = 0f;
    PlayerController pc;

    void Start()
    {
        pc = GetComponent<PlayerController>();
        if (bulletParent == null)
        {
            var go = new GameObject("BulletParent");
            bulletParent = go.transform;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= shootInterval)
        {
            SpawnBullet();
            timer = 0f;
        }
    }


    void SpawnBullet()
    {
        var b = Instantiate(bulletPrefab, transform.position, Quaternion.identity, bulletParent);
        var rb = b.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.up * bulletSpeed;
        }
        else
        {
            // Rigidbody2D 無しなら毎フレーム Translate でもよい
            b.AddComponent<Rigidbody2D>().gravityScale = 0f;
            b.GetComponent<Rigidbody2D>().linearVelocity = Vector2.up * bulletSpeed;
        }
    }
}
