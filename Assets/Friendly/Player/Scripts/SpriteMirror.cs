using UnityEngine;

public class SpriteMirror : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (moveHorizontal < 0f)
        {
            spriteRenderer.flipX = true;    // Flip the sprite horizontally
        }
        else if (moveHorizontal > 0f)
        {
            spriteRenderer.flipX = false;   // Do not flip the sprite
        }
    }
}