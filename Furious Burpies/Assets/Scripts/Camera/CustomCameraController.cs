using System;
using UnityEngine;

public class CustomCameraController : MonoBehaviour
{
    #region Fields & Properties
    [Header("Parameters")]
    [SerializeField]
    private Axis axis = Axis.X;
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
        Vector3 cameraPos = Vector3.zero;
        switch(this.axis)
        {
            case Axis.X:
                cameraPos = new Vector3(this.objToFollow.position.x, 0.0f, 0.0f);
                break;
            case Axis.Y:
                cameraPos = new Vector3(0.0f, this.objToFollow.position.y, 0.0f);
                break;
            case Axis.Z:
                cameraPos = new Vector3(0.0f, 0.0f, this.objToFollow.position.z);
                break;
            default:
                break;
        }
        this.currentPosition = cameraPos + offset;
    }
	#endregion
}
