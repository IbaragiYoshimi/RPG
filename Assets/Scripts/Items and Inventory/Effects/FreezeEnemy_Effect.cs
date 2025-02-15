using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze enemies effect", menuName = "Data/Item effect/Freeze enemies")]
public class FreezeEnemy_Effect : ItemEffect
{
    [SerializeField] private float duration;

    // Armor effect, when player's health less than 10%, call it.
    public override void ExecuteEffect(Transform _playerTransform)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        
        // When HP less than 10% of MaxHealth, use this effect automatically.
        if (playerStats.currentHealth > playerStats.GetMaxhealthValue() * 0.1f)
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_playerTransform.position, 2);

        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>()?.FreezeTimerFor(duration);
            }
        }
    }
}
