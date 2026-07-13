using System;
using UnityEngine;

public class CustomPhysicsBody : MonoBehaviour
{
    public double mass;
    public Vector2 velocity;
    public Vector2 acceleration;
    [HideInInspector] public Vector2 physicsPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        physicsPosition = transform.position;
        if (CustomPhysicsManager.Instance != null)
        {
            CustomPhysicsManager.Instance.physicsBodies.Add(this);
        }
    }

    private void OnDisable()
    {
        if (CustomPhysicsManager.Instance != null)
        {
            CustomPhysicsManager.Instance.physicsBodies.Remove(this);
        }        
    }

    public void UpdateTransform()
    {
        transform.position = physicsPosition;
    }
}
