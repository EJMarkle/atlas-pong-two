using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    [SerializeField] private HPaddle paddleScript;
    [SerializeField] private RectTransform playSpaceL; // Add this line
    private Canvas canvas;
    private Camera mainCamera;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        mainCamera = Camera.main;
        paddleScript.SetPlaySpace(playSpaceL); // Add this line
    }

    private void Update()
    {
        Vector2 mousePosition = GetMousePositionOnCanvas();
        paddleScript.Move(mousePosition);
    }

    private Vector2 GetMousePositionOnCanvas()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out mousePos);
        return mousePos;
    }
}