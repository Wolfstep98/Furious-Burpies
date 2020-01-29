using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class MainCameraAdapter : MonoBehaviour
{
    #region Fields & Properties
    [Header("Parameters")]
    [SerializeField]
    private float aspect = 1.0f;
	#endregion
	
	#region Methods
	#region Intialization
	private void Awake()
	{
		this.Initialize();
	}
	
	private void Initialize()
	{
        this.ComputeRatio();
	}
    #endregion

    private void Update()
    {
#if UNITY_EDITOR
        this.ComputeRatio();
#endif
    }

    private void ComputeRatio()
    {
        Camera.main.projectionMatrix = Matrix4x4.Ortho(-Camera.main.orthographicSize * aspect, Camera.main.orthographicSize * aspect, -Camera.main.orthographicSize, Camera.main.orthographicSize, Camera.main.nearClipPlane, Camera.main.farClipPlane);
    }
	#endregion
}
