using UnityEngine;

public class WateringCan : Tool
{
    public override bool OnUse(PlayerController player, Ray aimRay)
    {
        if (Physics.SphereCast(player.AimRay.origin, radius: SphereCastSize, player.AimRay.direction, out var hit, 500, layerMask: LayerMask.GetMask(Layers.Hitbox)))
        {

        }
        return true;
    }
}