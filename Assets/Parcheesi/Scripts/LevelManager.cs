using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Transform groundPlane;
    public static LevelManager Instance;
    private void Awake()
    {
        Instance = this;
    }
}
