using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using System;

public class PlayerController:MonoBehaviour
{
    private PlayerControls controls;

    private Rigidbody2D rb;
    private float fixedX;


    //UI
    public Image ladderBarFill;
    private Camera mainCamera;


    public Transform ladder;
    public float ladderGrowSpeed = 1.5f;
    public float dropRotateSpeed = 90f;
    public float maxLadderHeight = 0.6f;
    private bool isGrowing = false;
    private bool isDropping = false;


    public GameObject ladderPrefab;
    private GameObject currentLadder;
    private Vector3 initialLadderScale;
    private Vector3 initialLadderPosition;

    private void Awake()
    {
        controls = new PlayerControls();

        // Subscribe to input events to start/stop ladder growing
        controls.Player.Interact.started += ctx => isGrowing = true;
        controls.Player.Interact.canceled += ctx =>
        {
            StopGrowingAndDrop();
        };
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fixedX = transform.position.x;
        mainCamera = Camera.main;

        if(ladder != null)
        {
            currentLadder = ladder.gameObject;

            initialLadderScale = ladder.localScale;
            initialLadderPosition = ladder.localPosition;
        }
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
            if(ladder.localScale.y < maxLadderHeight)
            {
                ladder.localScale += new Vector3(0, growAmount, 0);

                if(ladderBarFill != null)
                {
                    float fillAmount = ladder.localScale.y / maxLadderHeight;
                    ladderBarFill.fillAmount = Mathf.Clamp01(fillAmount);
                }
            }
            else
            {
                StopGrowingAndDrop();
            }

        }
        else if(isDropping && ladder != null)
        {
            float step = dropRotateSpeed * Time.deltaTime;
            ladder.Rotate(0, 0, -step);
        }
    }

    void LateUpdate()
    {
        if(ladderBarFill != null)
        {
            Vector3 worldPos = transform.position;
            Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);
            ladderBarFill.transform.parent.position = screenPos ;
        }
    }

    public void OnLadderHit()
    {
        isDropping = false;

        StartCoroutine(CallWithDelay(0.5f, SpawnNewLadder));
    }

    private void SpawnNewLadder()
    {
        if(ladderPrefab != null)
        {
            currentLadder = Instantiate(ladderPrefab, transform);
            currentLadder.transform.localPosition = initialLadderPosition;
            currentLadder.transform.localRotation = Quaternion.identity;
            currentLadder.transform.localScale = initialLadderScale;

            ladder = currentLadder.transform;
            LadderCollisionHandler ladderCollisionHandler = currentLadder.GetComponent<LadderCollisionHandler>();
            if(ladderCollisionHandler != null)
            {
                ladderCollisionHandler.player = this;
            }
            isGrowing = false;
            isDropping = false;


        }
    }

    private void StopGrowingAndDrop()
    {
        isGrowing = false;
        isDropping = true;
        ladderBarFill.fillAmount = 0f;
    }
    private IEnumerator CallWithDelay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
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