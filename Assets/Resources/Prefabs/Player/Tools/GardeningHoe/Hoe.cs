using UnityEngine;

public class Hoe : Tool
{
    public override bool OnPrimaryUse(PlayerController player, Ray aimRay)
    {
        if (!base.CanUse(player, aimRay))
            return false;

        if (!TryGetInteractable(player, out var hit, Layers.Hitbox))
            return false;

        if (hit.collider.gameObject == null)
            return false;

        if (hit.collider.gameObject.TryGetComponent<InteractionHitbox>(out var interact) && interact.Root.TryGetComponent<Planter>(out var planter))
        {
            if (planter.HasCrop || planter.Tilled)
                return false;

            planter.Till();
        }

        return base.OnPrimaryUse(player, aimRay);
    }

    public override bool OnSecondaryUse(PlayerController player, Ray aimRay)
    {
        if (!TryGetInteractable(player, out var hit, Layers.Hitbox))
            return false;

        if (hit.collider.gameObject == null)
            return false;

        if (hit.collider.gameObject.TryGetComponent<InteractionHitbox>(out var interact) && interact.Root.TryGetComponent<Planter>(out var planter))
        {
            if (!planter.HasCrop)
                return false;

            if (planter.Harvest())
                return base.OnSecondaryUse(player, aimRay);
        }

        return false;
    }

    public override void UpdateHud(PlayerController player, HudController hud)
    {
        hud.SetLeftClickText("till soil");
        hud.SetRightClickText("harvest plant");
    }
}