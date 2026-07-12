using UnityEngine;
using UnityEngine.InputSystem;

public class Utils : MonoBehaviour
{
    public static CustomPhysicsBody DetectClickedObject()
    {
        // 1. Get the current mouse screen position (pixels)
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        // 2. Convert the screen position to your actual 2D World Position
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        // 3. Cast a tiny point ray at that exact world position to see if it hits a 2D collider
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        // 4. If we hit something, check if it has our CustomPhysicsBody component
        if (hit.collider != null)
        {
            CustomPhysicsBody body = hit.collider.GetComponent<CustomPhysicsBody>();
            return body; // Will return the component, or null if it's just a random decoration background
        }

        return null; // Hit nothing
    }
}
