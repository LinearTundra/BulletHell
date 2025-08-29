using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour {

    [Header("Animation")]
    [SerializeField] private GameObject playerSprite;
    [SerializeField] private Animator head;
    [SerializeField] private Animator body;
    [SerializeField] private InputActionReference cursorPosition;

    private Rigidbody2D rb;
    private int direction = 1;
    private float velocityX = 0;
    private bool isMoving = false;
    private bool alreadyFacingRight = false;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        velocityX = rb.linearVelocityX;
        changeAnimation();
        changeScale();
    }

    private bool isFacingRight() {
        Vector3 mousePos = cursorPosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        bool result = (transform.position.x-mousePos.x)>0?true:false; 
        return velocityX>0?!result:result; 
    }

    private void changeScale() {
        
        Vector3 scale = playerSprite.transform.localScale;
        void flipDirection() => direction = -direction;
        void flipScale() => scale.x = direction*Mathf.Abs(scale.x);
        void setScale() => playerSprite.transform.localScale = scale;

        if (direction!=1 && velocityX>0){
            flipDirection();
            flipScale();
            setScale();
        }
        
        if (direction!=-1 && velocityX<0) {
            flipDirection();
            flipScale();
            setScale();
        }
    }

    private void changeAnimation() {
        // Handle movement animation
        if (!isMoving && Mathf.Abs(velocityX) > 0.01f) {
            head.SetBool("isMoving", true);
            body.SetBool("isMoving", true);
            isMoving = true;}
        else if (isMoving && Mathf.Abs(velocityX) <= 0.01f)
        {
            head.SetBool("isMoving", false);
            body.SetBool("isMoving", false);
            isMoving = false;
        }

        // Flip animation if running backward (movement direction not cursor direction)
        Vector3 mousePos = cursorPosition.action.ReadValue<Vector2>();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        bool cursorOnLeft = mousePos.x < transform.position.x;
        bool movingRight = velocityX > 0;
        bool movingLeft = velocityX < 0;

        bool runningBackwards = (movingRight && cursorOnLeft) || (movingLeft && !cursorOnLeft);

        if (runningBackwards != !alreadyFacingRight) {
            head.SetBool("runningBackwards", runningBackwards);
            body.SetBool("runningBackwards", runningBackwards);
            alreadyFacingRight = !runningBackwards;
        }
    }


}
