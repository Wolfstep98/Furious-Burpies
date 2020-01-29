using System;
using UnityEngine;

public class CatapultInput : MonoBehaviour
{
    #region Fields & Properties
    [Header("Variables")]
    [SerializeField]
    private bool inputEnabled = true;
    public bool InputEnabled { set { this.inputEnabled = value; } }
    [SerializeField]
    private bool playerLocked = false;
    [SerializeField]
    private bool canCatapult = false;

    [SerializeField]
    private float maxInputDistance = 100;
    [SerializeField]
    private float velocityMultiplicator = 1.0f;

    [Header("Data")]
    [SerializeField]
    private Vector2 screenLockPos = Vector2.zero;
    [SerializeField]
    private Vector2 lockPos = Vector2.zero;

    [SerializeField]
    private Vector2 lastScreenPos = Vector2.zero;
    [SerializeField]
    private Vector2 lastPos = Vector2.zero;

    [SerializeField]
    private float distanceBetweenLockAndFinger = 0.0f;

    [SerializeField]
    private Vector2 predictedVelocity = Vector2.zero;

    [Header("References")]
    [Header("Player",order = 1)]
    [SerializeField]
    private GameObject player = null;
    [SerializeField]
    private CustomRigidbody2D customRigidbody2D = null;
    [SerializeField]
    private LockBehaviour lockBehaviour = null;
    [SerializeField]
    private SlowDownBehaviour slowDownBehaviour = null;
    [SerializeField]
    private LifeProperty lifeProperty = null;
    [SerializeField]
    private TrajectoryPrediction trajectoryPrediction = null;
    [SerializeField]
    private ShowTrajectory showTrajectory = null;
    [SerializeField]
    private Pause pause = null;

    [Space(5.0f)]
    [Header("Camera", order = 2)]
    [SerializeField]
    private Camera mainCamera = null;

    [Header("Debug")]
    [SerializeField]
    private bool showDirection = true;
    #endregion

    #region Methods

    #region Initialization
    private void Awake()
    {
#if UNITY_EDITOR
        if (this.player == null)
            Debug.LogError("[Missing Reference] player is not set !");
        if (this.mainCamera == null)
            Debug.LogError("[Missing Reference] mainCamera is not set !");
        if (this.customRigidbody2D == null)
            Debug.LogError("[Missing Reference] customRigidbody2D is not set !");
        if (this.lockBehaviour == null)
            Debug.LogError("[Missing Reference] lockBehaviour is not set !");
        if (this.slowDownBehaviour == null)
            Debug.LogError("[Missing Reference] slowDownBehaviour is not set !");
        if (this.pause == null)
            Debug.LogError("[Missing Reference] pause is not set !");
#endif
        this.Initiliaze();
    }

    private void Initiliaze()
    {
        this.maxInputDistance = Screen.width / 4.0f;

        this.trajectoryPrediction.Setup(this.customRigidbody2D.Gravity, this.customRigidbody2D.transform.position, Vector2.zero);
    }
    #endregion

    private void Update()
    {
        if (this.inputEnabled && !this.pause.IsPaused)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                if (this.customRigidbody2D.IsGrounded || this.customRigidbody2D.IsStick)
                {
                    this.screenLockPos = Input.mousePosition;
                    this.lockPos = this.mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    this.distanceBetweenLockAndFinger = 0.0f;

                    this.trajectoryPrediction.InitialPosition = this.customRigidbody2D.transform.position;

                    this.canCatapult = true;
                }
                else if (this.lifeProperty.Life > 0)
                {
                    this.screenLockPos = Input.mousePosition;
                    this.lockPos = this.mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    this.distanceBetweenLockAndFinger = 0.0f;

                    this.trajectoryPrediction.InitialPosition = this.customRigidbody2D.transform.position;

                    this.slowDownBehaviour.SlowDown();
                    this.canCatapult = true;
                }
                else
                {
                    this.canCatapult = false;
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (this.canCatapult)
                {
                    this.lastScreenPos = Input.mousePosition;
                    this.lastPos = this.mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    this.distanceBetweenLockAndFinger = Vector2.Distance(this.screenLockPos, Input.mousePosition);

                    //Clamp max distance
                    Vector2 screenDirection = (this.lastScreenPos - this.screenLockPos);
                    if (screenDirection.magnitude > this.maxInputDistance)
                    {
                        screenDirection.Normalize();
                        screenDirection *= this.maxInputDistance;
                        this.lastScreenPos = this.screenLockPos + screenDirection;
                        this.lastPos = mainCamera.ScreenToWorldPoint(this.lastScreenPos);
                    }

                    Vector2 direction = (this.lockPos - this.lastPos) * this.velocityMultiplicator;
                    if (this.showDirection)
                    {
                        Debug.DrawRay(this.player.transform.position, direction, Color.black);
                        Debug.DrawRay(this.lockPos, direction, Color.red);
                    }
                    //Feedback trajectory
                    if (this.slowDownBehaviour.IsTimeSlowDown)
                    {
                        this.trajectoryPrediction.InitialPosition = this.customRigidbody2D.transform.position;
                    }
                    this.predictedVelocity = direction;
                    this.trajectoryPrediction.InitialVelocity = this.predictedVelocity;
                    this.showTrajectory.ShowTrajectoryPrediction();
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (this.canCatapult)
                {
                    if (this.customRigidbody2D.IsGrounded || this.customRigidbody2D.IsStick)
                    {
                        this.customRigidbody2D.CatapultFromGround();
                    }
                    else
                    {
                        this.lifeProperty.TakeDamage(1);
                        this.slowDownBehaviour.RevertSlowDown();
                    }

                    this.lockBehaviour.Lock();
                    this.lockBehaviour.Unlock();

                    this.canCatapult = false;

                    //Clamp max distance
                    Vector2 screenDirection = (this.lastScreenPos - this.screenLockPos);
                    if(screenDirection.magnitude > this.maxInputDistance)
                    {
                        screenDirection.Normalize();
                        screenDirection *= this.maxInputDistance;
                    }
                    this.lastScreenPos = this.screenLockPos + screenDirection;
                    this.lastPos = mainCamera.ScreenToWorldPoint(this.lastScreenPos);

                    Vector2 direction = (this.lockPos - this.lastPos) * this.velocityMultiplicator;
                    //this.player.GetComponent<Rigidbody2D>().AddForce(direction * 50, ForceMode2D.Force);
                    this.player.GetComponent<CustomRigidbody2D>().Velocity = direction;
                    Debug.Log("Jump in " + direction);
                    Debug.DrawRay(this.lockPos, direction, Color.red, 10.00f);
                    Debug.DrawRay(this.player.transform.position, direction, Color.black);
                    //Catapult !!!

                    //this.showTrajectory.HideTrajectoryPrediction(0);
                }
            }
#else
            if (Input.touchCount > 0)
            {
                Touch touch = Input.touches[0];
                if (!this.playerLocked)
                {
                    if (this.customRigidbody2D.IsGrounded || this.customRigidbody2D.IsStick)
                    {
                        this.playerLocked = true;
                        this.screenLockPos = touch.position;
                        this.lockPos = this.mainCamera.ScreenToWorldPoint(touch.position);
                        this.distanceBetweenLockAndFinger = 0.0f;
                        this.trajectoryPrediction.InitialPosition = this.customRigidbody2D.transform.position;
                    }
                    else if (this.lifeProperty.Life > 0)
                    {
                        this.playerLocked = true;
                        this.screenLockPos = touch.position;
                        this.lockPos = this.mainCamera.ScreenToWorldPoint(touch.position);
                        this.distanceBetweenLockAndFinger = 0.0f;

                        this.trajectoryPrediction.InitialPosition = this.customRigidbody2D.transform.position;

                        this.slowDownBehaviour.SlowDown();
                    }
                    else
                    {
                        this.playerLocked = false;
                    }
                }
                else
                {
                    if (this.playerLocked)
                    {
                        this.lastScreenPos = touch.position;
                        this.lastPos = this.mainCamera.ScreenToWorldPoint(touch.position);
                        this.distanceBetweenLockAndFinger = Vector2.Distance(this.screenLockPos, touch.position);

                        //Clamp max distance
                        Vector2 screenDirection = (this.lastScreenPos - this.screenLockPos);
                        if (screenDirection.magnitude > this.maxInputDistance)
                        {
                            screenDirection.Normalize();
                            screenDirection *= this.maxInputDistance;
                        }
                        this.lastScreenPos = this.screenLockPos + screenDirection;
                        this.lastPos = mainCamera.ScreenToWorldPoint(this.lastScreenPos);

                        Vector2 direction = (this.lockPos - this.lastPos) * this.velocityMultiplicator;
                        if (this.showDirection)
                        {
                            Debug.DrawRay(this.player.transform.position, direction, Color.black);
                            Debug.DrawRay(this.lockPos, direction, Color.red);
                        }
                        //Feedback trajectory
                        if (this.slowDownBehaviour.IsTimeSlowDown)
                        {
                            this.trajectoryPrediction.InitialPosition = this.customRigidbody2D.transform.position;
                        }
                        this.predictedVelocity = direction;
                        this.trajectoryPrediction.InitialVelocity = this.predictedVelocity;
                        this.showTrajectory.ShowTrajectoryPrediction();
                    }
                }
            }
            else
            {
                if (this.playerLocked)
                {
                    if (this.customRigidbody2D.IsGrounded || this.customRigidbody2D.IsStick)
                    {
                        this.customRigidbody2D.CatapultFromGround();
                    }
                    else
                    {
                        this.lifeProperty.TakeDamage(1);
                        this.slowDownBehaviour.RevertSlowDown();
                    }

                    this.lockBehaviour.Lock();
                    this.lockBehaviour.Unlock();

                    this.playerLocked = false;

                    //Clamp max distance
                    Vector2 screenDirection = (this.lastScreenPos - this.screenLockPos);
                    if (screenDirection.magnitude > this.maxInputDistance)
                    {
                        screenDirection.Normalize();
                        screenDirection *= this.maxInputDistance;
                    }
                    this.lastScreenPos = this.screenLockPos + screenDirection;
                    this.lastPos = mainCamera.ScreenToWorldPoint(this.lastScreenPos);

                    Vector2 direction = (this.lockPos - this.lastPos) * this.velocityMultiplicator;
                    //this.player.GetComponent<Rigidbody2D>().AddForce(direction * 50, ForceMode2D.Force);
                    this.player.GetComponent<CustomRigidbody2D>().Velocity = direction;
                    Debug.Log("Jump in " + direction);
                    Debug.DrawRay(this.lockPos, direction, Color.red, 10.00f);
                    Debug.DrawRay(this.player.transform.position, direction, Color.black);
                    //Catapult !!!
                    this.showTrajectory.HideTrajectoryPrediction(0);
                }
            }
#endif
        }
    }

    public void Cancel()
    {
        this.showTrajectory.HideTrajectoryPrediction(0);
        this.slowDownBehaviour.RevertSlowDown();
        this.canCatapult = false;
        this.playerLocked = false;
    }
    #endregion

    #region Debug
    public void ChangeVelocityMultiplicator(float value)
    {
        this.velocityMultiplicator = value;
    }
    #endregion
}
