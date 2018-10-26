using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Update is called once per frame
    void Update ()
    {
        if (GameController.Instance.gameStarted == true)
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            float playerRight = transform.position.x + renderer.bounds.extents.x; // The right edge of the sprite.
            float playerTop = transform.position.y + renderer.bounds.extents.y;
            float playerBottom = transform.position.y - renderer.bounds.extents.y;

            RaycastHit2D hit = Physics2D.Raycast(new Vector2(playerRight, transform.position.y), -Vector2.up);

            // Check if the player even collides with a shape.
            if (hit.collider != null && hit.collider.tag == "Shape")
            {
                hit.collider.tag = "Untagged";

                // Get the top and bottom part of the shapes sprite.
                SpriteRenderer shapeRenderer = hit.transform.GetComponent<SpriteRenderer>();
                float shapeTop = hit.transform.position.y + shapeRenderer.bounds.extents.y;
                float shapeBottom = hit.transform.position.y - shapeRenderer.bounds.extents.y;

                // Check if player fits in the shape.
                if (playerTop > shapeTop || playerBottom < shapeBottom)
                {
                    GameController.OnPlayerDiedEvent(); // End Game.
                }
                else
                {
                    GameController.OnAddScoreEvent(); // Add score.
                }
            }
            else if (hit.collider == null)
            {
                GameController.OnPlayerDiedEvent();
            }
        }
    }
}
