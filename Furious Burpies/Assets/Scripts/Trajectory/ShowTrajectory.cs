using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrajectoryPrediction))]
public class ShowTrajectory : MonoBehaviour
{
    #region Fields & Properties
    [Header("Parameters")]
    [SerializeField]
    private bool isShowing = false;
    public bool IsShowing { get { return this.isShowing; } }

    [SerializeField]
    private float predictionRange = 2.0f;
    [SerializeField]
    private float interval = 0.2f;

    [Header("References")]
    [SerializeField]
    private TrajectoryPrediction trajectoryPrediction = null;

    [SerializeField]
    private GameObject[] predictionDots = new GameObject[0];

    #endregion

    #region Methods
    private void Awake()
    {
        this.Initialize();

        this.HideTrajectoryPrediction(0);
    }

    private void Initialize()
    {
#if UNITY_EDITOR
        if (this.trajectoryPrediction == null)
            Debug.LogError("[Missing Reference] - trajectory prediction is missing !");
        if (this.predictionDots.Length == 0)
            Debug.LogError("[Missing Reference] - predictionDots is empty !");
#endif
    }

    public void ShowTrajectoryPrediction()
    {
        this.isShowing = true;
        int dotIndex = 0;
        for (float i = this.interval; i < this.predictionRange && dotIndex < this.predictionDots.Length; i += this.interval, dotIndex++)
        {
            Vector2 position = this.trajectoryPrediction.CalculatePositionAtTime(i);
            this.predictionDots[dotIndex].transform.position = position;
            this.predictionDots[dotIndex].SetActive(true);
        }
        this.HideTrajectoryPrediction(dotIndex);
    }
    public void HideTrajectoryPrediction(int startIndex)
    {
        if (startIndex == 0)
            this.isShowing = false;
        for(int i = startIndex; i < this.predictionDots.Length;i++)
        {
            this.predictionDots[i].SetActive(false);
        }
    }
    #endregion
}
