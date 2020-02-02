using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Galeon/LevelSettings", fileName = "GameLevelValues")]
public class GameLevelSettingsSO : ScriptableObject
{
    public float levelTime = 120f;

    [Header("Eneny manager")]
    public const int numParts = 3;

    public int[] maxEnemies;

    public float startWaitingTime = 2.0f;
    public float[] waitingTime;

    public int[] numDecks;

}
