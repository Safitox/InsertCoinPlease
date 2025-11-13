using System;
using UnityEngine;

public static class CameraEvents
{
    public static event Action<Camera> OnCameraRegistered;

    private static Camera _currentCamera;
    public static Camera CurrentCamera
    {
        get => _currentCamera;
        set
        {
            _currentCamera = value;
            OnCameraRegistered?.Invoke(_currentCamera);
        }
    }
}
