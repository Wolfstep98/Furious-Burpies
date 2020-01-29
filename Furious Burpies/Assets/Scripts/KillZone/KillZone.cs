using System;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    #region Fields & Properties
    [Header("Parameters")]
    [SerializeField]
    private bool isActive = false;
    [SerializeField]
    private bool invertDirection = false;
    [SerializeField]
    private float speed = 5.0f;
    [SerializeField]
    private Axis axis = Axis.X;
    [SerializeField]
    private Vector3 direction = Vector3.zero;

    [Header("References")]
    [SerializeField]
    private Transform startingPoint = null;
    [SerializeField]
    private Transform killZone = null;
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
        if (this.killZone == null)
            Debug.LogError("[Missing Reference] - killZone is missing ! ");
        if (this.startingPoint == null)
            Debug.LogError("[Missing Reference] - startingPoint is missing ! ");
# endif

        this.killZone.position = this.startingPoint.position;
        this.isActive = false;
    }
    #endregion

    public void ActivateKillZone()
    {
        if (!this.isActive)
        {
            this.isActive = true;
            this.direction = Vector3.zero;
        }
    }

    private void Update()
    {
        if (this.isActive)
        {
            switch(this.axis)
            {
                case Axis.X:
                    this.direction = (this.invertDirection) ? Vector3.left : Vector3.right;
                    break;
                case Axis.Y:
                    this.direction = (this.invertDirection) ? Vector3.down : Vector3.up;
                    break;
                case Axis.Z:
                    this.direction = (this.invertDirection) ? Vector3.back : Vector3.forward;
                    break;
                default:
                    break;
            }
            this.killZone.position += this.direction * this.speed * Time.deltaTime;
        }
    }

    #region Debug
    public void UpdateKillZoneSpeed(float value)
    {
        this.speed = value;
    }
    #endregion
    #endregion
}
