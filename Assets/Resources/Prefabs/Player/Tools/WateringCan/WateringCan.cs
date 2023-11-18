using UnityEngine;

public class WateringCan : Tool
{
    [SerializeField]
    private float _waterAmount = 5;


    public override bool OnUse(PlayerController player, Ray aimRay)
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

            planter.Crop.Water(_waterAmount);
            Debug.Log($"Watering crop: {planter.Crop}");
            return base.OnUse(player, aimRay);
        }

        return false;
    }
}