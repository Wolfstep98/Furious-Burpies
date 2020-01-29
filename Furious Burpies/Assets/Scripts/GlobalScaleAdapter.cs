using System;
using UnityEngine;

public class GlobalScaleAdapter : MonoBehaviour
{
    #region Fields & Properties
    [Header("Parameters")]
    [SerializeField]
    private Vector2 initialScreenSize = Vector2.zero;
    [SerializeField]
    private Vector2 currentScreenSize = Vector2.zero;
    [SerializeField]
    private float widthRatio = 1.0f;
    [SerializeField]
    private float heightRatio = 1.0f;
    #endregion

    #region Methods
    #region Intialization
    private void Awake()
	{
		this.Initialize();
	}
	
	private void Initialize()
	{
        this.currentScreenSize = new Vector2(Screen.width, Screen.height);
        this.widthRatio = this.currentScreenSize.x / this.initialScreenSize.x;
        this.heightRatio = this.currentScreenSize.y / this.initialScreenSize.y;
        Vector3 scale = this.transform.localScale;
        scale.x *= this.widthRatio;
        scale.y *= this.heightRatio;
        this.transform.localScale = scale;
    }
	#endregion
	#endregion
}
