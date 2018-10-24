using UnityEngine;

public interface IStickBehaviour
{
    void Stick(GameObject parent);
}

public class StickBehaviour : MonoBehaviour, IStickBehaviour
{
    #region Fields & Properties
    [Header("Properties")]
    [SerializeField]
    private CustomRigidbody2D customRigidbody2D = null;
    #endregion

    #region Methods
    private void Awake()
    {
#if UNITY_EDITOR
        if (this.customRigidbody2D == null)
            Debug.LogError("[Missing Reference] - customRigidbody2D is missing !");
#endif
    }

    public void Stick(GameObject parent)
    {
        this.transform.SetParent(parent.transform);
        this.customRigidbody2D.Velocity = Vector2.zero;
        this.customRigidbody2D.IsStick = true;
    }
    #endregion
}
