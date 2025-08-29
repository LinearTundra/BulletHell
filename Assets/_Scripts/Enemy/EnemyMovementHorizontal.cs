using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovementHorizontal : MonoBehaviour {

    [Header("Movement Forces")]
    [SerializeField, Range(1,30)]
    private int MoveSpeed;

    [Header("Contition Checkers")]
    [SerializeField, Tooltip("Add game object with script \"GroundWallCheck\" to detect walkable platform")]
    public GroundWallCheck walkCheck;

    private Rigidbody2D rb;
    private int MoveDirection = 1;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if (!isGrounded()) {
            rb.linearVelocityX = 0;
            return;
        }

        if (!walkCheck.groundAvailable || walkCheck.wallCollision) {
            SwitchDirection();
        }

        MoveEnemy();
    }

    private void OnDrawGizmosSelected() {
        Vector2 origin = (Vector2)transform.position;
        Vector3 scale = transform.localScale;
        origin.y -= scale.y/2;
        Vector2 boxSize = new Vector2 (1, 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(origin, boxSize);
    }

    private void SwitchDirection() {
        MoveDirection *= -1;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * MoveDirection;
        transform.localScale = scale;
    }

    private void MoveEnemy() {
        rb.linearVelocityX = MoveDirection * MoveSpeed;
    }

    private bool isGrounded() {

        LayerMask groundLayer = LayerMask.GetMask("Platform");
        
        Vector3 scale = transform.localScale;
        Vector2 origin = (Vector2)transform.position;
        origin.y -= scale.y/2;
        
        Vector2 boxSize = new Vector2 (2, 0.1f);
        
        return Physics2D.OverlapBox(origin, boxSize, 0, groundLayer);
    }

}
