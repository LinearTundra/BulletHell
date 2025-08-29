using UnityEngine;

public class bullet : MonoBehaviour
{

    public float bspeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(bspeed,GetComponent<Rigidbody2D>().linearVelocity.y);
        Destroy(gameObject, 3f);
    }
}
