using UnityEngine;

public class GroundScroller:MonoBehaviour
{
    public float scrollSpeed = 3f;

    void Update()
    {
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
    }
}