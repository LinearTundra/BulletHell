using UnityEngine;

public class BackgroundMovement : MonoBehaviour {
    
    [SerializeField, Range(1,10)]
    private int MoveSpeed;

    private void Update() {
        MoveBG();
    }

    private void MoveBG() {
        float inputX = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3 (inputX*MoveSpeed*Time.deltaTime, 0, 0);
    }

}
