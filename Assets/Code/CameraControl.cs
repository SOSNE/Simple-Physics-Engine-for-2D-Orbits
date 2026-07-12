using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public static CustomPhysicsBody Target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            transform.position = new Vector3(Target.physicsPosition.x, Target.physicsPosition.y, -10);
        }
    }
}
