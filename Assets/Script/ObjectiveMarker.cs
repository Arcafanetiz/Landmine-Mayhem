using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveMarker : MonoBehaviour
{
    // Attach GameObjects
    [Header("Attach GameObjects")]
    [SerializeField] private Transform canvasTransform;     // Canvas transform
    [SerializeField] private Transform objective;           // Objective transform
    [SerializeField] private Transform onScreen;            // On Screen marker transform
    [SerializeField] private TextMeshProUGUI distanceLabel; // Distance label in the marker
    [SerializeField] private GameObject arrowPointer;       // Arrow pointer in the marker

    // Opacity Speed Settings
    [Header("All Opacity Speed Settings")]
    [SerializeField] private float opacitySpeed = 5f; // Speed of UI object changing opacity

    // Interface Settings
    [Header("Interface Settings")]
    [SerializeField] private float heightOffset = 1f;   // The height offset of the marker
    [Range(0.1f, 0.5f)]
    [SerializeField] private float ellipseSize = 0.4f;  // The size of eclipse from 0.1 to 0.5 beginning from center point

    // Reference Variables
    private Camera mainCamera;    // The main camera
    private RectTransform marker; // Marker recttransform

    // Internal Variables
    private float currentArrowAlpha = 0;    // Current alpha of distance label
    private float currentLabelAlpha = 0;    // Current alpha of distance label
    private Vector2 normalizedPosition;     // Position of the marker
    private bool outsideBound;              // Marker outside ellipse

    // Constant Variables
    private const float HALVE = 0.5f;  // Half value number
    private const float QUARTER = 90f; // 90 Degree number
    private const string DIST_FORMAT = "F0";    // Format for ToString distance, remove decimals
    private const string METER_TEXT = "m";      // Meter Text for distance label
    private const string OBJECTIVECANVASTAG = "Objective Canvas";

    private void Awake()
    {
        if (canvasTransform == null)
        {
            canvasTransform = GameObject.FindWithTag(OBJECTIVECANVASTAG).transform;
        }
        mainCamera = Camera.main;
        marker = onScreen.GetComponent<RectTransform>();
        onScreen.SetParent(canvasTransform, false);

        UpdateMarkerPosition();
        UpdateMarkerArrow();
        UpdateDistanceLabel();
    }
    private void Update()
    {
        UpdateMarkerPosition();
        UpdateMarkerArrow();
        UpdateDistanceLabel();
    }

    private void OnDestroy()
    {
        Destroy(onScreen.gameObject);
    }

    // Function to update marker's position
    private void UpdateMarkerPosition()
    {
        // Offset the objective height
        Vector3 offsetPosition = objective.position + (Vector3.up * heightOffset);

        // Calculate the direction from the camera to the objective
        Vector3 toObjective = offsetPosition - mainCamera.transform.position;

        // If the objective is behind the camera, project it onto the near clipping plane
        float dot = Vector3.Dot(toObjective, mainCamera.transform.forward);
        if (dot <= 0)
        {
            offsetPosition += (offsetPosition - mainCamera.transform.position).magnitude * mainCamera.transform.forward;
        }

        // Calculate the screen position of the objective
        Vector3 screenPos = mainCamera.WorldToViewportPoint(offsetPosition);

        // Calculate the normalized position within the ellipse
        normalizedPosition = new(
            (screenPos.x - HALVE) / ellipseSize,
            (screenPos.y - HALVE) / ellipseSize
        );

        // Calculate the clamped position within the ellipse
        float magnitude = normalizedPosition.magnitude;
        if (magnitude > 1)
        {
            normalizedPosition /= magnitude;
            outsideBound = true;
        }
        else
        {
            outsideBound = false;
        }

        // Update the marker's position
        marker.anchoredPosition = new Vector2(normalizedPosition.x * ellipseSize * Screen.width, normalizedPosition.y * ellipseSize * Screen.height);
    }
    // Function to update marker's arrow pointer
    private void UpdateMarkerArrow()
    {
        // Point arrow towards target
        float angle = (Mathf.Atan2(normalizedPosition.y, normalizedPosition.x) * Mathf.Rad2Deg) + QUARTER;
        arrowPointer.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Set alpha of arrow, visible when out of ellipse
        float targetAlpha;
        if (outsideBound)
        {
            targetAlpha = 1;
        }
        else
        {
            targetAlpha = 0;
        }
        currentArrowAlpha = Mathf.MoveTowards(currentArrowAlpha, targetAlpha, opacitySpeed * Time.deltaTime);
        arrowPointer.GetComponent<Image>().color = new Color(1, 1, 1, currentArrowAlpha);
    }
    // Function to update marker's arrow pointer
    private void UpdateDistanceLabel()
    {
        // Set distance label text from objective to camera
        distanceLabel.text = Vector3.Distance(mainCamera.transform.position, objective.position).ToString(DIST_FORMAT) + METER_TEXT;

        // Set alpha of distance label, visible when inside ellipse
        float targetAlpha;
        if (outsideBound)
        {
            targetAlpha = 0;
        }
        else
        {
            targetAlpha = 1;
        }
        currentLabelAlpha = Mathf.MoveTowards(currentLabelAlpha, targetAlpha, opacitySpeed * Time.deltaTime);
        distanceLabel.color = new Color(0, 0, 0, currentLabelAlpha);
    }
}