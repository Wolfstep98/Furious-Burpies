using System;
using UnityEngine;

public class CustomRigidbodyManager : MonoBehaviour
{
    #region Fields & Properties
    [Header("References")]
    [SerializeField]
    private CustomRigidbodyInput input = null;
    [SerializeField]
    private CustomRigidbody controller = null;
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
        if (this.input == null)
            Debug.LogError("[Missing Reference] - input is missing !");
        if (this.controller == null)
            Debug.LogError("[Missing Reference] - controller is missing !");
#endif
    }
    #endregion

    private void Update()
    {
        this.input.CustomUpdate();

        this.controller.CustomUpdate();
    }

    private void FixedUpdate()
    {
        this.controller.CustomFixedUpdate();
    }
    #endregion
}
