using UnityEngine;
using System.Collections;

public class EnemyShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float minShootInterval = 1f;
    public float maxShootInterval = 3f;
    public float bulletSpeed = 10f;

    private Transform player;
    private bool isShooting = false;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void FixedUpdate() {
        bool playerInBox = PlayerPresent();
        if (playerInBox && !isShooting){
            StartCoroutine(ShootAtRandomIntervals());
            isShooting = true;
        }
        else if (!playerInBox){
            StopAllCoroutines();
            isShooting = false;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Vector3 origin = transform.position;
        origin.y += 4;
        Gizmos.DrawWireCube(origin, new Vector3(30, 30));
    }

    private IEnumerator ShootAtRandomIntervals()
    {
        while (true)
        {
            float waitTime = Random.Range(minShootInterval, maxShootInterval);
            yield return new WaitForSeconds(waitTime);

            if (player != null)
                Shoot();
        }
    }

    private void Shoot()
    {
        Vector2 direction = (player.position - firePoint.position).normalized;
        SoundEffectManager.Play("enemy bullets");
        Vector2 varDir = direction;
        
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.AddForce(direction*bulletSpeed, ForceMode2D.Impulse);
        
        varDir.x += direction.y/2;
        varDir.y += direction.x/2;
        bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.AddForce(varDir.normalized*bulletSpeed, ForceMode2D.Impulse);
        
        varDir = direction;
        varDir.x -= direction.y/2;
        varDir.y -= direction.x/2;
        bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.AddForce(varDir.normalized*bulletSpeed, ForceMode2D.Impulse);
    }

    private bool PlayerPresent() {
        Vector3 origin = transform.position;
        origin.y += 4;
        return Physics2D.OverlapBox(origin, new Vector2(30, 30), 0, LayerMask.GetMask("Player"));
    }
}
