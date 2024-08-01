using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// HPaddle class, describes paddle movement and limits
/// </summary>
public class HPaddle : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] public float boundaryPadding = 20f;
    [SerializeField] private RectTransform playSpace;

    private RectTransform rectTransform;
    private Canvas canvas;

    private Vector2 lastPosition;
    private Vector2 currentVelocity;

    // inits
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    // init
    private void Start()
    {
        lastPosition = rectTransform.anchoredPosition;
    }

    // Logic for moving the paddles tranform 
    public void Move(Vector2 targetPosition)
    {
        Vector2 newPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);
        newPosition = ClampPositionToPlaySpace(newPosition);
        rectTransform.anchoredPosition = newPosition;

        currentVelocity = (newPosition - lastPosition) / Time.deltaTime;
        lastPosition = newPosition;
    }

    // gets velocity
    public Vector2 GetCurrentVelocity()
    {
        return currentVelocity;
    }

    // Stops paddles from moving outsode the playspace
    private Vector2 ClampPositionToPlaySpace(Vector2 position)
    {
        if (playSpace == null) return position;

        Vector3[] corners = new Vector3[4];
        playSpace.GetWorldCorners(corners);

        Vector2 min = canvas.transform.InverseTransformPoint(corners[0]);
        Vector2 max = canvas.transform.InverseTransformPoint(corners[2]);

        float clampedX = Mathf.Clamp(position.x, min.x + boundaryPadding, max.x - boundaryPadding);
        float clampedY = Mathf.Clamp(position.y, min.y + boundaryPadding, max.y - boundaryPadding);

        return new Vector2(clampedX, clampedY);
    }

    // Set playspace
    public void SetPlaySpace(RectTransform newPlaySpace)
    {
        playSpace = newPlaySpace;
    }
}