using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Galeon/LevelSettings", fileName = "GameLevelValues")]
public class GameLevelSettingsSO : ScriptableObject
{
    public float levelTime = 120f;
    [Header("Eneny manager")]
    public int maxEnemies = 15;
    public float waitingTime = 6.0f;

    public float startWaitingTime = 6.0f;

    public int numDecks = 3;

}
