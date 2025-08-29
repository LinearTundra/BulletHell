using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public Vector2 PointerPosition { get; set; }
    public SpriteRenderer weaponRenderer;
    [SerializeField]
    private Transform muzzle;
    private Vector3 originalMuzzleLocalPos;

    private void Start()
    {
        if (muzzle != null)
            originalMuzzleLocalPos = muzzle.localPosition;
    }

    private void Update()
    {
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        bool shouldFlip = angle > 90 || angle < -90;

        weaponRenderer.flipY = shouldFlip;
        weaponRenderer.flipX = shouldFlip;

        if (muzzle != null)
        {
            muzzle.localPosition = shouldFlip
                ? new Vector3(-originalMuzzleLocalPos.x, originalMuzzleLocalPos.y, originalMuzzleLocalPos.z)
                : originalMuzzleLocalPos;
        }
    }
}
