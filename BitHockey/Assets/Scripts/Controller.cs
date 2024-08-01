using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Controller class, enables user mouse input for player 1
/// </summary>
public class Controller : MonoBehaviour
{
    [SerializeField] private HPaddle paddleScript;
    [SerializeField] private RectTransform playSpaceL;
    private Canvas canvas;
    private Camera mainCamera;

    // inits
    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        mainCamera = Camera.main;
        paddleScript.SetPlaySpace(playSpaceL);
    }

    // get mouse position and move paddle to mouse position
    private void Update()
    {
        Vector2 mousePosition = GetMousePositionOnCanvas();
        paddleScript.Move(mousePosition);
    }

    // gets the mouses position on the canvas
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