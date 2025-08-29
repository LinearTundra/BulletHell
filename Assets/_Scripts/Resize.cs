using System.Collections;
using UnityEngine;

public class Resize : MonoBehaviour {

    [SerializeField, Range(2, 10)]
    private float ResizeTime;
    [SerializeField, Range(0.01f, 5)]
    private float AbilitySpamTime;
    [SerializeField, Range(3, 50)]
    private int AbilitySpamCount;
    [SerializeField, Range(3, 10)]
    private int MaxEnlargeSize;

    private Rigidbody2D rb;
    private float resizeTimeCounter = 0;

    // Reference to glitch effect
    private EnemyGlitchEffect glitchEffect;

    private Vector3 originalScale;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        glitchEffect = GetComponent<EnemyGlitchEffect>();
        ResizeTime += AbilitySpamTime * AbilitySpamCount;

        originalScale = transform.localScale;
    }

    private void FixedUpdate() {
        resizeTimeCounter += Time.fixedDeltaTime;
        if (resizeTimeCounter > ResizeTime+Random.Range(-0.5f, 0.5f)) {
            resizeTimeCounter = 0;
            StopCoroutine(SpamResize());
            StartCoroutine(SpamResize());
        }
    }

    private void resize() {
        
        transform.localScale = originalScale;

        float size = Random.Range(1f, MaxEnlargeSize);
        float Xvelocity = rb.linearVelocityX;

        Vector2 origin = transform.position;
        origin.y = origin.y - transform.localScale.y / 2 + size / 2;
        transform.position = origin;
        float xSign = Mathf.Sign(Xvelocity);
        if (xSign == 0) xSign = 1;
        transform.localScale = new Vector3(size * xSign, size, 1);

        if (glitchEffect != null) {
            glitchEffect.TriggerGlitch();
        }
    }

    private IEnumerator SpamResize() {
        for (int i=0; i<AbilitySpamCount; i++) {
            yield return new WaitForSeconds(AbilitySpamTime);
            resize();
        }
    }

}
