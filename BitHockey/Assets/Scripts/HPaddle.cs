using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HPaddle : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float boundaryPadding = 20f;

    private RectTransform rectTransform;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void Move(Vector2 targetPosition)
    {
        Vector2 newPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);
        newPosition = ClampPositionToCanvas(newPosition);
        rectTransform.anchoredPosition = newPosition;
    }

    private Vector2 ClampPositionToCanvas(Vector2 position)
    {
        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;
        float clampedX = Mathf.Clamp(position.x, -canvasSize.x / 2 + boundaryPadding, canvasSize.x / 2 - boundaryPadding);
        float clampedY = Mathf.Clamp(position.y, -canvasSize.y / 2 + boundaryPadding, canvasSize.y / 2 - boundaryPadding);
        return new Vector2(clampedX, clampedY);
    }
}