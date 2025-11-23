using UnityEngine;

public class SprintDetector : MonoBehaviour
{
    public PlayerMove mover;

    [SerializeField] public IntObject intttobj;
    [SerializeField] Inventory inventory;

    [Header("Sway")]
    public float step = 0.01f;
    public float maxStepDistance = 0.06f;
    private Vector3 swayPos;

    [Header("Sway Rotation")]
    public float rotationStep = 4f;
    public float maxRotationStep = 5f;
    private Vector3 swayEulerRot;

    public float smooth = 10f;
    private float smoothRot = 12f;

    [Header("Bobbing")]
    public float speedCurve;
    private float curveSin { get => Mathf.Sin(speedCurve); }
    private float curveCos { get => Mathf.Cos(speedCurve); }

    public Vector3 travelLimit = Vector3.one * 0.025f;
    public Vector3 bobLimit = Vector3.one * 0.01f;
    private Vector3 bobPosition;

    public float bobExaggeration;

    [Header("Bob Rotation")]
    public Vector3 multiplier;
    private Vector3 bobEulerRotation;

    // Extra position variable for bobbing
    public Vector3 additionalBobbingPosition;

    [Header("Bobbing Parameters")]
    [SerializeField] private ViewBobbing viewBobbing;

    // Direction factor for alternating bobbing
    private float bobDirection = 1f;

    // Idle bobbing parameters
    public float idleBobbingSpeed = 1f;
    public float idleBobbingAmount = 0.005f;

    // Start is called before the first frame update
    void Start()
    {
        mover = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (intttobj != null && intttobj.isNotInPositionY)
        {
            return;
        }

        GetInput();

        Sway();
        SwayRotation();
        BobOffset();
        BobRotation();

        CompositePositionRotation();
    }

    private Vector2 walkInput;
    private Vector2 lookInput;

    void GetInput()
    {
        if (mover.canMove)
        {
            walkInput.x = Input.GetAxis("Horizontal");
            walkInput.y = Input.GetAxis("Vertical");
            walkInput = walkInput.normalized;  // Normalize the walk input to prevent over-speeding during diagonal movement
        }
        else
        {
            walkInput = Vector2.zero;  // Prevent regular bobbing when the player can't move
        }

        lookInput.x = Input.GetAxis("Mouse X");
        lookInput.y = Input.GetAxis("Mouse Y");
    }

    void Sway()
    {
        Vector3 invertLook = lookInput * -step;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance);

        swayPos = invertLook;
    }

    void SwayRotation()
    {
        Vector2 invertLook = lookInput * -rotationStep;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxRotationStep, maxRotationStep);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxRotationStep, maxRotationStep);
        swayEulerRot = new Vector3(invertLook.y, invertLook.x, invertLook.x);
    }

    void CompositePositionRotation()
    {
        // Preserve the current local scale
        Vector3 currentScale = transform.localScale;

        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPos + bobPosition + additionalBobbingPosition, Time.deltaTime * smooth);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(swayEulerRot) * Quaternion.Euler(bobEulerRotation), Time.deltaTime * smoothRot);

        // Reapply the preserved local scale
        transform.localScale = currentScale;
    }

    void BobOffset()
    {
        if (mover.canMove && walkInput != Vector2.zero)
        {
            // Movement-based bobbing
            speedCurve += Time.deltaTime * (walkInput.magnitude * bobExaggeration) + 0.01f;

            bobDirection = Mathf.Sin(speedCurve / 2);

            bobPosition.x = Mathf.Sin(speedCurve) * bobLimit.x - (walkInput.x * travelLimit.x);
            bobPosition.y = Mathf.Sin(speedCurve * 2) * bobLimit.y - (walkInput.y * travelLimit.y);
            bobPosition.z = Mathf.Sin(speedCurve * 0.5f) * bobLimit.z - (walkInput.y * travelLimit.z);
        }
        else
        {
            // Idle bobbing effect when not moving
            if (idleBobbingAmount > 0f)
            {
                // Apply idle bobbing effect
                speedCurve += Time.deltaTime * idleBobbingSpeed;

                // Reset to neutral position and apply idle bobbing
                bobPosition.x = Mathf.Sin(speedCurve) * idleBobbingAmount;
                bobPosition.y = Mathf.Sin(speedCurve * 2) * idleBobbingAmount;
                bobPosition.z = Mathf.Sin(speedCurve * 0.5f) * idleBobbingAmount;
            }
            else
            {
                // No idle bobbing
                bobPosition = Vector3.zero;
            }
        }
    }

    void BobRotation()
    {
        bobEulerRotation.x = bobDirection * (walkInput != Vector2.zero ? multiplier.x * Mathf.Sin(2 * speedCurve) : 0);
        bobEulerRotation.y = bobDirection * (walkInput != Vector2.zero ? multiplier.y * curveCos : 0);
        bobEulerRotation.z = bobDirection * (walkInput != Vector2.zero ? multiplier.z * curveCos * walkInput.x : 0);
    }
}
