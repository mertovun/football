using UnityEngine;

public class BallUIHandler : MonoBehaviour
{
    public BallController ballController; // Reference to the BallController script
    public LineRenderer lineRenderer; // Reference to the LineRenderer for trajectory visualization

    private Camera mainCamera;
    private Vector3 mouseDownPosition;
    private Vector3 mouseUpPosition;
    private Vector3 dragVector;
    private bool mousePressed = false;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCurve();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndCurve();
        }
        else if (mousePressed)
        {
            UpdateCurve();
        }
    }

    void StartCurve()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            mouseDownPosition = hit.point + Vector3.up*2f;
            Debug.Log(hit.point);
            mousePressed = true;
            lineRenderer.positionCount = 0; // Reset the line renderer
        }
    }

    void EndCurve()
    {
        mousePressed = false;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 mouseUpPosition = hit.point + Vector3.up*2f;
            Debug.Log(hit.point);
            dragVector = mouseUpPosition - mouseDownPosition;

            // Inform the BallController of the target position and drag vector
            ballController.SetTarget(mouseDownPosition, dragVector);
        }
    }

    void UpdateCurve()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 currentMousePosition = hit.point + Vector3.up*2f;
            Vector3 ballPosition = ballController.GetPosition();
            dragVector = currentMousePosition - mouseDownPosition;

            // Predict the trajectory
            Vector3[] trajectoryPoints = ballController.PredictTrajectory(ballPosition, mouseDownPosition, dragVector);
            lineRenderer.positionCount = trajectoryPoints.Length;
            lineRenderer.SetPositions(trajectoryPoints);
        }
    }
}
