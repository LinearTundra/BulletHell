using UnityEngine;
using System.Collections;


public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 3;

    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Material matInstance;
    private Coroutine flashCoroutine;
    public Vector2 knockbackForce = new Vector2(5f, 2f);
    private Rigidbody2D rb;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            matInstance = spriteRenderer.material; 
            originalColor = matInstance.GetColor("_Color");
        }
    }


    public void TakeDamage(int amount, Vector2 hitDirection)
    {
        currentHealth -= amount;

        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashEffect());

        ApplyKnockback(hitDirection);

        if (currentHealth <= 0)
            Die();
    }
    private IEnumerator FlashEffect()
    {
    if (matInstance == null)
        yield break;

    matInstance.SetColor("_Color", Color.red);
    yield return new WaitForSeconds(0.1f);
    matInstance.SetColor("_Color", originalColor); 
    }

    private void ApplyKnockback(Vector2 direction)
    {
        if (rb != null)
            rb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse);
    }


    private void Die()
    {
        SoundEffectManager.Play("death");
        Destroy(gameObject);
    }
}
