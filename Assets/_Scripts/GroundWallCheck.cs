using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GroundWallCheck : MonoBehaviour {

    public bool wallCollision {get; private set;} = false;
    public bool groundAvailable {get; private set;} = false;

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Platform") {
            groundAvailable = false;
        }
        if (other.tag == "Wall") {
            wallCollision = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Platform") {
            groundAvailable = true;
        }
        if (other.tag == "Wall") {
            wallCollision = true;
        }
    }

}
