using System;
using UnityEngine;

public class CustomCameraInput : MonoBehaviour
{
    #region Fields & Properties
    [Header("References")]
    [SerializeField]
    private CustomCameraController controller = null;
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
        if (this.controller == null)
            Debug.LogError("[Missing Reference] - controller is missing ! ");
#endif
    }
    #endregion

    public void CustomUpdate()
    {
        this.controller.UpdatePosition();   
    }
    #endregion
}
