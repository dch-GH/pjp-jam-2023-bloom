
using UnityEngine;

public class SeedBag : Tool
{
    [SerializeField]
    private CropId _crop;

    [SerializeField]
    private GameObject _cropPrefab;

    [SerializeField]
    private int _numSeeds = 5;

    public override bool OnUse(PlayerController player, Ray aimRay)
    {
        if (!TryGetInteractable(player, out var hit, Layers.Hitbox))
            return false;

        if (hit.collider.gameObject == null)
            return false;

        if (hit.collider.gameObject.TryGetComponent<InteractionHitbox>(out var interact) && interact.Root.TryGetComponent<Planter>(out var planter))
        {
            if (planter.HasCrop)
                return false;

            var crop = Instantiate(_cropPrefab, planter.transform.position, Quaternion.identity).GetComponent<Crop>();
            planter.PlantCrop(crop);
            Debug.Log(crop);
            _numSeeds -= 1;
            if (_numSeeds <= 0)
            {
                Destroy(gameObject);
                return true;
            }

            return true;
        }

        return false;
    }
}