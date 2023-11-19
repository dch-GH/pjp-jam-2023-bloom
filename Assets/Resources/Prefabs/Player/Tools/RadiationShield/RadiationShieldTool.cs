using UnityEngine;

public class RadiationShieldTool : Tool
{
    public override bool OnPrimaryUse(PlayerController player, Ray aimRay)
    {
        if (!TryGetInteractable(player, out var hit, Layers.Hitbox))
            return false;

        if (hit.collider.gameObject == null)
            return false;

        if (hit.collider.gameObject.TryGetComponent<InteractionHitbox>(out var interact) && interact.Root.TryGetComponent<Planter>(out var planter))
        {
            if (planter.HasRadiationShield)
                return false;

            planter.PlaceRadShield();
            return base.OnPrimaryUse(player, aimRay);
        }
        return false;
    }

    public override void UpdateHud(PlayerController player, HudController hud)
    {
        hud.SetLeftClickText("place radiation shield");
    }
}