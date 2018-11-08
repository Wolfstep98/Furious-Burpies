using System;
using UnityEngine;

[Serializable]
public class TerrainData
{
    #region Fields & Properties
    [Header("Data")]
    [SerializeField]
    private GameObject prefab;
    public GameObject Prefab { get { return this.prefab; } }

    [Header("Platform Start Transition")]
    [SerializeField]
    private PlatformTransition platformStartTransition = null;
    public PlatformTransition PlatformStartTransition { get { return this.platformStartTransition; } }

    [Header("Platform End Transition")]
    [SerializeField]
    private PlatformTransition platformEndTransition = null;
    public PlatformTransition PlatformEndTransition { get { return this.platformEndTransition; } }
    #endregion
}

[Serializable]
public class PlatformTransition
{
    #region Fields & Properties
    [Header("Options")]
    [SerializeField]
    private PlatformTransitionOption[] transitionOption = new PlatformTransitionOption[1] { PlatformTransitionOption.None };
    public PlatformTransitionOption[] TransitionOption { get { return this.transitionOption; } }

    [Header("Main Transition")]
    [SerializeField]
    private float mainTransitionHeight = 0.0f;
    public float MainTransitionHeight { get { return this.mainTransitionHeight; } }

    [Header("Sub Transitions")]
    [SerializeField]
    private bool[] subTransitionsActive = new bool[0];
    public bool[] SubTransitionActive { get { return this.subTransitionsActive; } }

    [SerializeField]
    private float[] subTransitions = new float[0];
    public float[] SubTransitions { get { return this.subTransitions; } }
    #endregion
}

public enum PlatformTransitionOption
{
    None,
    Main,
    Subs,
    CertainSubs,
    MainAndCertainSubs,
    All
}