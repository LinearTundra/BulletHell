using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float lifetime = 5f;

    private void Start()
    {
       
        gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");

        Destroy(gameObject, lifetime);
    }

   private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the bullet hit the player
        Destroy(gameObject);
        

        // Check if it hit a platform
        //if (collision.collider.CompareTag("Platform"))
        //{
       //     Destroy(gameObject);
       // }
    }
}
