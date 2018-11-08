using System;
using UnityEngine;

public class CustomCameraController : MonoBehaviour
{
    #region Fields & Properties
    [Header("Parameters")]
    [SerializeField]
    private Vector3 currentPosition = Vector3.zero;
    [SerializeField]
    private Vector3 offset = Vector3.zero;

    [Header("References")]
    [SerializeField]
    private Transform objToFollow = null;
	#endregion
	
	#region Methods
	#region Intialization
	private void Awake()
	{
		this.Initialize();
	}
	
	private void Initialize()
	{
#if UNITY_EDITOR
        if (this.objToFollow == null)
            Debug.LogError("[Missing Reference] - objToFollow is missing !");
#endif
    }
    #endregion

    public void CustomUpdate()
    {
        this.transform.position = this.currentPosition;
    }

    public void UpdatePosition()
    {
        Vector3 cameraPos = this.objToFollow.position;
        cameraPos.y = 0.0f;
        cameraPos.z = 0.0f;
        this.currentPosition = cameraPos + offset;
    }
	#endregion
}
