using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairFollow : MonoBehaviour
{
    RectTransform crosshairRect; // Crosshair rect transfrom
    RectTransform canvasRect; // Canvas rect transfrom

    // Start is called before the first frame update
    void Start()
    {
        crosshairRect = GetComponent<RectTransform>(); // Cache crosshair RectTransform
        var canvas = GetComponentInParent<Canvas>(); // Find the parent Canvas
        canvasRect = canvas.GetComponent<RectTransform>(); // Cache the Canvas's RectTransform
    }

    // Update is called once per frame
    void Update()
    {
        // Convert the mouse position into the Canvas's local space
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Input.mousePosition,
            null,
            out Vector2 localPoint))
        {
            // Move the crosshair to follow
            crosshairRect.anchoredPosition = localPoint;
        }
    }
}