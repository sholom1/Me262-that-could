using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Difficulty Settings", menuName = "Difficulty")]
public class DifficultySettingsSO : ScriptableObject
{
    public int MaxEnemies;
    [Range(1, 2)]
    public float EnemyHealthIncrease;
    [Range(1, 2)]
    public float EnemyDamageIncrease;
    public float ScoreMultiplier;
    public float PlayerHealthMultiplier;
}
