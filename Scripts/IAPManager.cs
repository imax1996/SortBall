using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener {

    public static IAPManager instance;

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    private string passes1 = "com.imaxworld.passes_1";
    private string passes2 = "com.imaxworld.passes_2";
    private string passes5 = "com.imaxworld.passes_5";
    private string passes10 = "com.imaxworld.passes_10";

    public GameObject gameui;
    private GameUI insgameui;

    private void Awake()
    {
        TestSingleton();
    }

    private void TestSingleton()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (m_StoreController == null) {
            InitializePurchasing();
        }

        insgameui = gameui.GetComponent<GameUI>();
    }

    public void InitializePurchasing() {
        if (IsInitialized()) {
            return;
        }
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(passes1, ProductType.Consumable);
        builder.AddProduct(passes2, ProductType.Consumable);
        builder.AddProduct(passes5, ProductType.Consumable);
        builder.AddProduct(passes10, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized() {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyPasses1()
    {
        BuyProductID(passes1);
    }
    public void BuyPasses2()
    {
        BuyProductID(passes2);
    }
    public void BuyPasses5()
    {
        BuyProductID(passes5);
    }
    public void BuyPasses10()
    {
        BuyProductID(passes10);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {
        if (String.Equals(args.purchasedProduct.definition.id, passes1, StringComparison.Ordinal))
        {
            Debug.Log("Passes1 Succesful");
            insgameui.BuyProductPasses(1);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, passes2, StringComparison.Ordinal))
        {
            Debug.Log("Passes2 Succesful");
            insgameui.BuyProductPasses(2);

        }
        else if (String.Equals(args.purchasedProduct.definition.id, passes5, StringComparison.Ordinal))
        {
            Debug.Log("Passes5 Succesful");
            insgameui.BuyProductPasses(5);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, passes10, StringComparison.Ordinal))
        {
            Debug.Log("Passes10 Succesful");
            insgameui.BuyProductPasses(10);
        }

        return PurchaseProcessingResult.Complete;
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    void BuyProductID(string productId) {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }


}
