using UnityEngine;
using System.Collections.Generic;

public class BallController : MonoBehaviour
{
    public float speedMultiplier = 1f;
    public float maxSpinSpeed = 20f; // Maximum spin speed
    public float ballRadius = 0.11f; // Assuming a radius of 0.11 meters for the football
    public int trajectorySteps = 10; // Number of steps to predict the trajectory
    public float timeStep = 0.3f; // Time step for each prediction step
    public GameObject ballPrefab; // Reference to the ball prefab

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetTarget(Vector3 targetPosition, Vector3 dragVector)
    {
        Vector3 direction = targetPosition - transform.position;

        rb.velocity = direction * speedMultiplier;
        rb.angularVelocity += Vector3.Cross(Vector3.up, dragVector.normalized) * maxSpinSpeed;
    }

    public Vector3 GetPosition() 
    {
        return transform.position;
    }

    public Vector3[] PredictTrajectory(Vector3 initialPosition, Vector3 targetPosition, Vector3 dragVector)
    {
        var previousSimulationMode = Physics.simulationMode;
        Physics.simulationMode = SimulationMode.Script;

        Vector3[] trajectoryPoints = new Vector3[trajectorySteps];

        // Create a duplicate ball for simulation
        GameObject tempBall = Instantiate(ballPrefab, initialPosition, Quaternion.identity);
        Rigidbody tempRb = tempBall.GetComponent<Rigidbody>();
        Vector3 originalVel = rb.velocity;
        Vector3 originalAngularVel = rb.angularVelocity;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 direction = targetPosition - transform.position;
        Vector3 velocity = direction * speedMultiplier;
        Vector3 angularVelocity = Vector3.Cross(Vector3.up, dragVector.normalized) * maxSpinSpeed;

        tempRb.velocity = direction * speedMultiplier;
        tempRb.angularVelocity = originalAngularVel + angularVelocity;

        for (int i = 0; i < trajectorySteps; i++)
        {
            trajectoryPoints[i] = tempBall.transform.position;
            Physics.Simulate(timeStep); // Simulate physics for each time step
        }

        // restore original ball phase after trajectory simulation
        transform.position = initialPosition;
        rb.velocity = originalVel;
        rb.angularVelocity = originalAngularVel;

        Destroy(tempBall); // Destroy the temporary ball after prediction

        Physics.simulationMode = previousSimulationMode;

        return trajectoryPoints;
    }

}
