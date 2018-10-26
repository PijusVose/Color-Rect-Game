using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeController : MonoBehaviour {

    public Camera mainCamera;
    public Transform cameraTransform;

    public Transform shapePrefab;
    public Transform shapeFolder;
    public List<Transform> shapes;

    public float initialSpeed = 1f;
    public float initialMaxHeight = 5f;
    public float maxSpeed = 3f;
    public float currentSpeed = 1f;
    public float acceleration = 0.05f;
    public float downSizeAcceleration = 0.05f;
    public float maxWidth = 4f;
    public float currentMaxHeight = 5f;
    public int maxShapes = 10;
    public float shapeLimits = 10f; // The smallest x of shape before it is respawned.
    public float minHeight = 1.5f;
    public float minWidth = 1.2f;

    private Transform lastShape; // Last shape that was created.
    private Transform endShape; // Last shape at the left.

	// Use this for initialization
	void Start ()
    {
        mainCamera = FindObjectOfType<Camera>();

        // Get the camera transform, if not available show warning.
        if (mainCamera != null)
        {
            cameraTransform = mainCamera.transform;
            currentSpeed = initialSpeed;
        }
        else
        {
            Debug.LogWarning("Camera has not been found.");
        }

        // Spawn some shapes at the beggining.
        for (int s = 0; s < maxShapes; s++)
        {
            SpawnShape();
        }

        GameController.Instance.StartGame();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Move the camera forward.
        if (GameController.Instance.gameStarted == true)
        {
            cameraTransform.position = cameraTransform.position + new Vector3(currentSpeed * Time.deltaTime, 0, 0);
        }

        // Spawn a new shape if the end shape reaches the X limits.
        if (endShape != null && endShape.position.x < cameraTransform.position.x - shapeLimits)
        {
            SpawnShape();
        }

        // Increase speed.
        currentSpeed += acceleration * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);

        // Reduce max shape height.
        currentMaxHeight -= downSizeAcceleration * Time.deltaTime;
        currentMaxHeight = Mathf.Clamp(currentMaxHeight, minHeight, initialMaxHeight);
    }

    Transform CreateShape()
    {
        // Instantiate a new shape and randomize it.
        Transform newShape = Instantiate(shapePrefab, shapeFolder);
        newShape = ResetShape(newShape);
        shapes.Add(newShape);

        return newShape;
    }

    void SpawnShape()
    {
        Transform nextShape = endShape;

        // If there is no available shape, create a new one.
        if (nextShape == null || shapes.Count < maxShapes)
        {
            nextShape = CreateShape();
        }

        nextShape.tag = "Shape";
        nextShape = ResetShape(nextShape);
        lastShape = nextShape; // The newly created shape becomes the last shape to be created.
        endShape = GetEndShape();
    }

    Transform ResetShape(Transform shape)
    {
        // Randomize the shape color.
        SpriteRenderer renderer = shape.GetComponent<SpriteRenderer>();
        renderer.color = new Color(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f));

        if (lastShape != null)
        {
            renderer.size = new Vector2(Random.Range(minWidth, maxWidth), Random.Range(minHeight, currentMaxHeight)); // Randomize the size.

            // If lastShape is null, that means this is the first shape and it needs to be set at the center and scaled right.
            if (lastShape != null)
            {
                SpriteRenderer lastRenderer = lastShape.GetComponent<SpriteRenderer>(); // Get the renderer of the last shape.
                Vector3 shapePosition = new Vector3(lastShape.position.x + (lastRenderer.size.x / 2f) + (renderer.size.x / 2f) - 0.026f, Random.Range(-5f, 5f), 0f); // Place the shape after the last shape.

                // If the position Y is close or at the center, change the Y to be further away from the center.
                if (shapePosition.y < 2f && shapePosition.y > 0f)
                {
                    shapePosition.y = Random.Range(2f, 5f);
                }
                else if (shapePosition.y > -2f && shapePosition.y < 0f)
                {
                    shapePosition.y = Random.Range(-2f, -5f);
                }

                shape.position = shapePosition;
            }
        }
        else
        {
            renderer.size = new Vector2(6f, 4f);
            shape.position = new Vector3(-2f, 0f);
            shape.tag = "Untagged";
        }

        // Set Collider to the sprite size.
        BoxCollider2D collider = shape.GetComponent<BoxCollider2D>();
        collider.size = renderer.size;

        return shape;
    }

    Transform GetEndShape()
    {
        Transform shape = lastShape;

        if (shape != null)
        {
            for (int s = 0; s < shapes.Count; s++)
            {
                if (shapes[s].position.x < shape.position.x)
                {
                    shape = shapes[s];
                }
            }
        }

        return shape;
    }
}
