using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour {

    public Transform draggedShape;
    public float offset;

    /// <summary>
    /// - When pressed quickly, the shape teleports to mouse position.
    /// </summary>
	
	// Update is called once per frame
	void Update ()
    {
        if (draggedShape != null && draggedShape.tag != "Shape")
        {
            draggedShape = null;
        }

        Vector3 mousePositionWorld = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f)); // World positions of the mouse.
        RaycastHit2D hit = Physics2D.Raycast(mousePositionWorld, -Vector2.up);

        // Check if hit a shape
        if (hit.collider != null && hit.collider.tag == "Shape")
        {
            // If the LMS is down, set the draggedShape.
            if (Input.GetMouseButtonDown(0))
            {
                draggedShape = hit.transform;
                offset = draggedShape.position.y - mousePositionWorld.y;
            }
        }

        // If draggedShape is not null, move the shape with touch/mouse.
        if (draggedShape != null)
        {
            Vector2 shapePosition = mousePositionWorld;
            shapePosition.x = draggedShape.position.x;
            shapePosition.y = mousePositionWorld.y + offset;

            draggedShape.position = shapePosition;
        }

        // When the mouse is released, set draggedShape to null.
        if (Input.GetMouseButtonUp(0))
        {
            draggedShape = null;
        }
    }
}
