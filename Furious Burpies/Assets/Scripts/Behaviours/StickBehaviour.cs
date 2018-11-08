using UnityEngine;

public interface IStickBehaviour
{
    void Stick(IStickProperty property);
}

/*
public class StickBehaviour : MonoBehaviour, IStickBehaviour
{
    #region Fields & Properties
    [Header("Properties")]
    [SerializeField]
    private CustomRigidbody2D customRigidbody2D = null;
    [SerializeField]
    private CustomCharacterController customCharacterController = null;
    #endregion

    #region Methods
    private void Awake()
    {
#if UNITY_EDITOR
        if (this.customRigidbody2D == null)
            Debug.LogError("[Missing Reference] - customRigidbody2D is missing !");
#endif
    }

    public void Stick(Vector3 hitNormal)
    {
        //this.transform.SetParent(parent.transform);
        //this.customRigidbody2D.Velocity = Vector2.zero;
        //this.customRigidbody2D.IsStick = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {    
        if (hit.gameObject.tag == GameObjectsTags.Wall)
        {
            customCharacterController.Stick(hit.normal);
            Debug.Log("Stick Collision Controller !!!");
        }
    }
    #endregion
}*/
