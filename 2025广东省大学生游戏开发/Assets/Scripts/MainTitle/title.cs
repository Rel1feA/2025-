using UnityEngine;

public class TitleFloat : MonoBehaviour
{
    [Header("漂浮参数")]
    [Tooltip("漂浮速度 (值越大越快)")]
    public float floatSpeed = 1f;

    [Tooltip("漂浮幅度 (上下移动的距离)")]
    public float floatAmplitude = 0.2f;

    [Tooltip("漂浮平滑度 (值越大越平滑)")]
    [Range(0.1f, 5f)]
    public float smoothness = 2f;

    [Header("随机化设置")]
    [Tooltip("启用随机漂浮速度")]
    public bool randomizeSpeed = true;

    [Tooltip("速度随机范围 (相对于基础速度)")]
    public Vector2 speedRandomRange = new Vector2(0.8f, 1.2f);

    [Tooltip("启用随机漂浮相位")]
    public bool randomizePhase = true;

    [Tooltip("相位随机范围 (0-360度)")]
    public Vector2 phaseRandomRange = new Vector2(0f, 360f);

    // 私有变量
    private Vector3 _startPosition;
    private float _currentSpeed;
    private float _phaseOffset;

    void Start()
    {
        // 保存初始位置
        _startPosition = transform.position;

        // 随机化速度
        _currentSpeed = floatSpeed;
        if (randomizeSpeed)
        {
            _currentSpeed *= Random.Range(speedRandomRange.x, speedRandomRange.y);
        }

        // 随机化相位
        _phaseOffset = 0f;
        if (randomizePhase)
        {
            _phaseOffset = Random.Range(phaseRandomRange.x, phaseRandomRange.y) * Mathf.Deg2Rad;
        }
    }

    void Update()
    {
        // 计算正弦波位置
        float newY = _startPosition.y + Mathf.Sin(Time.time * _currentSpeed + _phaseOffset) * floatAmplitude;

        // 平滑移动到新位置
        Vector3 targetPosition = new Vector3(_startPosition.x, newY, _startPosition.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothness * Time.deltaTime);
    }

    // 编辑器方法：重置到初始位置
    public void ResetPosition()
    {
        transform.position = _startPosition;
    }

    // 编辑器方法：在Scene视图中显示漂浮范围
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            Vector3 startPos = Application.isPlaying ? _startPosition : transform.position;
            Vector3 topPos = startPos + Vector3.up * floatAmplitude;
            Vector3 bottomPos = startPos + Vector3.down * floatAmplitude;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(topPos, bottomPos);
            Gizmos.DrawSphere(topPos, 0.05f);
            Gizmos.DrawSphere(bottomPos, 0.05f);
            Gizmos.DrawSphere(startPos, 0.03f);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 0.1f);
        }
    }
}