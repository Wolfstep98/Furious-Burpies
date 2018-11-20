using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomRigidbody : MonoBehaviour , IStickBehaviour
{
    #region Fields & Properties
    [Header("       Parameters")]
    [Header("Gravity")]
    [SerializeField]
    private bool isGrounded = false;
    public bool IsGrounded { get { return this.isGrounded; } }
    [SerializeField]
    private float gravityForce = 9.81f;
    [SerializeField]
    private float gravityMultiplicator = 1.0f;

    [Header("Ground Checker")]
    [SerializeField]
    private int raycastBitMask = 0;
    [SerializeField]
    private float groundCheckerDistance = 1.0f;
    [SerializeField]
    private Vector3 groundCheckerPosition = Vector3.zero;

    [Header("   Behaviours")]
    [Header("Stick")]
    [SerializeField]
    private bool isStick = false;
    public bool IsStick { get { return this.isStick; } }
    [SerializeField]
    private float stickTimer = 0.0f;
    public float StickTimer { get { return this.stickTimer; } }
    [SerializeField]
    private float maxStickTime = 1.5f;
    public float MaxStickTime { get { return this.maxStickTime; } }


    [Header("References")]
    [SerializeField]
	new private Rigidbody rigidbody;
    public Rigidbody Rigidbody { get { return this.rigidbody; } }

    [Header("Debug")]
    [SerializeField]
    private bool showGroundChecker = true;
    #endregion

    #region Methods
    #region Initialization
    //MonoBehaviour Awake
    private void Awake () 
	{
		this.Init();
	}
	
	//Initialize
	private void Init()
	{
#if UNITY_EDITOR
        if (this.rigidbody == null)
            Debug.LogError("[Missing Reference] - rigidbody is missing !");
#endif
        this.raycastBitMask = 1 << LayerMask.NameToLayer("CollisionWithPlayer");
    }
    #endregion

    #region Update
    public void CustomUpdate()
    {
        //Update Behaviours
        this.UpdateStick();
    }

    public void CustomFixedUpdate()
    {
        this.GroundCheck();
    }
    #endregion


    #region Collisions
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision !");
        //Stick Behaviour
        IStickProperty stickProperty = collision.gameObject.GetComponent<StickProperty>();
        if(stickProperty != null)
        {
            if (stickProperty.IsEnable)
            {
                Debug.Log("Stick !");
                this.Stick(collision, stickProperty);
            }
        }
    }

    private void GroundCheck()
    {
        RaycastHit raycastHit;
        Ray ray = new Ray(this.transform.position + this.groundCheckerPosition, Vector3.down);
        if(Physics.Raycast(ray, out raycastHit, this.groundCheckerDistance, this.raycastBitMask, QueryTriggerInteraction.UseGlobal))
        {
            this.isGrounded = true;
        }
        else
        {
            this.isGrounded = false;
        }
    }
    #endregion

    #region Behaviours

    #region Catapult
    public void CatapultFromGround()
    {
        this.isGrounded = false;
        this.ResetStick();
    }

    public void CatapultFromAir()
    {
        this.rigidbody.velocity = Vector3.zero;
    }
    #endregion

    #region Stick
    public void Stick(Collision collision, IStickProperty property)
    {
        this.isStick = true;
        this.rigidbody.useGravity = false;
        this.rigidbody.velocity = Vector3.zero;
    }

    public void UpdateStick()
    {
        if(this.isStick)
        {
            this.stickTimer += GameTime.deltaTime;
            if (this.stickTimer >= this.maxStickTime)
            {
                this.ResetStick();
            }
        }
    }

    /// <summary>
    /// Reset the initial value of the stick properties.
    /// </summary>
    public void ResetStick()
    {
        this.isStick = false;
        this.stickTimer = 0.0f;
        this.rigidbody.useGravity = true;
    }
    #endregion
    #endregion

    #region Debug
    private void OnDrawGizmosSelected()
    {
        if(this.showGroundChecker)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawRay(this.transform.position + this.groundCheckerPosition, Vector3.down * this.groundCheckerDistance);
        }
    }
    #endregion

    #endregion
}
