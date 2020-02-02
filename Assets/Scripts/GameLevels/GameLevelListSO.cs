using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Galeon/LevelSettingsList", fileName = "LevelsList")]
public class GameLevelListSO : ScriptableObject
{
  public List<GameLevelSettingsSO> list;
}
