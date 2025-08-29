using UnityEngine;

public class EnemyGlitchEffect : MonoBehaviour
{
    private Material mat;
    private float glitchTime = 0.2f;

    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }

    public void TriggerGlitch()
    {
        Debug.Log("gg");
        StopAllCoroutines();
        StartCoroutine(GlitchRoutine());
    }

    private System.Collections.IEnumerator GlitchRoutine()
    {
        mat.SetFloat("_GlitchIntensity", 1f);
        yield return new WaitForSeconds(glitchTime);
        mat.SetFloat("_GlitchIntensity", 0f);
    }
}
