using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCharacterControllerManager : MonoBehaviour 
{
    #region Fields & Properties
    [Header("References")]
    [SerializeField]
    private CustomCharacterControllerInput input = null;
    [SerializeField]
    private CustomCharacterController controller = null;
	#endregion

	#region Methods
	#region Initializers
	private void Awake () 
	{
        this.Initialize();
	}

    private void Initialize()
    {
#if UNITY_EDITOR
        if (this.input == null)
            Debug.LogError("[Missing Reference] - input is not set !");
        if (this.controller == null)
            Debug.LogError("[Missing Reference] - controller is not set !");
#endif
    }
	#endregion
	
	private void Update () 
	{
        //Inputs
        this.input.CustomUpdate();

        //Controller
        this.controller.CustomUpdate();
	}

    private void FixedUpdate()
    {
        this.controller.CustomFixedUpdate();
    }
    #endregion
}
