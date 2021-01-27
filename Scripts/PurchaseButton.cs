using UnityEngine;

public class PurchaseButton : MonoBehaviour
{
    public enum PurchaseType {passes1, passes2, passes5, passes10 };
    public PurchaseType purchaseType;

    public void ClickPurchaseButton() {
        switch (purchaseType) {
            case PurchaseType.passes1:
                IAPManager.instance.BuyPasses1();
                break;
            case PurchaseType.passes2:
                IAPManager.instance.BuyPasses2();
                break;
            case PurchaseType.passes5:
                IAPManager.instance.BuyPasses5();
                break;
            case PurchaseType.passes10:
                IAPManager.instance.BuyPasses10();
                break;
        }
    }
}
