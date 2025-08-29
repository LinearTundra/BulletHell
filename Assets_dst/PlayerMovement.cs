using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    float horizontalMovement;

    [SerializeField]
    private InputActionReference pointerPosition;
    public Transform launchoffset;
    public GameObject ProjectilePrefab;
    private WeaponParent weaponParent;
    private Vector2 pointerinput;
    private void Awake(){
        weaponParent = GetComponentInChildren<WeaponParent>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        pointerinput = GetPointerInput();
        weaponParent.PointerPosition = pointerinput;
        rb.linearVelocity = new Vector2(horizontalMovement*moveSpeed,rb.linearVelocity.y);

        if(Input.GetButtonDown("Fire1"))
        {
            Instantiate(ProjectilePrefab,launchoffset.position,transform.rotation);
        }
    }
    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;    
    }

    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
