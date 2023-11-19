using UnityEngine;

public class ShopScreenController : MonoBehaviour
{
    public Vector3 BuyPoint;
    public GameObject BluepetalSeedsPrefab;
    public GameObject PurplepetalSeedsPrefab;
    public GameObject WhitepetalSeedsPrefab;
    public GameObject WateringCanPrefab;
    public GameObject RadiationShieldPrefab;

    public AudioSource Sound;

    void Update()
    {
        var mask = LayerMask.GetMask(Layers.Thing) & ~LayerMask.GetMask(Layers.Player);
        if (Physics.Raycast(PlayerController.Instance.AimRay, out var hit, PlayerController.InteractionDistance, mask) && hit.collider.gameObject == gameObject)
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                Buy(BluepetalSeedsPrefab, (int)StockMarket.Instance.GetStockPrice(CropId.BluePetal));
                return;
            }

            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                Buy(PurplepetalSeedsPrefab, (int)StockMarket.Instance.GetStockPrice(CropId.PurplePetal));
                return;
            }

            if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                Buy(WhitepetalSeedsPrefab, (int)StockMarket.Instance.GetStockPrice(CropId.WhitePetal));
                return;
            }

            if (Input.GetKeyUp(KeyCode.Alpha4))
            {
                Buy(WateringCanPrefab, 10);
                return;
            }

            if (Input.GetKeyUp(KeyCode.Alpha5))
            {
                Buy(RadiationShieldPrefab, 20);
                return;
            }
        }
    }

    private void Buy(GameObject bought, int price)
    {
        if (Player.Instance.Money < price)
            return;

        Sound.Stop();
        Sound.Play();
        var thing = Instantiate(bought, transform.position + BuyPoint, Quaternion.identity);
        thing.name = thing.name.Replace("(Clone)", string.Empty);
        Player.Instance.Money -= price;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + BuyPoint, 0.5f);
    }
}