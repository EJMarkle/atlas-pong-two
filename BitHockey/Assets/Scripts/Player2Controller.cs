using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player2Controller : MonoBehaviour
{
    [SerializeField] private HPaddle paddleScript;
    [SerializeField] private string horizontalAxis = "Horizontal2";
    [SerializeField] private string verticalAxis = "Vertical2";
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private RectTransform playSpaceR; // Add this line

    private RectTransform rectTransform;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        paddleScript.SetPlaySpace(playSpaceR); // Add this line
    }

    private void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis));
        Vector2 currentPosition = rectTransform.anchoredPosition;
        Vector2 targetPosition = currentPosition + input * moveSpeed * Time.deltaTime;
        
        paddleScript.Move(targetPosition);
    }
}