using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerPullController : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;
    private bool isDragging = false;
    private Rigidbody2D rb;
    private Vector3 dragForce;
    public float forceMult;
    public Vector3 mouseLocation;
    public Vector2 minPower;
    public Vector2 MaxPower;
    public bool isGrounded;
    private LineRenderer lineRenderer;
    private Vector3 currentMousePosition;
    private Vector2 currentDragForce;
    private Vector3 lineStartPos;
    public float fallAcceleration;
    public PhysicsMaterial2D bounceMat, normalMat;
    public bool isTouchingWall;
    public bool isWallSticking;
    public bool isWallJumpMode;
    public int wallJumpCounter = 0;
    public bool currentlyFacingRight;
    public bool isFalling;
    public bool isCloseToGround;
    public PlayerInventoryManager inventory;

    //sprite flipping
    private float previousXPosition;

    //powerup particle effecst
    public GameObject powerupParticle;
    public GameObject powerupLight;

    private CinemachineImpulseSource impulseSource;

    //audio
    public AudioSource bigJump;
    public AudioSource smallJump;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // We need two points to draw a line
        Color lineColor = new(251, 245, 239, 1);
        lineRenderer.material.SetColor("_Color", lineColor);
        previousXPosition = transform.position.x;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // close to ground check
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));
       isCloseToGround = hit.collider != null;

        if (isCloseToGround)
        {
            rb.sharedMaterial = normalMat;
        }
        if(wallJumpCounter >= 1)
        {
            isWallJumpMode = false;
            isWallSticking = false;
            isTouchingWall = false;
            rb.sharedMaterial = normalMat;
        };
        //on ground check
        isGrounded = Physics2D.OverlapBox(
            new Vector2(gameObject.transform.position.x,
            gameObject.transform.position.y - 0.5f),
            new Vector2(0.8f, 0.05f), 0f, LayerMask.GetMask("Ground"));
        if(isGrounded || isWallSticking)
        {
            isFalling = false;
            if (!isWallSticking)
            {
                wallJumpCounter = 0;
            }
            rb.sharedMaterial = normalMat;
            rb.velocity = new Vector2(0f, 0f);
            if (Input.GetMouseButtonDown(0))
            {
                startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                lineStartPos = startPos;
                startPos.z = 15;
                isDragging = true;
            }

            if (Input.GetMouseButton(0) && isDragging)
            {
                currentMousePosition =
                    Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentMousePosition.z = 0; 

                // Calculate the drag force
                Vector2 dragVector = new Vector2(startPos.x -
                    currentMousePosition.x, startPos.y -
                    currentMousePosition.y);
                dragVector = new Vector2(Mathf.Clamp(dragVector.x,
                    minPower.x, MaxPower.x),
                    Mathf.Clamp(dragVector.y, minPower.y, MaxPower.y));

                // Calculate the clamped end position
                Vector3 clampedEndPosition = new Vector3(startPos.x -
                    dragVector.x, startPos.y - dragVector.y, 0);

                // Update the line renderer positions
                lineStartPos.z = 0;
                lineRenderer.SetPosition(0, lineStartPos);
                clampedEndPosition.z = 0;
                lineRenderer.SetPosition(1, clampedEndPosition);
            }

            if (Input.GetMouseButtonUp(0) && isDragging)
            {
                if (isWallJumpMode && isWallSticking)
                {
                    wallJumpCounter += 1;
                }
                isGrounded = false;
                isWallSticking = false;
                endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                endPos.z = 15;

                dragForce = new Vector2(Mathf.Clamp(startPos.x - endPos.x,
                    minPower.x, MaxPower.x), Mathf.Clamp(startPos.y - endPos.y,
                    minPower.y, MaxPower.y));
                isDragging = false;
                lineRenderer.SetPosition(0, Vector3.zero);
                lineRenderer.SetPosition(1, Vector3.zero);
                float impulseForce = (Mathf.Abs(dragForce.x) + Mathf.Abs(dragForce.y) / 2) / 15;
                Debug.Log(impulseForce);
                if (dragForce.x >= MaxPower.x || dragForce.y >= MaxPower.y)
                {
                    CinemachineShake.instance.CameraShake(impulseSource, 0.7f);
                    bigJump.pitch = Random.Range(0.8f, 1.2f);
                    bigJump.Play();
                }
                else
                {
                    smallJump.pitch = Random.Range(0.8f, 1.2f);
                    smallJump.volume = 1 * Mathf.Clamp(Mathf.Abs(impulseForce), 0.02f, 0.199f);
                    Debug.Log("VOLUME" + smallJump.volume);
                    smallJump.Play();

                    CinemachineShake.instance.CameraShake(impulseSource, impulseForce);
                }
                Debug.Log(wallJumpCounter);
            }

            if (Input.GetMouseButtonDown(0))
            {
                startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Debug.Log(startPos);
            }
        }
        if (!isGrounded && !isFalling)
        {
            rb.sharedMaterial = bounceMat;
            
        }
        // Handle right-click for wall sticking
        if (Input.GetMouseButtonDown(1) && inventory.jumpPowerup)
        {
            isWallJumpMode = !isWallJumpMode;
            Debug.Log(isWallJumpMode);
        }
        handlePowerupEffects();
    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(0f, 0f);
        }
        if (dragForce != Vector3.zero)
        {
            if(wallJumpCounter <= 1)
            {
                rb.AddForce(dragForce * forceMult, ForceMode2D.Impulse);
                dragForce = Vector3.zero;
            } else
            {
                //rb.sharedMaterial = normalMat;
                if (!isGrounded)
                {
                    //rb.AddForce(dragForce * 150f, ForceMode2D.Impulse);
                }
            }
        }

        if (rb.velocity.y < 0)
        {
            isFalling = true;
            if (isCloseToGround)
            {
                dragForce = Vector3.zero;
                rb.sharedMaterial = normalMat;
            }
            rb.AddForce(Vector2.down * forceMult, ForceMode2D.Force);
        }
        FlipSprite();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (isWallJumpMode)
            {
                Debug.Log("WALL");
                isWallSticking = true;
            }
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.sharedMaterial = normalMat;
            Debug.Log("Ground");
        }
    }

    void FlipSprite()
    {
        float currentXPosition = transform.position.x;
        if (currentXPosition > previousXPosition && !isGrounded)
        {
            // Moving right
            transform.localScale = new Vector3(1, 1, 1);
            currentlyFacingRight = true;
        }
        else if (currentXPosition < previousXPosition && !isGrounded)
        {
            // Moving left
            transform.localScale = new Vector3(-1, 1, 1);
            currentlyFacingRight = false;
        }
        previousXPosition = currentXPosition;
    }

    void handlePowerupEffects()
    {
        powerupLight.SetActive(isWallJumpMode);
        powerupParticle.SetActive(isWallJumpMode);
    }
}
