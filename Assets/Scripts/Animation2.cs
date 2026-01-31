using UnityEngine;

public class PhysicsBobber2D : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] Rigidbody2D parentRb;
    
    private SpriteRenderer[] childRenderers; 
    private Color[] defaultColors;

    [Header("Hop Settings")]
    [SerializeField] float hopHeight = 0.2f;
    [SerializeField] float stepRate = 0.3f;
    [SerializeField] float minSpeed = 0.1f;

    [Header("Sway Settings")]
    [SerializeField] float swayAngle = 5f;
    [SerializeField] float swaySpeed = 10f;

    [Header("Death Settings")]
    [SerializeField] float deathPopHeight = 0.5f; 
    [SerializeField] float deathPopSpeed = 10f;   
    [SerializeField] Color flashColor = Color.red; 

    private bool isDead = false;
    private float deathTargetAngle;
    private float deathTimer = 0f;

    private float stepTimer;
    private bool toggleStep;
    private Vector3 initialLocalPos;

    void Awake()
    {
        if (parentRb == null) 
            parentRb = GetComponentInParent<Rigidbody2D>();

        childRenderers = GetComponentsInChildren<SpriteRenderer>();
        
        defaultColors = new Color[childRenderers.Length];

        for (int i = 0; i < childRenderers.Length; i++)
        {
            defaultColors[i] = childRenderers[i].color;
        }

        initialLocalPos = transform.localPosition;
    }

    void Update()
    {
        if (isDead)
        {
            HandleDeath();
            return; 
        }

        float speed = Mathf.Abs(parentRb.linearVelocity.x); 

        if (speed > minSpeed)
        {
            HandleHopping();
            HandleSwaying();
        }
        else
        {
            stepTimer = 0;
            ResetVisuals();
        }
    }

    public void TriggerDeath()
    {
        if (isDead) return;

        isDead = true;
        deathTimer = 0f;

        float direction = (Random.value > 0.5f) ? 1f : -1f;
        deathTargetAngle = direction * Random.Range(75f, 90f);
        
        if(parentRb != null) parentRb.linearVelocity = Vector2.zero;

        foreach (SpriteRenderer sr in childRenderers)
        {
            sr.color = flashColor;
        }
    }

    void HandleDeath()
    {
        deathTimer += Time.deltaTime;

        if (deathTimer < 0.15f)
        {
            Vector3 popTarget = initialLocalPos + Vector3.up * deathPopHeight;
            transform.localPosition = Vector3.Lerp(transform.localPosition, popTarget, Time.deltaTime * deathPopSpeed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime * 10f);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialLocalPos, Time.deltaTime * 5f);
            
            Quaternion targetRot = Quaternion.Euler(0, 0, deathTargetAngle);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * 5f);

            for (int i = 0; i < childRenderers.Length; i++)
            {
                childRenderers[i].color = Color.Lerp(childRenderers[i].color, defaultColors[i], Time.deltaTime * 3f);
            }
        }
    }

    void HandleHopping()
    {
        stepTimer += Time.deltaTime;
        float hopY = Mathf.Abs(Mathf.Sin((stepTimer / stepRate) * Mathf.PI)) * hopHeight;
        transform.localPosition = new Vector3(initialLocalPos.x, initialLocalPos.y + hopY, initialLocalPos.z);

        if (stepTimer >= stepRate)
        {
            toggleStep = !toggleStep;
            stepTimer = 0;
        }
    }

    void HandleSwaying()
    {
        float targetZ = toggleStep ? swayAngle : -swayAngle;
        Quaternion targetRot = Quaternion.Euler(0, 0, targetZ);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * swaySpeed);
    }

    void ResetVisuals()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime * 5f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, initialLocalPos, Time.deltaTime * 5f);
    }
}