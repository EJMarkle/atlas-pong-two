using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HPaddle : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] public float boundaryPadding = 20f;
    [SerializeField] private RectTransform playSpace; // Add this line

    private RectTransform rectTransform;
    private Canvas canvas;

    private Vector2 lastPosition;
    private Vector2 currentVelocity;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    private void Start()
    {
        lastPosition = rectTransform.anchoredPosition;
    }

    public void Move(Vector2 targetPosition)
    {
        Vector2 newPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);
        newPosition = ClampPositionToPlaySpace(newPosition);
        rectTransform.anchoredPosition = newPosition;

        currentVelocity = (newPosition - lastPosition) / Time.deltaTime;
        lastPosition = newPosition;
    }

    public Vector2 GetCurrentVelocity()
    {
        return currentVelocity;
    }

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

    public void SetPlaySpace(RectTransform newPlaySpace)
    {
        playSpace = newPlaySpace;
    }
}