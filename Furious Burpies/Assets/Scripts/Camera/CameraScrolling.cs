using System;
using UnityEngine;

public class CameraScrolling : MonoBehaviour
{
    #region Fields & Properties
    [SerializeField]
    private bool isScrolling = false;
    [Header("Parameters")]
    [SerializeField]
    private Axis scrollingAxis = Axis.X;
    [SerializeField]
    private float scrollingSpeed = 1.0f;
    [SerializeField]
    private Vector3 direction = Vector3.zero;
	#endregion
	
	#region Methods
	#region Intialization
	private void Awake()
	{
		this.Initialize();
	}
	
	private void Initialize()
	{
		
	}
    #endregion

    #region Enable/Disable
    public void StartScrolling()
    {
        this.isScrolling = true;
    }

    public void StopScrolling()
    {
        this.isScrolling = false;
    }
    #endregion

    #region Updates
    private void Update()
    {
        if(this.isScrolling)
        {
            switch(this.scrollingAxis)
            {
                case Axis.X:
                    this.direction = Vector3.right;
                    break;
                case Axis.Y:
                    this.direction = Vector3.up;
                    break;
                case Axis.Z:
                    this.direction = Vector3.forward;
                    break;
                default:
                    break;
            }
            this.transform.position += this.direction * this.scrollingSpeed * Time.deltaTime;
        }
    }
    #endregion

    public void UpdateSpeed(float value)
    {
        this.scrollingSpeed = value; 
    }

    #endregion
}
