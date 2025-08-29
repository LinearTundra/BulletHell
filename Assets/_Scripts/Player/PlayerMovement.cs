using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

    [Header("Movement Input")]
    [SerializeField]
    private InputActionReference Move;
    // ------------------------------------------------------------------------------------- //
    [Header("Movement Forces")]
    [SerializeField, Tooltip("Player Movement Speed"), Range(1, 50)]
    private int MoveSpeed;
    [SerializeField, Tooltip("Player Jump Force"), Range(1, 50)]
    private int JumpForce;
    [SerializeField, Tooltip("Maximum Fall Speed of the Player"), Range(10, 50)]
    private int MaxFallSpeed;
    // ------------------------------------------------------------------------------------- //
    [Header("Hidden Movement Details")]
    [SerializeField, Tooltip("Increased Gravity When Falling"), Range(1, 10)]
    private float FallGravityMultiplier;
    [SerializeField, Tooltip("Time between pressing jump in air and jumping after landing"), Range(0, 3)]
    private float JumpBuffer;
    [SerializeField, Tooltip("Time interval between which player can jump after leaving the platform"), Range(0, 3)]
    private float cayoteTime;
    // ------------------------------------------------------------------------------------- //
    [Header("Weapon Properties")]
    [SerializeField]
    private InputActionReference pointerPosition;
    public Transform launchoffset;
    public GameObject ProjectilePrefab;
    // ------------------------------------------------------------------------------------- //
    private WeaponParent weaponParent;
    private Vector2 pointerinput;
    // ------------------------------------------------------------------------------------- //
    private Rigidbody2D rb;
    private bool validBufferJump = false;
    private bool isJumping = false;
    private bool isGrounded = false;
    private bool queuedJump = false;

    private void Awake() {
        weaponParent = GetComponentInChildren<WeaponParent>();
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        MovePlayerHorizontal();

        if(Input.GetButtonDown("Fire1"))
        {
            Quaternion baseRotation = launchoffset.rotation;
            SoundEffectManager.Play("fire");
            GameObject bullet = Instantiate(ProjectilePrefab, launchoffset.position, baseRotation);
            Destroy(bullet, 8);
            bullet = Instantiate(ProjectilePrefab, launchoffset.position, baseRotation * Quaternion.Euler(0, 0, 30));
            Destroy(bullet, 8);
            bullet = Instantiate(ProjectilePrefab, launchoffset.position, baseRotation * Quaternion.Euler(0, 0, -30));
            Destroy(bullet, 8);
        }

        if (Input.GetKeyDown(KeyCode.Space))
            InitiateJump();

        if (Input.GetKeyUp(KeyCode.Space) || rb.linearVelocityY <= 0) {
            rb.gravityScale = FallGravityMultiplier;
        }

        pointerinput = RotateGunToMouse();
        weaponParent.PointerPosition = pointerinput;
    }

    private void FixedUpdate() {
        Vector2 velocity = rb.linearVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -MaxFallSpeed, 99999);
        rb.linearVelocity = velocity;

        if (!isGrounded) checkGround();
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (rb.linearVelocityY == 0) isJumping = false;
        if (validBufferJump && queuedJump && isGrounded) {
            PLayerJump();
        }
    }

    private void OnDrawGizmosSelected() {
        Vector3 scale = transform.localScale;
        Vector2 origin = (Vector2)transform.position;
        origin.y -= scale.y / 2;
        Vector2 boxSize = new Vector2(1, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(origin, boxSize);
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (gameObject.activeInHierarchy) {
            StopCoroutine(cayoteHover());
            StartCoroutine(cayoteHover());
        }
    }

    private void MovePlayerHorizontal() {
        float xVelocity = Move.action.ReadValue<Vector2>().x;
        rb.linearVelocityX = xVelocity * MoveSpeed;
    }

    private void InitiateJump() {
        if (!isJumping) {
            PLayerJump();
            return;
        }
        if (gameObject.activeInHierarchy) {
            StopCoroutine(JumpQueue());
            StartCoroutine(JumpQueue());
            queuedJump = true;
        }
    }

    private void PLayerJump() {
        if (!isGrounded) return;
        rb.gravityScale = 1;
        rb.linearVelocityY = 0;
        SoundEffectManager.Play("jump");
        rb.AddForceY(JumpForce, ForceMode2D.Impulse);
        isJumping = true;
    }

    private void checkGround() {
        LayerMask groundLayer = LayerMask.GetMask("Platform");
        Vector3 scale = transform.localScale;
        Vector2 origin = (Vector2)transform.position;
        origin.y -= scale.y / 2;
        Vector2 boxSize = new Vector2(1, 0.1f);
        isGrounded = Physics2D.OverlapBox(origin, boxSize, 0, groundLayer);
    }

    private Vector2 RotateGunToMouse() {
        Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private IEnumerator JumpQueue() {
        validBufferJump = true;
        yield return new WaitForSeconds(JumpBuffer);
        validBufferJump = false;
        queuedJump = false;
    }

    private IEnumerator cayoteHover() {
        yield return new WaitForSeconds(cayoteTime);
        isGrounded = false;
    }

}
