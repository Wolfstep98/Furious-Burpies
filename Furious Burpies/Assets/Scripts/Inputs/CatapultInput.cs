using System;
using UnityEngine;

public class CatapultInput : MonoBehaviour
{
    #region Fields & Properties
    [Header("Variables")]
    [SerializeField]
    private bool showDirection = true;
    [SerializeField]
    private bool playerLocked = false;

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

    [Header("References")]
    [SerializeField]
    private GameObject player = null;
    [SerializeField]
    private Camera mainCamera = null;
    [SerializeField]
    private LockBehaviour lockBehaviour = null;
    #endregion

    #region Methods
    private void Awake()
    {
#if UNITY_EDITOR
        if (this.player == null)
            Debug.LogError("[Missing Reference] player is not set !");
        if (this.mainCamera == null)
            Debug.LogError("[Missing Reference] mainCamera is not set !");
        if (this.lockBehaviour == null)
            Debug.LogError("[Missing Reference] lockBehaviour is not set !");
#endif
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            this.playerLocked = true;
            this.screenLockPos = Input.mousePosition;
            this.lockPos = this.mainCamera.ScreenToWorldPoint(Input.mousePosition);
            this.distanceBetweenLockAndFinger = 0.0f;
            this.lockBehaviour.Lock();
        }
        else if (Input.GetMouseButton(0))
        {
            this.lastScreenPos = Input.mousePosition;
            this.lastPos = this.mainCamera.ScreenToWorldPoint(Input.mousePosition);
            this.distanceBetweenLockAndFinger = Vector2.Distance(this.screenLockPos, Input.mousePosition);
            Vector2 direction = this.lockPos - this.lastPos;
            if(this.showDirection)
                Debug.DrawLine(this.player.transform.position, direction, Color.black);
            //Feedback trajectory
        }
        else if (Input.GetMouseButtonUp(0))
        {
            this.lockBehaviour.Unlock();
            this.playerLocked = false;
            Vector2 direction = this.lockPos - this.lastPos;
            this.player.GetComponent<Rigidbody2D>().AddForce(direction * 50, ForceMode2D.Force);
            Debug.Log("Jump in " + direction);
            Debug.DrawRay(this.player.transform.position, direction, Color.red, 10.00f);
            //Catapult !!!
        }
#else
        if(Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            if(!this.playerLocked)
            {
                this.playerLocked = true;
                this.screenLockPos = touch.position;
                this.lockPos = this.mainCamera.ScreenToWorldPoint(touch.position);
                this.distanceBetweenLockAndFinger = 0.0f;
                this.lockBehaviour.Lock();
            }
            else
            {
                this.lastScreenPos = touch.position;
                this.lastPos = this.mainCamera.ScreenToWorldPoint(touch.position);
                this.distanceBetweenLockAndFinger = Vector2.Distance(this.screenLockPos, touch.position);
                Vector2 direction = this.lockPos - this.lastPos;
                //Feedback trajectory
            }
        }
        else
        {
            if(this.playerLocked)
            {
                this.lockBehaviour.Unlock();
                this.playerLocked = false;
                Vector2 direction = this.lockPos - this.lastPos;
                this.player.GetComponent<Rigidbody2D>().AddForce(direction * 50, ForceMode2D.Force);
                Debug.Log("Jump in " + direction);
                Debug.DrawRay(this.player.transform.position, direction, Color.red, 10.00f);
                //Catapult !!!
            }
        }
#endif
    }
    #endregion
}
