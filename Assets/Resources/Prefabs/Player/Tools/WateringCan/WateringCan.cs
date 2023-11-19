using UnityEngine;

public class WateringCan : Tool
{
    [SerializeField]
    private float _waterAmount = 0.05f;

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
            if (!planter.HasCrop)
                return false;

            if (planter.Crop.Water(_waterAmount))
            {
                Debug.Log($"Watering crop: {planter.Crop}");
                return base.OnPrimaryUse(player, aimRay);
            }
        }

        return false;
    }

    public override bool OnSecondaryUse(PlayerController player, Ray aimRay)
    {
        return false;
    }

    public override void UpdateHud(PlayerController player, HudController hud)
    {
        base.UpdateHud(player, hud);
        hud.SetLeftClickText("water crop");
    }
}