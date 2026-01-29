using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class Animation2 : MonoBehaviour
{
    [Tooltip("Swing angle in degrees (right then left)")]
    [SerializeField] float angle = 45f;
    [Tooltip("Rotation speed in degrees per second")]
    [SerializeField] float rotationSpeed = 360f;
    [Tooltip("Minimum movement magnitude to trigger the animation")]
    [SerializeField] float moveThreshold = 0.01f;

    Rigidbody2D rb2d;
    Vector3 lastPosition;
    bool isAnimating;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        lastPosition = transform.position;
    }

    void Update()
    {
        bool moved = false;

        if (rb2d != null)
        {
            moved = rb2d.linearVelocity.sqrMagnitude > (moveThreshold * moveThreshold);
        }
        else
        {
            moved = ((Vector3)transform.position - lastPosition).sqrMagnitude > (moveThreshold * moveThreshold);
        }

        if (moved && !isAnimating)
        {
            StartCoroutine(AnimateSwing());
        }

        lastPosition = transform.position;
    }

    IEnumerator AnimateSwing()
    {
        isAnimating = true;

        Quaternion start = transform.rotation;
        Quaternion right = start * Quaternion.Euler(0f, 0f, angle);
        Quaternion left = start * Quaternion.Euler(0f, 0f, -angle);

        float segmentDuration = Mathf.Max(0.01f, angle / rotationSpeed);

        // rotate to right
        yield return RotateOver(start, right, segmentDuration);
        // rotate to left (through a smooth interpolation)
        yield return RotateOver(right, left, segmentDuration);
        // return to start
        yield return RotateOver(left, start, segmentDuration);

        transform.rotation = start;
        isAnimating = false;
    }

    IEnumerator RotateOver(Quaternion from, Quaternion to, float duration)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.rotation = Quaternion.Slerp(from, to, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        transform.rotation = to;
    }
}
