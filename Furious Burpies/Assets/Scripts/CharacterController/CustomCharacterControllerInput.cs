﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCharacterControllerInput : MonoBehaviour 
{
    #region Fields & Properties
    [Header("Parameters")]
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
    [Header("Player", order = 1)]
    [SerializeField]
    private GameObject player = null;
    [SerializeField]
    private CustomCharacterController customCharacterController = null;
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
    [SerializeField]
    private bool showInputGamePosition = true;
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
        if (this.slowDownBehaviour == null)
            Debug.LogError("[Missing Reference] slowDownBehaviour is not set !");
        if (this.pause == null)
            Debug.LogError("[Missing Reference] pause is not set !");
#endif
        this.Initiliaze();
    }

    private void Initiliaze()
    {
        //this.maxInputDistance = Screen.width / 4.0f;
        this.maxInputDistance = Screen.height / 4.0f;

        this.trajectoryPrediction.Setup(-this.customCharacterController.GravityForce, this.customCharacterController.transform.position, Vector2.zero);
    }
    #endregion

    public void CustomUpdate()
    {
        if (this.inputEnabled && !this.pause.IsPaused)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                if (this.customCharacterController.IsGrounded || this.customCharacterController.IsStick)
                {
                    this.screenLockPos = Input.mousePosition;
                    this.lockPos = this.mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    this.distanceBetweenLockAndFinger = 0.0f;

                    this.trajectoryPrediction.InitialPosition = this.customCharacterController.transform.position;

                    this.canCatapult = true;
                }
                else if (this.lifeProperty.Life > 0)
                {
                    this.screenLockPos = Input.mousePosition;
                    this.lockPos = this.mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    this.distanceBetweenLockAndFinger = 0.0f;

                    this.trajectoryPrediction.InitialPosition = this.customCharacterController.transform.position;

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
                    if (!this.customCharacterController.IsGrounded && !this.customCharacterController.IsStick)
                    {
                        Vector3 temp = this.customCharacterController.FinalDirection * GameTime.deltaTime;
                        this.lockPos.x += temp.x;
                    }

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
                        if (this.customCharacterController.IsGrounded || this.customCharacterController.IsStick)
                        {
                            this.trajectoryPrediction.InitialPosition = this.customCharacterController.transform.position;
                            this.slowDownBehaviour.RevertSlowDown();
                        }
                        else
                        {
                            this.trajectoryPrediction.InitialPosition = this.customCharacterController.transform.position;
                        }
                    }
                    else
                    {
                        if(!this.customCharacterController.IsGrounded && !this.customCharacterController.IsStick)
                        {
                            this.slowDownBehaviour.SlowDown();
                            this.trajectoryPrediction.InitialPosition = this.customCharacterController.transform.position;
                        }
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
                    if (this.customCharacterController.IsGrounded || this.customCharacterController.IsStick)
                    {
                        //this.customRigidbody2D.CatapultFromGround(); //TODO
                        this.customCharacterController.CatapultFromGround();
                    }
                    else
                    {
                        this.lifeProperty.TakeDamage(1);
                        this.slowDownBehaviour.RevertSlowDown();
                        this.customCharacterController.CatapultFromAir();
                    }

                    this.canCatapult = false;

                    //Clamp max distance
                    Vector2 screenDirection = (this.lastScreenPos - this.screenLockPos);
                    if (screenDirection.magnitude > this.maxInputDistance)
                    {
                        screenDirection.Normalize();
                        screenDirection *= this.maxInputDistance;
                    }
                    this.lastScreenPos = this.screenLockPos + screenDirection;
                    this.lastPos = mainCamera.ScreenToWorldPoint(this.lastScreenPos);

                    Vector2 direction = (this.lockPos - this.lastPos); //* this.velocityMultiplicator;
                    //this.player.GetComponent<Rigidbody2D>().AddForce(direction * 50, ForceMode2D.Force);
                    //this.player.GetComponent<CustomRigidbody2D>().Velocity = direction;
                    this.customCharacterController.UpdateDirection(direction, this.velocityMultiplicator);
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
                    if (this.customCharacterController.IsGrounded || this.customCharacterController.IsStick)
                    {
                        this.screenLockPos = touch.position;
                        this.lockPos = this.mainCamera.ScreenToWorldPoint(touch.position);
                        this.distanceBetweenLockAndFinger = 0.0f;

                        this.trajectoryPrediction.InitialPosition = this.customCharacterController.transform.position;

                        this.playerLocked = true;
                    }
                    else if (this.lifeProperty.Life > 0)
                    {
                        this.screenLockPos = touch.position;
                        this.lockPos = this.mainCamera.ScreenToWorldPoint(touch.position);
                        this.distanceBetweenLockAndFinger = 0.0f;

                        this.trajectoryPrediction.InitialPosition = this.customCharacterController.transform.position;

                        this.slowDownBehaviour.SlowDown();
                        this.playerLocked = true;
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
                        if (!this.customCharacterController.IsGrounded && !this.customCharacterController.IsStick)
                        {
                            Vector3 temp = this.customCharacterController.FinalDirection * GameTime.deltaTime;
                            this.lockPos.x += temp.x;
                        }

                        this.lastScreenPos = touch.position;
                        this.lastPos = this.mainCamera.ScreenToWorldPoint(touch.position);
                        this.distanceBetweenLockAndFinger = Vector2.Distance(this.screenLockPos, touch.position);

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
                            if (this.customCharacterController.IsGrounded || this.customCharacterController.IsStick)
                            {
                                this.trajectoryPrediction.InitialPosition = this.customCharacterController.transform.position;
                                this.slowDownBehaviour.RevertSlowDown();
                            }
                            else
                            {
                                this.trajectoryPrediction.InitialPosition = this.customCharacterController.transform.position;
                            }
                        }
                        else
                        {
                            if (!this.customCharacterController.IsGrounded && !this.customCharacterController.IsStick)
                            {
                                this.slowDownBehaviour.SlowDown();
                                this.trajectoryPrediction.InitialPosition = this.customCharacterController.transform.position;
                            }
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
                    if (this.customCharacterController.IsGrounded || this.customCharacterController.IsStick)
                    {
                        this.customCharacterController.CatapultFromGround();
                    }
                    else
                    {
                        this.lifeProperty.TakeDamage(1);
                        this.slowDownBehaviour.RevertSlowDown();
                        this.customCharacterController.CatapultFromAir();
                    }

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

                    Vector2 direction = (this.lockPos - this.lastPos); //* this.velocityMultiplicator;
                    //this.player.GetComponent<Rigidbody2D>().AddForce(direction * 50, ForceMode2D.Force);
                    //this.player.GetComponent<CustomRigidbody2D>().Velocity = direction;
                    this.customCharacterController.UpdateDirection(direction, this.velocityMultiplicator);
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

    private void OnDrawGizmosSelected()
    {
        if (this.showInputGamePosition)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(this.lockPos, this.lastPos);
        }
    }
    #endregion
}
