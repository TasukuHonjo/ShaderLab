
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Input Systemのアクションが押されたとき、Prefabを生成する。
/// </summary>
public class SpawnPrefabOnInput : MonoBehaviour
{
    [Header("生成設定")]

    [Tooltip("生成するPrefab")]
    [SerializeField] private GameObject prefab;

    [Tooltip("生成位置")]
    [SerializeField] private Transform spawnPoint;

    [Tooltip("生成したオブジェクトをこのTransformの子にする")]
    [SerializeField] private Transform spawnParent;

    [Header("入力設定")]

    [Tooltip("Prefab生成に使用するInput Action")]
    [SerializeField] private InputActionReference spawnAction;

    [Tooltip("生成エリアのランダム値")]
    [SerializeField] float rand = 5;

    [Tooltip("生成間隔の乱数")]
    [SerializeField] float randInterval = 5;

    float time = 0;
    float interval = 0;

    [SerializeField] int spawnCount = 5;

    void Start()
    {
        interval = Random.Range(0, randInterval);
    }

    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        if (time > interval)
        {
            time = 0;
            interval = Random.Range(0, randInterval);
            for(int i = 0; i < spawnCount; i++)
            {
                OnSpawnPer();
            }
        }
    }

    /// <summary>
    /// Input Actionを有効化し、入力イベントを登録する。
    /// </summary>
    private void OnEnable()
    {
        if (spawnAction == null || spawnAction.action == null)
        {
            Debug.LogWarning(
                $"{nameof(SpawnPrefabOnInput)}: Spawn Actionが未設定です。",
                this
            );
            return;
        }

        spawnAction.action.performed += OnSpawnPerformed;
        spawnAction.action.Enable();
    }

    /// <summary>
    /// Input Actionの登録を解除する。
    /// </summary>
    private void OnDisable()
    {
        if (spawnAction == null || spawnAction.action == null)
            return;

        spawnAction.action.performed -= OnSpawnPerformed;
        spawnAction.action.Disable();
    }

    /// <summary>
    /// ボタンが押されたときにPrefabを生成する。
    /// </summary>
    private void OnSpawnPerformed(InputAction.CallbackContext context)
    {
        OnSpawnPer();
    }

    private void OnSpawnPer()
    {
        if (prefab == null)
        {
            Debug.LogWarning("生成するPrefabが設定されていません。", this);
            return;
        }

        Vector3 position = spawnPoint != null
            ? spawnPoint.position
            : transform.position;

        Quaternion rotation = spawnPoint != null
            ? spawnPoint.rotation
            : transform.rotation;

        position += new Vector3(Random.Range(-rand, rand), 0, Random.Range(-rand, rand));
        Instantiate(prefab, position, rotation);
    }
}
