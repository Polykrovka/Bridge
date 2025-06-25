using UnityEngine;

public class LadderCollisionHandler:MonoBehaviour
{
    public PlayerController player;
    private Rigidbody2D rb;
    private bool hasLanded = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(hasLanded) return;
        
        

        if(collision.transform.CompareTag("Ground"))
        {
            hasLanded = true;

            transform.SetParent(collision.transform);

            if(rb != null)
            {
                rb.gravityScale = 1f;
            }

            if(player != null)
            {
                player.OnLadderHit();
            }
        }
    }
}