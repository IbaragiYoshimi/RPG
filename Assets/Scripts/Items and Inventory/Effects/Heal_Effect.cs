using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal effect", menuName = "Data/Item effect/Heal effect")]
public class Heal_Effect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercent;

    // Be used in weapon effect.
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        // Get player stats.
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        // How much to heal.
        int healAmount = Mathf.RoundToInt(playerStats.GetMaxhealthValue() * healPercent);

        // Heal.
        playerStats.IncreaseHealthBy(healAmount);
    }

    // Be used in healing flask.
    public override void ExecuteEffect()
    {
        // Get player stats.
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        
        // How much to heal.
        int healAmount = Mathf.RoundToInt(playerStats.GetMaxhealthValue() * healPercent);

        // Heal.
        playerStats.IncreaseHealthBy(healAmount);
    }
}
