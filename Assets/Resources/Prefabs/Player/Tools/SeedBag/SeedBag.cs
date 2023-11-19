
using UnityEngine;

public class SeedBag : Tool
{
    [SerializeField]
    private CropId _crop;

    [SerializeField]
    private GameObject _cropPrefab;

    [SerializeField]
    private int _numSeeds = 5;

    public override bool OnPrimaryUse(PlayerController player, Ray aimRay)
    {
        if (!TryGetInteractable(player, out var hit, Layers.Hitbox))
            return false;

        if (hit.collider.gameObject == null)
            return false;

        if (hit.collider.gameObject.TryGetComponent<InteractionHitbox>(out var interact) && interact.Root.TryGetComponent<Planter>(out var planter))
        {
            if (planter.HasCrop || !planter.Tilled)
                return false;

            var crop = Instantiate(_cropPrefab, planter.transform.position, Quaternion.identity).GetComponent<Crop>();
            planter.PlantCrop(crop);
            base.OnPrimaryUse(player, aimRay);
            _numSeeds -= 1;
            if (_numSeeds <= 0)
            {
                Destroy(gameObject, 0.25f);
                return true;
            }

            return true;
        }

        return false;
    }

    public override bool OnSecondaryUse(PlayerController player, Ray aimRay)
    {
        return false;
    }
}