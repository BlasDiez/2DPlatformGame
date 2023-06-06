using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject character;
    [SerializeField] private Vector2 minCameraPosition;
    [SerializeField] private Vector2 maxCameraPosition;
    private float _xCamera;
    private float _yCamera;
    [SerializeField] private float cameraTransition;
    private Vector2 _auxVector2;

    private void FixedUpdate()
    {
        var cameraPosition = Camera.main.transform.position;
        var characterPosition = character.transform.position;
        _xCamera = Mathf.SmoothDamp(cameraPosition.x, characterPosition.x, ref _auxVector2.x, cameraTransition);
        _yCamera = Mathf.SmoothDamp(cameraPosition.y, characterPosition.y, ref _auxVector2.y, cameraTransition);
        cameraPosition = new Vector3(Math.Clamp(_xCamera, minCameraPosition.x, maxCameraPosition.x), Math.Clamp(_yCamera, minCameraPosition.y, maxCameraPosition.y), cameraPosition.z);
        Camera.main.transform.position = cameraPosition;
    }
}


