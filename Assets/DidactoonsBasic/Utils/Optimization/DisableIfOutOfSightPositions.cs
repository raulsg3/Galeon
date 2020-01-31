using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableIfOutOfSightPositions : MonoBehaviour
{
    public RectTransform topMostPos;
    public RectTransform bottomMostPos;
    public RectTransform leftMostPos;
    public RectTransform rightMostPos;

    private static DisableIfOutOfSightPositions s_instance = null;
    public static DisableIfOutOfSightPositions Instance
    {
        get
        {
            return s_instance;
        }
    }

    private void Awake()
    {
        s_instance = this;
    }
    private void OnDisable()
    {
        if (s_instance = this)
        {
            s_instance = null;
        }
    }

}
