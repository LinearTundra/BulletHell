using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private Vector2 knockbackForce = new Vector2(8f, 5f);

    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Rigidbody2D rb;
    private Coroutine flashCoroutine;

    private void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int amount, Vector2 hitDirection)
    {
        currentHealth -= amount;

        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashRed());

        if (rb != null)
            rb.AddForce(hitDirection.normalized * knockbackForce, ForceMode2D.Impulse);

        if (currentHealth <= 0)
            Die();
    }

    private IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
        gameObject.SetActive(false); // or trigger death animation/event
        SceneManager.LoadScene("GameOver");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
        {
            Vector2 hitDirection = (transform.position - collision.transform.position).normalized;
            TakeDamage(1, hitDirection);
        }
    }
}
