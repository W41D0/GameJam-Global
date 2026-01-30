using System.Collections;
using UnityEngine;

public class AttendeeAnimator : MonoBehaviour
{
    [Header("Bob & Sway Settings")]
    [SerializeField] float bobHeight = 0.2f;    // How high it jumps
    [SerializeField] float swayAngle = 15f;     // How much it tilts
    [SerializeField] float stepSpeed = 10f;     // How fast the animation plays

    private Transform parentTransform;
    private Vector3 lastParentPosition;
    private bool isMoving;
    private bool isAnimating;
    private Vector3 defaultLocalPos;

    void Awake()
    {
        // Get the parent transform to track movement
        parentTransform = transform.parent;
        
        if (parentTransform == null)
        {
            Debug.LogError("This script must be on a CHILD object!");
            enabled = false;
            return;
        }

        lastParentPosition = parentTransform.position;
        defaultLocalPos = transform.localPosition;
    }

    void Update()
    {
        // 1. Calculate if the parent has moved since last frame
        float distanceMoved = Vector3.Distance(parentTransform.position, lastParentPosition);
        
        // Check if moved significantly (threshold avoids floating point errors)
        isMoving = distanceMoved > 0.001f;

        // 2. Trigger animation loop
        if (isMoving && !isAnimating)
        {
            StartCoroutine(DoBobAndSway());
        }

        // 3. Update position for the next frame calculation
        lastParentPosition = parentTransform.position;
    }

    IEnumerator DoBobAndSway()
    {
        isAnimating = true;

        // --- CYCLE START ---
        
        // 1. Tilt Right + Jump Up
        Quaternion rightRot = Quaternion.Euler(0, 0, -swayAngle);
        yield return MoveLocal(rightRot, bobHeight);

        // 2. Tilt Left + Dip Down then Up (The "step" over)
        Quaternion leftRot = Quaternion.Euler(0, 0, swayAngle);
        yield return MoveLocalDip(leftRot, bobHeight);

        // 3. Return to Center + Land
        yield return MoveLocal(Quaternion.identity, 0f);

        // --- CYCLE END ---
        
        // Reset purely to prevent drift
        transform.localPosition = defaultLocalPos;
        transform.localRotation = Quaternion.identity;
        
        isAnimating = false;
    }

    // Standard move: Linear rotation, Linear height change
    IEnumerator MoveLocal(Quaternion targetRot, float targetY)
    {
        Quaternion startRot = transform.localRotation;
        float startY = transform.localPosition.y;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * stepSpeed;
            float smooth = Mathf.SmoothStep(0, 1, t);

            // Rotate
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, smooth);
            
            // Move Y (Bob)
            float newY = Mathf.Lerp(startY, defaultLocalPos.y + targetY, smooth);
            transform.localPosition = new Vector3(defaultLocalPos.x, newY, defaultLocalPos.z);
            
            yield return null;
        }
    }

    // Dip move: Used when switching feet (High -> Low -> High)
    IEnumerator MoveLocalDip(Quaternion targetRot, float targetPeakY)
    {
        Quaternion startRot = transform.localRotation;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * stepSpeed;
            float smooth = Mathf.SmoothStep(0, 1, t);

            // Rotate
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, smooth);

            // Calculate a "U" shape for the dip (Simulates stepping down and back up)
            // 4 * (x - 0.5)^2 generates a parabola from 1 to 0 to 1
            float dipCurve = 4f * (smooth - 0.5f) * (smooth - 0.5f);
            
            float newY = defaultLocalPos.y + (targetPeakY * dipCurve);
            transform.localPosition = new Vector3(defaultLocalPos.x, newY, defaultLocalPos.z);

            yield return null;
        }
    }
}