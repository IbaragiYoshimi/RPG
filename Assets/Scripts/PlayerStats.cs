using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakePhysicalDamage(int _damage)
    {
        base.TakePhysicalDamage(_damage);

        player.DamageEffect();
        Debug.Log("Player was hitten and invoke hit knock back.");
    }


    protected override void Die()
    {
        base.Die();

        player.Die();

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }
}
