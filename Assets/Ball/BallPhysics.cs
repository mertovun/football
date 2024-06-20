using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    public float dragCoefficient = 0.47f; // Approximate for a sphere
    public float airDensity = 1.225f; // kg/m^3 at sea level
    public float ballRadius = 0.11f; // meters, for a football
    public float magnusCoefficient = 0f; // Tweak this for the desired effect

    private Rigidbody rb;
    private float ballArea;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ballArea = Mathf.PI * ballRadius * ballRadius; 

    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        // ApplyDrag();
        // ApplyMagnusEffect();
    }

    public Vector3 CalculateDragForce()
    {
        Vector3 velocity = rb.velocity;
        float sqrSpeed = velocity.sqrMagnitude;
        return -0.5f * dragCoefficient * airDensity * ballArea * sqrSpeed * velocity.normalized;
    }

    void ApplyDrag()
    {
        Vector3 dragForce = CalculateDragForce();
        rb.AddForce(dragForce);
    }

    public Vector3 CalculateMagnusForce()
    {
        Vector3 velocity = rb.velocity;
        Vector3 angularVelocity = rb.angularVelocity;
        return magnusCoefficient * Vector3.Cross(angularVelocity, velocity);
    }

    void ApplyMagnusEffect()
    {
        Vector3 magnusForce = CalculateMagnusForce();
        rb.AddForce(magnusForce);
    }
}