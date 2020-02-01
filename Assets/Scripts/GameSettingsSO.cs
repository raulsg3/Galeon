﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Galeon/GameSettings", fileName ="GameSettings")]
public class GameSettingsSO : ScriptableObject
{
    public float playerHorSpeed = 2f;
    public float playerVerSpeed = 2f;

    public float bulletSpeed = 15f;
    public float shootRecoil = 2.5f;
    public float stopTime = 0.3f;
}
