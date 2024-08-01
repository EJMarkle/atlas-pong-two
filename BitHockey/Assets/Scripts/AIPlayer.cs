using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// AIPlayer class, gives computer control to other paddle
/// </summary>
public class AIPlayer : MonoBehaviour
{
    [SerializeField] private HPaddle paddleScript;
    [SerializeField] private RectTransform playSpaceR;
    [SerializeField] private GameObject puck;
    [SerializeField] private float yPositionDeadzone = 10f;
    private RectTransform rectTransform;
    private Canvas canvas;
    private RectTransform puckRectTransform;
    private Vector2 targetPosition;

    // inits
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        puckRectTransform = puck.GetComponent<RectTransform>();
        paddleScript.SetPlaySpace(playSpaceR);
    }

    // If puck in play space, attack mode. Otherwise defese mode
    private void Update()
    {
        if (IsPuckInPlaySpace())
        {
            // Attack mode: move left to intercept the puck
            if (rectTransform.anchoredPosition.x > puckRectTransform.anchoredPosition.x)
            {
                float puckY = puckRectTransform.anchoredPosition.y;
                float paddleY = rectTransform.anchoredPosition.y;

                if (Mathf.Abs(puckY - paddleY) > yPositionDeadzone)
                {
                    targetPosition = new Vector2(puckRectTransform.anchoredPosition.x, puckY);
                }
            }
            else
            {
                float rightX = playSpaceR.anchoredPosition.x + (playSpaceR.rect.width / 2) - paddleScript.boundaryPadding;
                targetPosition = new Vector2(rightX, rectTransform.anchoredPosition.y);
            }
        }
        else
        {
            // Defense mode: move right and mirror puck's Y position with a deadzone
            float rightX = playSpaceR.anchoredPosition.x + (playSpaceR.rect.width / 2) - paddleScript.boundaryPadding;
            float puckY = puckRectTransform.anchoredPosition.y;
            float paddleY = rectTransform.anchoredPosition.y;

            if (Mathf.Abs(puckY - paddleY) > yPositionDeadzone)
            {
                targetPosition = new Vector2(rightX, puckY);
            }
        }

        paddleScript.Move(targetPosition);
    }

    // check if puck is in playspace
    private bool IsPuckInPlaySpace()
    {
        Vector3 viewportPosition = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, puckRectTransform.position);
        return RectTransformUtility.RectangleContainsScreenPoint(playSpaceR, viewportPosition, canvas.worldCamera);
    }
}
