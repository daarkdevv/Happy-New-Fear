using UnityEngine;

public class ViewBobbing : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private bool _enable = true;
    [SerializeField, Range(0, 1f)] private float _yAmplitude = 0.015f; // Intensity of the vertical bobbing effect
    [SerializeField, Range(0, 30)] private float _yFrequency = 10.0f; // Speed of the vertical bobbing effect

    [SerializeField, Range(0, 0.2f)] private float _xAmplitude = 0.01f; // Intensity of the horizontal bobbing effect
    [SerializeField, Range(0, 30)] private float _xFrequency = 5.0f; // Speed of the horizontal bobbing effect

    [SerializeField] private Transform _camera = null; // The camera itself
    [SerializeField] private Transform _cameraHolder = null; // The pivot or holder of the camera
    [SerializeField] private float _bobTransitionSpeed = 5.0f; // Speed at which the bobbing adjusts to different states
    [SerializeField] private float _minSpeedThreshold = 0.05f; // Minimum speed to trigger bobbing
    [SerializeField] private PlayerMove _movement = null; // Reference to your custom player movement script

    private Vector3 _startPos;

    private void Awake()
    {
        _startPos = _camera.localPosition;
    }

    void Update()
    {
        if (!_enable) return;

        AdjustBobbing();
        ResetPosition();
        _camera.LookAt(FocusTarget());
    }

    private void AdjustBobbing()
    {
        float speed = _movement.movement.magnitude;

        float currentYBobIntensity = _yAmplitude;
        float currentYBobSpeed = _yFrequency;
        float currentXBobIntensity = _xAmplitude;
        float currentXBobSpeed = _xFrequency;

        if (_movement.isSprinting && _movement.canMove)
        {
            currentYBobIntensity *= 1.5f; // Adjust for sprinting
            currentYBobSpeed *= 1.5f;
            currentXBobIntensity *= 1.5f;
            currentXBobSpeed *= 1.5f;
        }
        else if (speed > _minSpeedThreshold && _movement.canMove)
        {
            currentYBobIntensity *= 1.0f; // Normal walking
            currentYBobSpeed *= 1.0f;
            currentXBobIntensity *= 1.0f;
            currentXBobSpeed *= 1.0f;
        }
        else
        {
            currentYBobIntensity *= 0.6f; // Idle
            currentYBobSpeed *= 0.5f;
            currentXBobIntensity *= 0.6f;
            currentXBobSpeed *= 0.5f;
        }

        Vector3 motion = CalculateBobbingMotion(currentYBobIntensity, currentYBobSpeed, currentXBobIntensity, currentXBobSpeed);
        PlayMotion(motion);
    }

    private Vector3 CalculateBobbingMotion(float yIntensity, float ySpeed, float xIntensity, float xSpeed)
    {
        Vector3 motion = Vector3.zero;
        motion.y = Mathf.Sin(Time.time * ySpeed) * yIntensity;
        motion.x = Mathf.Sin(Time.time * xSpeed) * xIntensity;
        return motion;
    }

    private void PlayMotion(Vector3 motion)
    {
        _camera.localPosition = _startPos + motion;
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + _cameraHolder.localPosition.y, transform.position.z);
        pos += _cameraHolder.forward * 15.0f;
        return pos;
    }

    private void ResetPosition()
    {
        if (_camera.localPosition == _startPos) return;
        _camera.localPosition = Vector3.Lerp(_camera.localPosition, _startPos, _bobTransitionSpeed * Time.deltaTime);
    }

    // Getter methods for bobbing parameters
    public float GetYAmplitude() => _yAmplitude;
    public float GetYFrequency() => _yFrequency;
    public float GetXAmplitude() => _xAmplitude;
    public float GetXFrequency() => _xFrequency;
}
