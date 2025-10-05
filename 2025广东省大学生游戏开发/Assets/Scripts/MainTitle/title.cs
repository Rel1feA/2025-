using UnityEngine;

public class TitleFloat : MonoBehaviour
{
    [Header("Ư������")]
    [Tooltip("Ư���ٶ� (ֵԽ��Խ��)")]
    public float floatSpeed = 1f;

    [Tooltip("Ư������ (�����ƶ��ľ���)")]
    public float floatAmplitude = 0.2f;

    [Tooltip("Ư��ƽ���� (ֵԽ��Խƽ��)")]
    [Range(0.1f, 5f)]
    public float smoothness = 2f;

    [Header("���������")]
    [Tooltip("�������Ư���ٶ�")]
    public bool randomizeSpeed = true;

    [Tooltip("�ٶ������Χ (����ڻ����ٶ�)")]
    public Vector2 speedRandomRange = new Vector2(0.8f, 1.2f);

    [Tooltip("�������Ư����λ")]
    public bool randomizePhase = true;

    [Tooltip("��λ�����Χ (0-360��)")]
    public Vector2 phaseRandomRange = new Vector2(0f, 360f);

    // ˽�б���
    private Vector3 _startPosition;
    private float _currentSpeed;
    private float _phaseOffset;

    void Start()
    {
        // �����ʼλ��
        _startPosition = transform.position;

        // ������ٶ�
        _currentSpeed = floatSpeed;
        if (randomizeSpeed)
        {
            _currentSpeed *= Random.Range(speedRandomRange.x, speedRandomRange.y);
        }

        // �������λ
        _phaseOffset = 0f;
        if (randomizePhase)
        {
            _phaseOffset = Random.Range(phaseRandomRange.x, phaseRandomRange.y) * Mathf.Deg2Rad;
        }
    }

    void Update()
    {
        // �������Ҳ�λ��
        float newY = _startPosition.y + Mathf.Sin(Time.time * _currentSpeed + _phaseOffset) * floatAmplitude;

        // ƽ���ƶ�����λ��
        Vector3 targetPosition = new Vector3(_startPosition.x, newY, _startPosition.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothness * Time.deltaTime);
    }

    // �༭�����������õ���ʼλ��
    public void ResetPosition()
    {
        transform.position = _startPosition;
    }

    // �༭����������Scene��ͼ����ʾƯ����Χ
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