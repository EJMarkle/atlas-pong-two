using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIPlayer : MonoBehaviour
{
    [SerializeField] private HPaddle paddleScript;
    [SerializeField] private RectTransform playSpaceR;
    [SerializeField] private GameObject puck;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rightSideThreshold = 0f; // X-coordinate that determines the right side of the screen

    private RectTransform rectTransform;
    private Canvas canvas;
    private RectTransform puckRectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        puckRectTransform = puck.GetComponent<RectTransform>();
        paddleScript.SetPlaySpace(playSpaceR);
    }

    private void Update()
    {
        Vector2 targetPosition;

        if (IsPuckOnRightSide())
        {
            // Offensive mode: move towards the puck
            targetPosition = new Vector2(puckRectTransform.anchoredPosition.x, puckRectTransform.anchoredPosition.y);
        }
        else
        {
            // Defensive mode: stay on the right, mirror puck's Y position
            float rightX = playSpaceR.anchoredPosition.x + (playSpaceR.rect.width / 2) - paddleScript.boundaryPadding;
            targetPosition = new Vector2(rightX, puckRectTransform.anchoredPosition.y);
        }

        paddleScript.Move(targetPosition);
    }

    private bool IsPuckOnRightSide()
    {
        return puckRectTransform.anchoredPosition.x > rightSideThreshold;
    }
}