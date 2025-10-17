using UnityEngine;
using UnityEngine.InputSystem;
#if ENABLE_INPUT_SYSTEM && UNITY_INPUT_SYSTEM
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
#endif

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public RectTransform touchArea; // Canvas上の透明FullScreen領域（割り当て）
    private Vector2 kbInput = Vector2.zero;
    private Vector2 touchPosition; // 最新のタッチ位置（スクリーン）
    private bool isTouching = false;

    private Vector3 lastPos;
    [HideInInspector] public bool IsMoving = false;

    void OnEnable()
    {
#if ENABLE_INPUT_SYSTEM && UNITY_INPUT_SYSTEM
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += OnFingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += OnFingerUp;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += OnFingerMove;
#endif
    }

    void OnDisable()
    {
#if ENABLE_INPUT_SYSTEM && UNITY_INPUT_SYSTEM
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= OnFingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= OnFingerUp;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove -= OnFingerMove;
        EnhancedTouchSupport.Disable();
#endif
    }

    void Start()
    {
        lastPos = transform.position;
    }

    void Update()
    {
        // キーボード入力（WASD / 矢印）
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            float x = 0;
            float y = 0;
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) x -= 1;
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) x += 1;
            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) y += 1;
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) y -= 1;
            kbInput = new Vector2(x, y).normalized;
        }

        Vector3 delta = Vector3.zero;

        if (isTouching)
        {
            // touchPosition はスクリーン座標。ワールドに変換して追従する方式にする
            Vector3 worldTouch = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, -Camera.main.transform.position.z));
            Vector3 dir = worldTouch - transform.position;
            delta = dir.normalized * moveSpeed * Time.deltaTime;
            // 近ければ直接合わせる
            if (dir.magnitude < 0.05f) delta = dir;
        }
        else if (kbInput.sqrMagnitude > 0.01f)
        {
            delta = new Vector3(kbInput.x, kbInput.y, 0f) * moveSpeed * Time.deltaTime;
        }

        transform.Translate(delta);

        // 移動判定（位置差分）
        float moved = Vector3.Distance(transform.position, lastPos);
        IsMoving = moved > 0.001f; // 閾値
        lastPos = transform.position;

        // --- 画面外に出ないようにする処理 ---
        Vector3 pos = transform.position;
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z));
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, -Camera.main.transform.position.z));

        // プレイヤーのサイズ分だけ余裕を持ってClamp
        float padding = 0.5f;
        pos.x = Mathf.Clamp(pos.x, min.x + padding, max.x - padding);
        pos.y = Mathf.Clamp(pos.y, min.y + padding, max.y - padding);

        transform.position = pos;

    }

#if ENABLE_INPUT_SYSTEM && UNITY_INPUT_SYSTEM
    void OnFingerDown(Finger finger) { isTouching = true; touchPosition = finger.currentTouch.screenPosition; }
    void OnFingerUp(Finger finger) { isTouching = false; }
    void OnFingerMove(Finger finger) { touchPosition = finger.currentTouch.screenPosition; }
#endif
}
