﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Galeon/GameSettings", fileName ="GameSettings")]
public class GameSettingsSO : ScriptableObject
{
    public float playerHorSpeed = 2f;
    public float playerVerSpeed = 2f;
    public float pistolCooldown = 1f;
    public float swordCooldown = 1f;
    public int playerMaxHealth = 5;

    public float bulletSpeed = 15f;
    public float shootRecoil = 2.5f;
    public float stopTime = 0.3f;

    [Header("Ship damage")]
    public float damagePerMidObject = 0.5f;
    public float damagePerDestroyed = 1;
}
