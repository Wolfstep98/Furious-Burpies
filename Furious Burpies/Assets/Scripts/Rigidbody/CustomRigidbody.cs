using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomRigidbody : MonoBehaviour , IStickBehaviour, IBounceBehaviour
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
    [SerializeField]
    private LifeProperty lifeProperty = null;

    [Header("Debug")]
    [SerializeField]
    private bool showGroundChecker = true;
    [SerializeField]
    private bool showVelocity = true;
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
        if (this.lifeProperty == null)
            Debug.LogError("[Missing Reference] - lifeProperty is missing !");
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
        switch (collision.gameObject.tag)
        {
            case GameObjectsTags.Stick:
                //Stick Behaviour
                IStickProperty stickProperty = collision.gameObject.GetComponent<StickProperty>();
                if (stickProperty != null)
                {
                    if (stickProperty.IsEnable)
                    {
                        Debug.Log("Stick !");
                        this.Stick(collision, stickProperty);
                    }
                }
                break;
            case GameObjectsTags.Bounce:
                //Bounce Behaviour
                IBounceProperty bounceProperty = collision.gameObject.GetComponent<BounceProperty>();
                if (bounceProperty != null)
                {
                    if (bounceProperty.IsEnable)
                    {
                        Debug.Log("Bounce !");
                        this.Bounce(collision, bounceProperty);
                    }
                }
                break;
            case GameObjectsTags.Slip:
                //Slip Behaviour
                this.rigidbody.useGravity = false;
                break;
            default:
                break;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Collision exit!");
        switch (collision.gameObject.tag)
        {
            case GameObjectsTags.Slip:
                //Slip Behaviour
                this.rigidbody.useGravity = true;
                break;
            default:
                break;
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

    #region Triggers
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Contains(GameObjectsTags.PowerUp))
        {
            if(other.tag.Contains(GameObjectsTags.LifePowerUp))
            {
                IPowerUpLifeUp lifeUp = other.GetComponent<PowerUpLifeUp>();
                this.lifeProperty.AddLife(lifeUp.LifeAmount);
                Destroy(other.gameObject);
            }
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

    #region Bounce
    public void Bounce(Collision collision, IBounceProperty property)
    {
        int normalX = Mathf.RoundToInt(collision.contacts[0].normal.x);
        if (Math.Abs(normalX) == 1)
        {
            Debug.Log("Force added");
            this.rigidbody.AddForce(Vector3.up * property.UpwardForceAdded, ForceMode.Impulse);
        }
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
        if(this.showVelocity)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawRay(this.transform.position, this.rigidbody.velocity);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(this.transform.position, this.rigidbody.velocity.x * Vector3.right);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(this.transform.position, this.rigidbody.velocity.y * Vector3.up);
        }
    }
    #endregion

    #endregion
}
