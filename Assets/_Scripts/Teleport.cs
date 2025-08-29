using System.Collections;
using UnityEngine;

public class Teleport : MonoBehaviour {

    [SerializeField, Range(2, 10)]
    private float TeleportTime;
    [SerializeField, Range(0.01f, 5)]
    private float AbilitySpamTime;
    [SerializeField, Range(3, 50)]
    private int AbilitySpamCount;

    private float teleportTimeCounter = 0;
    
    private void Start() {
        TeleportTime += AbilitySpamTime * AbilitySpamCount;
    }

    private void FixedUpdate() {
        teleportTimeCounter += Time.fixedDeltaTime;
        if (teleportTimeCounter > TeleportTime+Random.Range(-0.5f, 0.5f)) {
            teleportTimeCounter = 0;
            StopCoroutine(SpamTeleport());
            StartCoroutine(SpamTeleport());
        }
    }

    private void teleport() {
        LayerMask groundLayer = LayerMask.GetMask("Platform");
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platform");
        foreach (GameObject platform in platforms) {
            var tpPos = validTPlocation(platform, groundLayer);
            if (tpPos == null) continue;
            transform.position = (Vector3)tpPos;
            return;
        }
    }

    private Vector3? validTPlocation(GameObject platform, LayerMask groundLayer) {

        Vector3 platformPosition = platform.transform.position;
        Vector3 playerPosition = transform.position;
        float distance = Vector3.Distance(platformPosition, playerPosition);
        
        if (distance < 3 || distance > 15) return null;
        
        float RandomX = Random.Range(platformPosition.x-6, platformPosition.x+6);
        Vector3 tpPos = new Vector3 (RandomX, platformPosition.y+2, 0);
        
        if (!Physics2D.Raycast(tpPos, Vector2.down, 3, groundLayer)) return null;

        return tpPos;
    }

    private IEnumerator SpamTeleport() {
        for (int i=0; i<AbilitySpamCount; i++) {
            yield return new WaitForSeconds(AbilitySpamTime);
            teleport();
        }
    }

}
