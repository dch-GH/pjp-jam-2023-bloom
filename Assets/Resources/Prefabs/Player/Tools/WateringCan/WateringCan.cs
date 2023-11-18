using UnityEngine;

public class WateringCan : Tool
{
    [SerializeField]
    private float _waterAmount = 5;

    [SerializeField]
    private float _coolDownSeconds = 10;
    private float _sinceWaterTime;

    public override bool OnUse(PlayerController player, Ray aimRay)
    {
        if (Time.time - _sinceWaterTime < _coolDownSeconds)
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
            _sinceWaterTime = Time.time;
            Debug.Log($"Watering crop: {planter.Crop}");
            return true;
        }

        return false;
    }
}