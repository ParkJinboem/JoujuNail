//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Purchasing;
//using UnityEngine.Purchasing.Security;

//public class IAPEvent
//{
//    public delegate void InitedHandler(bool isSuccess);
//    public static event InitedHandler OnInited;
//    public static void Inited(bool isSuccess)
//    {
//        OnInited?.Invoke(isSuccess);
//    }

//    public delegate void PurchasedHandler(Product product, bool isSuccess);
//    public static event PurchasedHandler OnPurchased;
//    public static void Purchased(Product product, bool isSuccess)
//    {
//        OnPurchased?.Invoke(product, isSuccess);
//    }

//    public delegate void RestoredHandler(bool isSuccess);
//    public static event RestoredHandler OnRestored;
//    public static void Restored(bool isSuccess)
//    {
//        OnRestored?.Invoke(isSuccess);
//    }

//    public delegate void CompletedHandler();
//    public static event CompletedHandler OnCompleted;
//    public static void Completed()
//    {
//        OnCompleted?.Invoke();
//    }
//}

//public abstract class IAPManager : MonoBehaviour, IStoreListener
//{
//    [Serializable]
//    public class AppProduct
//    {
//        public string name;
//        public string android;
//        public string iOS;
//        public ProductType productType;
//    }

//    public static IAPManager Instance = null;

//    private IStoreController m_StoreController;          // Unity 구매 시스템
//    private IExtensionProvider m_StoreExtensionProvider; // 상점 구매 하위 시스템 
//    private CrossPlatformValidator m_Validator;
//    private bool isRestore = false; // 구매 복구 진행 상태

//    protected List<AppProduct> appProducts = new List<AppProduct>();
//    public List<AppProduct> AppProducts
//    {
//        get { return appProducts; }
//    }

//    protected abstract bool InternetReachable();
//    protected abstract void InitAppProducts();
//    protected abstract void InitSuccess();
//    protected abstract void InitFail(string errorMsg);
//    protected abstract void PurchaseSuccess(Product product);
//    protected abstract void PurchaseFail(string errorMsg);
//    protected abstract void RestoreSuccess();
//    protected abstract void RestoreFail(string errorMsg);

//    private void Awake()
//    {
//        if (Instance)
//        {
//            Destroy(gameObject);
//        }
//        else
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//    }

//    public void Init()
//    {
//        InitAppProducts();
//        InitializePurchasing();
//    }

//    public void Clear()
//    {
//        m_StoreController = null;
//        m_StoreExtensionProvider = null;
//    }

//    private void InitializePurchasing()
//    {
//        if (IsInitialized())
//        {
//            InitSuccess();
//            return;
//        }

//        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
//        for (int i = 0; i < appProducts.Count; i++)
//        {
//            builder.AddProduct(appProducts[i].name, appProducts[i].productType, new IDs()
//            {
//                { appProducts[i].android, GooglePlay.Name },
//                { appProducts[i].iOS, AppleAppStore.Name },
//            });
//        }

//        UnityPurchasing.Initialize(this, builder);
//    }

//    private bool IsInitialized()
//    {
//        return m_StoreController != null && m_StoreExtensionProvider != null;
//    }

//    public void BuyProductId(string productId)
//    {
//        if (!InternetReachable())
//        {
//            return;
//        }

//        if (IsInitialized())
//        {
//            Product product = m_StoreController.products.WithID(productId);
//            if (product != null && product.availableToPurchase)
//            {
//                m_StoreController.InitiatePurchase(product);
//            }
//            else
//            {
//                PurchaseFail("BuyProductId: Not purchasing product, either is not found or is not available for purchase");
//            }
//        }
//        else
//        {
//            PurchaseFail("BuyProductId: Not initialized.");
//        }
//    }

//    public void Restore()
//    {
//        if (!InternetReachable())
//        {
//            return;
//        }

//        if (isRestore)
//        {
//            return;
//        }
//        isRestore = true;

//        Clear();
//        Init();
//    }

//    private void RestorePurchases()
//    {
//        if (!IsInitialized())
//        {
//            RestoreFail("Not initialized.");
//            isRestore = false;
//            return;
//        }

//        if (Application.platform == RuntimePlatform.IPhonePlayer ||
//            Application.platform == RuntimePlatform.OSXPlayer)
//        {
//            Debug.Log("RestorePurchases started ...");
//            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
//            apple.RestoreTransactions((result) =>
//            {
//                if (result)
//                {
//                    RestoreSuccess();
//                }
//                else
//                {
//                    RestoreFail("IOS RestoreTransactions error");
//                }
//                isRestore = false;
//            });
//        }
//        else
//        {
//            RestoreSuccess();
//            isRestore = false;
//        }
//    }

//    private void LogProduct(Product product)
//    {
//        Debug.Log("transactionID : " + product.transactionID);
//        Debug.Log("receipt : " + product.receipt);
//        Debug.Log("metadata isoCurrencyCode : " + product.metadata.isoCurrencyCode);
//        Debug.Log("metadata localizedDescription : " + product.metadata.localizedDescription);
//        Debug.Log("metadata localizedPrice : " + product.metadata.localizedPrice);
//        Debug.Log("metadata localizedPriceString : " + product.metadata.localizedPriceString);
//        Debug.Log("metadata localizedTitle : " + product.metadata.localizedTitle);
//        Debug.Log("hasReceipt : " + product.hasReceipt);
//        Debug.Log("availableToPurchase : " + product.availableToPurchase);
//        Debug.Log("enabled : " + product.definition.enabled);
//        Debug.Log("id : " + product.definition.id);
//        Debug.Log("payout : " + product.definition.payout);
//        Debug.Log("payouts : " + product.definition.payouts);
//        Debug.Log("storeSpecificId : " + product.definition.storeSpecificId);
//        Debug.Log("type : " + product.definition.type);
//    }

//    private void LogProducts(Product[] products)
//    {
//        foreach (Product product in products)
//        {
//            LogProduct(product);
//        }
//    }

//    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
//    {
//        m_StoreController = controller;
//        m_StoreExtensionProvider = extensions;
//        InitializeValidator();

//        LogProducts(m_StoreController.products.all);

//        if (isRestore)
//        {
//            RestorePurchases();
//        }
//        else
//        {
//            InitSuccess();
//        }
//    }

//    private void InitializeValidator()
//    {
//        if (IsCurrentStoreSupportedByValidator())
//        {
//            #if !UNITY_EDITOR
//                //var appleTangleData = AppleStoreKitTestTangle.Data(); // 샌드박스 결제 테스트
//                var appleTangleData = AppleTangle.Data();
//                m_Validator = new CrossPlatformValidator(GooglePlayTangle.Data(), appleTangleData, Application.identifier);
//            #endif
//        }
//    }

//    private bool IsCurrentStoreSupportedByValidator()
//    {
//        return IsGooglePlayStoreSelected() || IsAppleAppStoreSelected();
//    }

//    private bool IsGooglePlayStoreSelected()
//    {
//        var currentAppStore = StandardPurchasingModule.Instance().appStore;
//        return currentAppStore == AppStore.GooglePlay;
//    }

//    private bool IsAppleAppStoreSelected()
//    {
//        var currentAppStore = StandardPurchasingModule.Instance().appStore;
//        return currentAppStore == AppStore.AppleAppStore ||
//               currentAppStore == AppStore.MacAppStore;
//    }

//    public void OnInitializeFailed(InitializationFailureReason error)
//    {
//        InitFail(error.ToString());
//    }

//    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
//    {
//        var product = args.purchasedProduct;
//        LogProduct(product);

//        // 영수증 검증
//        if (IsPurchaseValid(product))
//        {
//            var result = m_Validator.Validate(product.receipt);
//            foreach (var receipt in result)
//            {
//                if (receipt is GooglePlayReceipt google)
//                {
//                    if ((int)google.purchaseState == 4)
//                    {
//                        // 결제 대기중
//                        return PurchaseProcessingResult.Pending;
//                    }
//                }
//            }

//            PurchaseSuccess(args.purchasedProduct);
//        }
//        else
//        {
//            PurchaseFail("ProcessPurchase: Invalid receipt, not unlocking content.");
//        }

//        return PurchaseProcessingResult.Complete;

//        // 이전 영수증 처리
//        //var isPurchaseValid = IsPurchaseValid(product);
//        //if (isPurchaseValid)
//        //{
//        //    PurchaseSuccess(args.purchasedProduct);
//        //}
//        //else
//        //{
//        //    PurchaseFail("ProcessPurchase: Invalid receipt, not unlocking content.");
//        //}

//        //return PurchaseProcessingResult.Complete;
//    }

//    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
//    {
//        PurchaseFail(string.Format("OnPurchaseFailed: Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
//    }

//    private bool IsPurchaseValid(Product product)
//    {
//        if (IsCurrentStoreSupportedByValidator())
//        {
//            try
//            {
//                var result = m_Validator.Validate(product.receipt);
//                LogReceipts(result);
//                return true;
//            }
//            catch (IAPSecurityException reason)
//            {
//                Debug.Log($"Invalid receipt: {reason}");
//                return false;
//            }
//        }
//        else
//        {
//            return false;
//        }
//    }

//    private void LogReceipts(IEnumerable<IPurchaseReceipt> receipts)
//    {
//        Debug.Log("Receipt is valid. Contents:");
//        foreach (var receipt in receipts)
//        {
//            LogReceipt(receipt);
//        }
//    }

//    private void LogReceipt(IPurchaseReceipt receipt)
//    {
//        Debug.Log($"Product ID: {receipt.productID}\n" +
//                  $"Purchase Date: {receipt.purchaseDate}\n" +
//                  $"Transaction ID: {receipt.transactionID}");

//        if (receipt is GooglePlayReceipt googleReceipt)
//        {
//            Debug.Log($"Purchase State: {googleReceipt.purchaseState}\n" +
//                      $"Purchase Token: {googleReceipt.purchaseToken}");
//        }

//        if (receipt is AppleInAppPurchaseReceipt appleReceipt)
//        {
//            Debug.Log($"Original Transaction ID: {appleReceipt.originalTransactionIdentifier}\n" +
//                      $"Subscription Expiration Date: {appleReceipt.subscriptionExpirationDate}\n" +
//                      $"Cancellation Date: {appleReceipt.cancellationDate}\n" +
//                      $"Quantity: {appleReceipt.quantity}");
//        }
//    }

//    public Product GetProduct(string productId)
//    {
//        Product product = null;
//        if (IsInitialized())
//        {
//            product = m_StoreController.products.WithID(productId);
//        }
//        return product;
//    }
//}