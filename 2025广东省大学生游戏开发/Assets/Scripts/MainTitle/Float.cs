using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float rotateSpeed = 30f;
    public float amplitude = 0.5f; // 上下浮动幅度
    public bool randomizeRotation = true; // 是否启用随机旋转
    public Vector2 rotateSpeedRange = new Vector2(15f, 45f); // 旋转速度范围

    private Vector3 _startPos;
    private float _currentRotateSpeed;
    private int _rotationDirection; // 1为顺时针，-1为逆时针

    void Start()
    {
        _startPos = transform.position;
        moveSpeed *= Random.Range(0.8f, 1.2f); // 随机速度更自然

        // 随机化旋转方向和速度
        if (randomizeRotation)
        {
            // 随机选择旋转方向（顺时针或逆时针）
            _rotationDirection = (Random.Range(0, 2) == 0) ? 1 : -1;

            // 在指定范围内随机旋转速度
            _currentRotateSpeed = Random.Range(rotateSpeedRange.x, rotateSpeedRange.y);
        }
        else
        {
            // 如果不随机化，使用默认值
            _rotationDirection = 1;
            _currentRotateSpeed = rotateSpeed;
        }
    }

    void Update()
    {
        // 上下漂浮（正弦曲线）
        float newY = _startPos.y + Mathf.Sin(Time.time * moveSpeed) * amplitude;
        transform.position = new Vector3(_startPos.x, newY, _startPos.z);

        // 自转（使用随机方向和速度）
        transform.Rotate(Vector3.forward, _currentRotateSpeed * _rotationDirection * Time.deltaTime);
    }
}