using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController:MonoBehaviour
{
    private PlayerControls controls;

    private Rigidbody2D rb;
    private float fixedX;

    public Transform ladder;
    public float ladderGrowSpeed = 2f;
    public float dropRotateSpeed = 90f;
    private bool isGrowing = false;
    private bool isDropping = false;

    private void Awake()
    {
        controls = new PlayerControls();

        // Subscribe to input events to start/stop ladder growing
        controls.Player.Interact.started += ctx => isGrowing = true;
        controls.Player.Interact.canceled += ctx =>
        {
            isGrowing = false;
            isDropping = true; 
        };
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fixedX = transform.position.x;
    }

    void FixedUpdate()
    {
        // Keep player fixed on X axis
        rb.position = new Vector2(fixedX, rb.position.y);
    }

    void Update()
    {
        if(isGrowing && ladder != null)
        {
            float growAmount = ladderGrowSpeed * Time.deltaTime;

            // Increase scale only upwards
            ladder.localScale += new Vector3(0, growAmount, 0);

            // Move ladder down by half of growAmount to keep bottom fixed
            ladder.localPosition += new Vector3(0, growAmount / 2f, 0);
        }
        else if(isDropping && ladder != null)
        {
            float step = dropRotateSpeed * Time.deltaTime;
            ladder.Rotate(0, 0, -step);
        }
    }

    private void OnEnable()
    {
        // Enable input system when this script is enabled
        controls.Enable();
    }

    private void OnDisable()
    {
        // Disable input system when this script is disabled
        controls.Disable();
    }
}