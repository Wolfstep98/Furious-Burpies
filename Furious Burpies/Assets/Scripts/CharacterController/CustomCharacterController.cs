using System;
using System.Collections;
using UnityEngine;

public class CustomCharacterController : MonoBehaviour, IStickBehaviour, IBounceBehaviour, IFrictionBehaviour, ISlipBehaviour
{
    #region Fields & Properties
    [Header("States")]
    [SerializeField]
    private bool isGrounded = false;
    /// <summary>
    /// Is the character grounded ?
    /// </summary>
    public bool IsGrounded { get { return this.isGrounded; } }

    [SerializeField]
    private bool isStick = false;
    /// <summary>
    /// Is the character stick ?
    /// </summary>
    public bool IsStick { get { return this.isStick; } }

    [SerializeField]
    private bool isSlip = false;
    /// <summary>
    /// Is the character spliping ?
    /// </summary>
    public bool IsSlip { get { return this.isSlip; } }

    [Header("Gravity")]
    [SerializeField]
    private bool gravityEnabled = true;
    /// <summary>
    /// Is the gravity enabled ?
    /// </summary>
    public bool GravityEnabled { get { return this.gravityEnabled; } }

    [SerializeField]
    private float gravityForce = 9.81f;
    /// <summary>
    /// The current gravity that affects the character.
    /// </summary>
    public float GravityForce { get { return this.gravityForce; } set { this.gravityForce = value; } }

    [SerializeField]
    private float gravityMaxVelocity = 20.0f;
    /// <summary>
    /// The max force the gravity can be.
    /// </summary>
    public float GravityMaxVelocity { get { return this.gravityMaxVelocity; } }

    [SerializeField]
    private float gravityModifier = 1.0f;
    /// <summary>
    /// The current modifier applied to gravity.
    /// </summary>
    public float GravityModifier { get { return this.gravityModifier; } }

    [SerializeField]
    private Vector3 gravityApplied = Vector3.zero;
    /// <summary>
    /// The current velocity of the character.
    /// </summary>
    public Vector2 GravityApplied { get { return this.gravityApplied; } set { this.gravityApplied = value; } }

    [Header("Collisions")]
    [SerializeField]
    private bool showCollisions = true;
    [SerializeField]
    private bool showHitbox = true;
    [SerializeField]
    private int collisionLayerMask = 0;
    [SerializeField]
    private float hitboxRadius = 0.0f;
    [SerializeField]
    private Vector3 hitboxPosition = Vector2.zero;
    [SerializeField]
    private Vector2 normalHit = Vector2.zero;
    [SerializeField]
    private Vector3 hitPoint = Vector3.zero;

    [Header("RoofChecker")]
    [SerializeField]
    private bool showRoofCheckerDebug = true;
    [SerializeField]
    private float roofCheckerRadius = 0.25f;
    [SerializeField]
    private Vector3 roofCheckerPosition = Vector3.zero;

    [Header("Ground Checker")]
    [SerializeField]
    private bool showGroundCheckerDebug = true;
    [SerializeField]
    private float groundCheckerRadius = 0.25f;
    [SerializeField]
    private Vector3 groundCheckerPosition = Vector3.zero;

    [Header("Velocity")]
    [SerializeField]
    private bool showVelocity = true;
    [SerializeField]
    private Vector3 finalDirection = Vector3.zero;
    public Vector3 FinalDirection { get { return this.finalDirection; } }

    [SerializeField]
    private Vector3 direction = Vector3.zero;
    public Vector3 Direction { get { return this.direction; } }

    [Header("Bounce Behaviour")]
    [SerializeField]
    private float bounceThreshold = 0.5f;

    [Header("Stick behaviour")]
    [SerializeField]
    private float stickTime = 0.0f;
    public float StickTimer { get { return this.stickTime; } }
    [SerializeField]
    private float stickBehaviourTime = 1.5f;
    public float MaxStickTime { get { return this.stickBehaviourTime; } }


    [Header("Friction behaviour")]
    [SerializeField]
    private float frictionThreshold = 0.5f;

    [Header("Slip Behaviour")]


    [Header("References")]
    [SerializeField]
    private CharacterController characterController = null;
    #endregion

    #region Methods

    #region Initialization
    private void Awake()
    {
        this.Initialize();
    }

    private void Initialize()
    {
#if UNITY_EDITOR
        if (this.characterController == null)
            Debug.LogError("[Missing Reference] characterController not set !");
#endif

        this.collisionLayerMask = 1 << LayerMask.NameToLayer("CollisionWithPlayer");
        //this.collisionLayerMask ^= Int32.MaxValue;
    }
    #endregion

    #region MonoBehaviour
    public void CustomUpdate()
    {

    }

    public void CustomFixedUpdate()
    {
        //Test Better Behaviour

        //Check Behaviours
        this.UpdateStick();

        //Check Collisions
        //this.DetectCollisions();
        //this.DetectRoof();
        this.DetectGround();

        this.UpdateGravity();

        this.finalDirection = this.direction + this.gravityApplied;
        this.finalDirection.z = 0.0f;
        if (this.transform.position.z != 0.0f)
        {
            Vector3 position = this.transform.position;
            position.z = 0.0f;
            this.transform.position = position;
        }
        this.characterController.Move(this.finalDirection * GameTime.deltaTime);
    }
    #endregion

    #region Collisions
    private void DetectRoof()
    {
        if (!this.isGrounded)
        {
            RaycastHit info;
            if (Physics.SphereCast(this.transform.position, this.roofCheckerRadius, Vector3.up, out info, Math.Abs(this.roofCheckerPosition.y), this.collisionLayerMask))
            {
                Debug.Log("Roof Detected");
                this.OnCollision(info.collider.gameObject);
            }
        }
    }

    private void DetectGround()
    {
        if (this.IsGrounded)
        {
            RaycastHit info;
            if (Physics.Raycast(this.transform.position, ((this.transform.position + this.groundCheckerPosition) - this.transform.position).normalized, out info, Math.Abs(this.groundCheckerPosition.y), this.collisionLayerMask))
            {
                //this.normalHit = info.normal;
                //this.OnCollision(info.collider.gameObject);
                Debug.Log("Is grounded");
            }
            else
            {
                Debug.Log("Isn't grounded");
                this.isGrounded = false;
            }
        }
    }

    private void DetectCollisions()
    {
        //RaycastHit infos;
        //if (Physics.SphereCast(this.transform.position, this.hitboxRadius, Vector3.zero, out infos, 0.5f, this.collisionLayerMask))
        //{
        //    Debug.Log("SphereCast collision");
        //    this.normalHit = infos.normal;
        //    this.OnCollision(infos.collider.gameObject);
        //}
        //Collider[] colliders = Physics.OverlapSphere(this.transform.position + this.hitboxPosition, this.hitboxRadius, this.collisionLayerMask);
        //if(colliders != null && colliders.Length > 0)
        //{
        //    Debug.Log("OverlapSphere collision");
        //    this.OnCollision(colliders[0].gameObject);
        //}
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Collison with Character Controller : " + hit.collider.name);

        if (this.showCollisions)
        {
            Debug.DrawRay(hit.point, hit.normal, Color.black, 10.0f);
            Debug.DrawRay(this.transform.position, this.finalDirection, Color.blue, 10.0f);
        }

        GameObject obj = hit.collider.gameObject;
        this.normalHit = hit.normal;
        this.hitPoint = hit.point;

        this.OnCollision(obj);
    }

    private void OnCollision(GameObject obj)
    {
        Debug.Log("Checking collisions");

        //Get the component of the object we collided
        IStickProperty stickProperty = obj.GetComponent<IStickProperty>();
        IBounceProperty bounceProperty = obj.GetComponent<IBounceProperty>();
        IFrictionProperty frictionProperty = obj.GetComponent<IFrictionProperty>();
        ISlipProperty slipProperty = obj.GetComponent<ISlipProperty>();

        if (Math.Abs(Vector3.Angle(this.normalHit, this.finalDirection)) > 90.0f)
        {
            if (bounceProperty != null)
            {
                Debug.Log("Bounce");
                this.Bounce(new Collision(),bounceProperty);
            }
            else if (stickProperty != null && !this.isGrounded && !this.isStick)
            {
                Debug.Log("Stick");
                this.Stick(stickProperty);
            }
        }
        else if (frictionProperty != null && this.direction != Vector3.zero)
        {
            Debug.Log("Friction");
            this.Friction(frictionProperty);
        }
        if(slipProperty != null && !this.isSlip)
        {
            Debug.Log("Slip");
            this.Slip(slipProperty);
        }
    }

    #endregion

    private void UpdateGravity()
    {
        if (!this.gravityEnabled)
            return;

        if(!this.isGrounded && !this.isStick)
        {
            this.gravityApplied = new Vector3(0.0f, this.gravityApplied.y - this.gravityForce * this.gravityModifier * GameTime.deltaTime, 0.0f);

            if(this.gravityApplied.y < -this.gravityMaxVelocity)
            {
                this.gravityApplied = new Vector3(0.0f, -this.gravityMaxVelocity, 0.0f);
            }
        }
        else
        {
            this.gravityApplied = Vector3.zero;
        }
    }

    public void UpdateDirection(Vector3 direction, float forceApplied)
    {
        this.direction = direction * forceApplied;
    }

    #region Behaviours
    public void Bounce(Collision collision, IBounceProperty property)
    {
        if (property.IsEnable)
        {
            Vector2 vel = this.finalDirection;

            Vector2 result = vel - (2 * (Vector2.Dot(vel, this.normalHit)) * this.normalHit);

            if (Math.Abs(result.y) < this.bounceThreshold)
            {
                result.y = 0.0f;
                this.isGrounded = true;
            }
            else
            {
                this.isGrounded = false;
            }
            if (Math.Abs(result.x) < this.bounceThreshold)
            {
                result.x = 0.0f;
            }

            result *= property.BounceCoef;

            this.direction = result;
            this.gravityApplied = Vector3.zero;
            
            Debug.Log("vector : " + vel + " | result : " + result);
        }
    }

    public void Stick(IStickProperty property)
    {
        if (property.IsEnable)
        {
            int normalX = Mathf.RoundToInt(this.normalHit.x);
            int normalY = Mathf.RoundToInt(this.normalHit.y);

            if (Math.Abs(normalX) == 1 || normalY == -1)
            {
                this.stickTime = 0.0f;
                this.isStick = true;
            }
            else if (normalY == 1)
            {
                this.isGrounded = true;
            }
            else
            {
                this.stickTime = 0.0f;
                this.isStick = true;
            }

            //Vector3 offset = (this.transform.position - this.hitPoint);
            //this.transform.position = this.hitPoint + offset;
            this.direction = Vector3.zero;
        }
    }

    public void Friction(IFrictionProperty property)
    {
        if(Math.Abs(this.direction.x) > 0.0f)
        {
            if (this.direction.x > 0)
                this.direction.x -= (property.FrictionCoef.x * GameTime.deltaTime);
            else
                this.direction.x += (property.FrictionCoef.x * GameTime.deltaTime);

            if (Mathf.Abs(this.direction.x) < this.frictionThreshold)
                this.direction.x = 0.0f;
        }
    }

    public void Slip(ISlipProperty property)
    {
        //CalculateNewDirection
        float angle = Vector3.Angle(this.normalHit, this.finalDirection);
        Vector3 A = Vector3.zero;
        Vector3 B = this.normalHit;
        Vector3 M = this.finalDirection;
        float value = ((B.x - A.x) * (M.y - A.y) - (B.y - A.y) * (M.x - A.x)); // -1 is on the right, 1 is on the left, 0 is on the line.
        float sign = 0;
        if (value > 0)
            sign = 1;
        else
            sign = -1;
        Vector3 nextDirection = Vector3.Cross(this.normalHit, Vector3.forward).normalized * -sign;
        Debug.Log("Angle : " + angle + " | sign : " + sign + " | nextDirection : " + nextDirection);
        Debug.DrawRay(this.transform.position, nextDirection,Color.gray);
        this.direction = nextDirection * Math.Abs(this.finalDirection.x);
        this.gravityApplied.y = 0.0f;
        this.isGrounded = true;
        //this.isSlip = true;
    }

    #endregion

    //public void Bounce(Vector2 frictionCoef, Vector2 normal)
    //{
    //    Vector2 vel = this.finalDirection;

    //    Vector2 result = vel - (2 * (Vector2.Dot(vel, normal)) * normal);

    //    result *= frictionCoef;

    //    if (result.y < 1.0f)
    //    {
    //        result.y = 0.0f;
    //    }
    //    else
    //    {
    //        this.direction = result;
    //        this.gravityApplied = Vector3.zero;
    //        this.isGrounded = false;
    //    }

    //    Debug.Log("vector : " + vel + " | result : " + result);
    //}


    //public void Stick(Vector3 hitNormal)
    //{
    //    int normalX = Mathf.RoundToInt(hitNormal.x);
    //    int normalY = Mathf.RoundToInt(hitNormal.y);

    //    if(Math.Abs(normalX) == 1 || normalY == -1)
    //    {
    //        this.isStick = true;
    //    }
    //    if(normalY == 1)
    //    {
    //        this.isGrounded = true;
    //    }

    //    this.direction = Vector3.zero;
    //}

    private void UpdateStick()
    {
        if (this.isStick)
        {
            this.stickTime += GameTime.deltaTime;
            if (this.stickTime >= this.stickBehaviourTime)
            {
                this.isStick = false;
                this.stickTime = 0.0f;
            }
        }
    }

    public void CatapultFromGround()
    {
        this.transform.parent = null;
        this.isGrounded = false;
        this.isStick = false;
    }

    public void CatapultFromAir()
    {
        this.gravityApplied = Vector3.zero;
    }

    #region Debug
    private void OnDrawGizmosSelected()
    {
        //Roof checker
        if (this.showRoofCheckerDebug)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(this.transform.position + this.roofCheckerPosition, this.roofCheckerRadius);
        }

        //Ground checker
        if (this.showGroundCheckerDebug)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(this.transform.position, (this.transform.position + this.groundCheckerPosition) - this.transform.position);
            //Gizmos.DrawWireSphere(this.transform.position + this.groundCheckerPosition, this.groundCheckerRadius);
        }

        //Hitbox
        if (this.showHitbox)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(this.transform.position + this.hitboxPosition, this.hitboxRadius);
        }

        //Velocity
        if(this.showVelocity)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(this.transform.position, Vector3.right * this.finalDirection.x);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(this.transform.position, Vector3.up * this.finalDirection.y);
        }
    }

    public void Stick(Collision collision, IStickProperty property)
    {
        throw new NotImplementedException();
    }

    void IStickBehaviour.UpdateStick()
    {
        throw new NotImplementedException();
    }

    public void ResetStick()
    {
        throw new NotImplementedException();
    }

    #endregion
    #endregion
}
