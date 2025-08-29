using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float Speed = 1f;
    public int Damage = 1;

    private void Update()
    {
        transform.position += transform.right * Speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Bullet hit something: {collision.name}");

        var health = collision.GetComponentInParent<EnemyHealth>(); 
        if (health != null)
        {
            Debug.Log("EnemyHealth component found!");
            Vector2 hitDirection = (collision.transform.position - transform.position).normalized;
            health.TakeDamage(Damage, hitDirection);
        }

        Destroy(gameObject);
    }


}