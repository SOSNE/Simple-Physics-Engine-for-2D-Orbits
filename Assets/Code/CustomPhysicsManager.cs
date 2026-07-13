using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomPhysicsManager : MonoBehaviour
{
    public static CustomPhysicsManager Instance;
    public List<CustomPhysicsBody> physicsBodies = new List<CustomPhysicsBody>();

    public float testForcePower;
    private Vector2 _testForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CustomPhysicsBody body1 = physicsBodies[0];
        CustomPhysicsBody body2 = physicsBodies[1];
        

        // Vector2 gravity = GetGravityForce(body1, body2);
        
        print(body1.name);
        
        Vector2 velocity = GetOrbitingVelocity(body2, body1);
        body2.velocity = velocity;

        // Vector2 force2 = CombineForces(-gravity);
        // ApplyForceToBody(body2, force2, dt);
    }

    void Update()
    {
        if (Keyboard.current.digit1Key.isPressed)
        {
            if (physicsBodies.Count > 0) CameraControl.Target = physicsBodies[0];
        }
        if (Keyboard.current.digit2Key.isPressed)
        {
            if (physicsBodies.Count > 1) CameraControl.Target = physicsBodies[1];
        }
        if (Keyboard.current.digit3Key.isPressed)
        {
            if (physicsBodies.Count > 2) CameraControl.Target = physicsBodies[2];
        }
        
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            CustomPhysicsBody clickedBody = Utils.DetectClickedObject();

            if (clickedBody != null)
            {
                Debug.Log($"Clicked on: {clickedBody.gameObject.name} with mass: {clickedBody.mass}");
                
                // Do whatever you want with the selected body here!
                // (e.g., store it in a "selectedBody" variable to apply forces or show UI)
            }

            CameraControl.Target = clickedBody;
        }
    }

    private void FixedUpdate()
    {
        if (Keyboard.current != null) // Safety check to make sure a keyboard is plugged in
        {
            if (Keyboard.current.aKey.isPressed)
            {
                _testForce.y = testForcePower;
            }
            else
            {
                _testForce.y = 0f;
            }
        }
        RunPhysicsLoop();
    }

    private void RunPhysicsLoop()
    {
        if (physicsBodies.Count == 0) return;
        float dt = Time.deltaTime;
        
        int count = physicsBodies.Count;

        for (int i = 0; i < count; i++)
        {
            if (count != 1)
            {
                for (int j = i + 1; j < count; j++)
                {
                    CustomPhysicsBody body1 = physicsBodies[i];
                    CustomPhysicsBody body2 = physicsBodies[j];

                    Vector2 gravity = GetGravityForce(body1, body2);


                    Vector2 force1 = CombineForces(gravity, _testForce);
                    ApplyForceToBody(body1, force1, dt);

                    Vector2 force2 = CombineForces(-gravity);
                    ApplyForceToBody(body2, force2, dt);

                }
            }
            else
            {
                CustomPhysicsBody body1 = physicsBodies[0];
                Vector2 force1 = CombineForces(_testForce);
                ApplyForceToBody(body1, force1, dt);
            }
        }
        
        // Future steps will go here:
        // 1. Reset Accelerations
        // 2. Calculate Gravity Forces
        // 3. Integrate Positions
        // 4. Update Transforms
        
    }

    private Vector2 GetAcceleration(Vector2 force, double mass)
    {
        return force / (float)mass;
    }
    private Vector2 GetVelocity(Vector2 acceleration, float dt)
    {
        return acceleration * dt;
    }
    private Vector2 GetPosition(Vector2 velocity,float dt)
    {
        return velocity * dt;
    }

    private void ApplyForceToBody(CustomPhysicsBody body, Vector2 force, float dt)
    {
        body.acceleration = GetAcceleration(force, body.mass);
        body.velocity += GetVelocity(body.acceleration, dt);
        body.physicsPosition += GetPosition(body.velocity, dt);
        
        body.UpdateTransform();
    }
    
    public Vector2 CombineForces(params Vector2[] forces)
    {
        Vector2 netForce = Vector2.zero;

        for (int i = 0; i < forces.Length; i++)
        {
            netForce += forces[i];
        }

        return netForce;
    }

    private Vector2 GetGravityForce(CustomPhysicsBody body1, CustomPhysicsBody body2)
    {
        float G = 6.674f; // Your custom gravity constant
    
        Vector2 direction = body2.physicsPosition - body1.physicsPosition;
        float r = direction.magnitude;

        return (float)(G * (body2.mass + body1.mass)) / (r * r) * direction.normalized;
    }

    private Vector2 GetOrbitingVelocity(CustomPhysicsBody body, CustomPhysicsBody bodyAround)
    {
        float G = 6.674f;
        Vector2 direction = bodyAround.physicsPosition - body.physicsPosition;
        float r = direction.magnitude;
        if (r < 0.01f) return Vector2.zero;
        float speed = Mathf.Sqrt((G * (float)bodyAround.mass) / r);
        Vector2 normalDir = direction.normalized;
        Vector2 orbitDirection = new Vector2(-normalDir.y, normalDir.x);
        
        return speed * orbitDirection;
    }
}
