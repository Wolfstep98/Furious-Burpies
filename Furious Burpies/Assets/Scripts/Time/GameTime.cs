using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    #region Fields & Properties
    [Header("Time")]
    [SerializeField]
    private static float _time = 0.0f;
    public static float time { get { return _time; } }

    [SerializeField]
    private static float _deltaTime = 0.0f;
    public static float deltaTime { get { return _deltaTime; } }

    [SerializeField]
    private static float _timeScale = 1.0f;
    public static float timeScale { get { return _timeScale; } set { _timeScale = value; } }
    #endregion

    #region Methods
    #region Constructors
    static GameTime()
    {
        _time = 0.0f;
        _deltaTime = 0.0f;
        _timeScale = 1.0f;
    }
    #endregion

    private void LateUpdate()
    {
        _deltaTime = Time.deltaTime * _timeScale;
        _time += _deltaTime;
    }
    #endregion
}
