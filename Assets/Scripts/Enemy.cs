using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int hitPoints = 10;
    public float fallSpeed = 1.5f;

    [Header("UI Settings")]
    public Transform canvasTransform;
    public GameObject hpTextPrefab;
    public TextMeshProUGUI hpText;
    public Camera worldCamera;

    private GameObject hpTextObj;

    void Start()
    {
        worldCamera = Camera.main;

        // HP Text を Canvas に生成
        if (canvasTransform != null && hpTextPrefab != null)
        {
            hpTextObj = Instantiate(hpTextPrefab, canvasTransform);
            hpText = hpTextObj.GetComponent<TextMeshProUGUI>();

            // 👇 重要：Pivotを中央にする（Prefab側で設定してても保険で）
            RectTransform rt = hpText.rectTransform;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);

            UpdateHPText();
        }
        else
        {
            Debug.LogWarning($"canvasTransformまたはhpTextPrefabが設定されていません: {gameObject.name}");
        }
    }

    void Update()
    {
        // 敵の移動
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

        // HP Text の追従（中央）
        if (hpText != null && worldCamera != null)
        {
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(worldCamera, transform.position);
            hpText.rectTransform.position = screenPos;
        }

        // ゲームオーバー判定
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CheckGameOver(transform.position.y);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            hitPoints--;
            Destroy(other.gameObject);
            UpdateHPText();

            if (hitPoints <= 0)
            {
                if (hpTextObj != null)
                {
                    Destroy(hpTextObj);
                }
                Destroy(gameObject);
            }
        }
    }

    void OnDestroy()
    {
        if (hpTextObj != null)
        {
            Destroy(hpTextObj);
        }
    }

    void UpdateHPText()
    {
        if (hpText != null)
        {
            hpText.text = hitPoints.ToString();
        }
    }
}
