using UnityEngine;

public class PhysicsBobber2D : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] Rigidbody2D parentRb; // Drag the Main Parent here

    [Header("Hop Settings")]
    [SerializeField] float hopHeight = 0.2f;    // How high visual moves up
    [SerializeField] float stepRate = 0.3f;     // Speed of the bob
    [SerializeField] float minSpeed = 0.1f;     // Minimum speed to start

    [Header("Sway Settings")]
    [SerializeField] float swayAngle = 5f;      // Tilt angle
    [SerializeField] float swaySpeed = 10f;     // How fast it tilts

    private float stepTimer;
    private bool toggleStep;
    private Vector3 initialLocalPos; // Remembers where the sprites sit naturally

    void Awake()
    {
        // If you forgot to assign it in inspector, try to find it on the parent
        if (parentRb == null) 
            parentRb = GetComponentInParent<Rigidbody2D>();

        initialLocalPos = transform.localPosition;
    }

    void Update()
    {
        // 1. Check Parent's Speed
        // Note: Use .velocity for Unity 5/2017-2022, .linearVelocity for Unity 6+
        float speed = Mathf.Abs(parentRb.linearVelocity.x); 

        if (speed > minSpeed)
        {
            HandleHopping();
            HandleSwaying();
        }
        else
        {
            // Reset to neutral when stopped
            stepTimer = 0;
            ResetVisuals();
        }
    }

    void HandleHopping()
    {
        stepTimer += Time.deltaTime;

        // VISUAL HOP: calculating a nice curve for the hop
        // We use a Sine wave based on the timer for a smooth up/down
        float hopY = Mathf.Abs(Mathf.Sin((stepTimer / stepRate) * Mathf.PI)) * hopHeight;
        
        // Apply to localPosition so it moves relative to the parent
        transform.localPosition = new Vector3(initialLocalPos.x, initialLocalPos.y + hopY, initialLocalPos.z);

        if (stepTimer >= stepRate)
        {
            toggleStep = !toggleStep; // Switch feet
            stepTimer = 0;
        }
    }

    void HandleSwaying()
    {
        // Pick a target angle based on which "foot" is active
        float targetZ = toggleStep ? swayAngle : -swayAngle;
        
        // VISUAL SWAY: Rotate ONLY this child container
        Quaternion targetRot = Quaternion.Euler(0, 0, targetZ);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * swaySpeed);
    }

    void ResetVisuals()
    {
        // Smoothly return to default position and rotation
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime * 5f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, initialLocalPos, Time.deltaTime * 5f);
    }
}