using UnityEngine;

using VoxelBusters.EssentialKit;
using VoxelBusters.CoreLibrary;

namespace TechnologyWings
{

	public class InAppController : MonoBehaviour
	{
		public static InAppController Instance { get; private set; }

		void Awake()
		{
			if (Instance != null)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		private void OnEnable()
		{
			// register for events
			BillingServices.OnInitializeStoreComplete += OnInitializeStoreComplete;
			BillingServices.OnTransactionStateChange += OnDidFinishProductPurchase;
			BillingServices.OnRestorePurchasesComplete += OnDidFinishRestoringPurchases;
		}

		private void OnDisable()
		{
			// unregister from events
			BillingServices.OnInitializeStoreComplete -= OnInitializeStoreComplete;
			BillingServices.OnTransactionStateChange -= OnDidFinishProductPurchase;
			BillingServices.OnRestorePurchasesComplete -= OnDidFinishRestoringPurchases;
		}

		void Start()
		{

			if (BillingServices.IsAvailable())
			{
				BillingServices.InitializeStore();
				Debug.Log("In-app Purchases is initialized!");
			}
		}

		// Register for the BillingServices.OnInitializeStoreComplete event
		private void OnInitializeStoreComplete(BillingServicesInitializeStoreResult result, Error error)
		{

			if (error == null)
			{
				// update UI
				// show console messages
				var m_products = result.Products;
				Debug.Log("Store initialized successfully.");
				Debug.Log("Total products fetched: " + m_products.Length);
				Debug.Log("Below are the available products:");

				// for (int iter = 0; iter < m_products.Length; iter++) {
				// 	if( arrayOnInappButtons.Length > iter ) {
				// 		arrayOnInappButtons[iter].interactable = !BillingServices.IsProductPurchased(  m_products[iter] );
				// 		Debug.Log(string.Format("[{0}]: {1}", iter,  m_products[iter]));
				// 	}
				// }
			}

			else
			{
				Debug.Log("Store initialization failed with error. Error: " + error);
			}

			var invalidIds = result.InvalidProductIds;
			Debug.Log("Total invalid products: " + invalidIds.Length);
			if (invalidIds.Length > 0)
			{
				Debug.Log("Here are the invalid product ids:");

				for (int iter = 0; iter < invalidIds.Length; iter++)
				{
					Debug.Log(string.Format("[{0}]: {1}", iter, invalidIds[iter]));
				}
			}
		}

		//for the purchasing the product
		public void BuyInAppProduct(int item_id)
		{
			BuyProduct(BillingServices.Products[item_id]);
		}

		bool IsProductPurchased(IBillingProduct _product)
		{
			//product => "Product you got from OnInitializeStoreComplete event (result.Products)" 
			return BillingServices.IsProductPurchased(_product);
		}

		void BuyProduct(IBillingProduct _product)
		{

			if (BillingServices.CanMakePayments())
			{

				if (!IsProductPurchased(_product))
				{
					//product => "Product you got from OnInitializeStoreComplete event (result.Products)" 
					BillingServices.BuyProduct(_product);
				}
			}
		}

		void OnDidFinishProductPurchase(BillingServicesTransactionStateChangeResult result)
		{

			var transactions = result.Transactions;
			for (int iter = 0; iter < transactions.Length; iter++)
			{

				var transaction = transactions[iter];
				switch (transaction.TransactionState)
				{
					case BillingTransactionState.Purchased:
						Debug.Log(string.Format("Buy product with id:{0} finished successfully.", transaction.Payment.ProductId));

						//Give reward here - after a particular puduct is purchased
						//write your rewarding code in side that method so that restore method can also work 
						//for that product
						GiveRewardFor(transaction.Payment.ProductId);
						break;

					case BillingTransactionState.Failed:
						Debug.Log(string.Format("Buy product with id:{0} failed with error. Error: {1}", transaction.Payment.ProductId, transaction.Error));
						break;
				}
			}
		}

		//for restoring the product
		public void RestorePurchases()
		{
			BillingServices.RestorePurchases();
		}

		private void OnDidFinishRestoringPurchases(BillingServicesRestorePurchasesResult result, Error error)
		{
			if (error == null)
			{
				var transactions = result.Transactions;
				Debug.Log("Request to restore purchases finished successfully.");
				Debug.Log("Total restored products: " + transactions.Length);

				for (int iter = 0; iter < transactions.Length; iter++)
				{
					var transaction = transactions[iter];
					Debug.Log(string.Format("[{0}]: {1}", iter, transaction.Payment.ProductId));

					//unlocking the already purchased products
					GiveRewardFor(transaction.Payment.ProductId);
				}
			}
			else
			{
				Debug.Log("Request to restore purchases failed with error. Error: " + error);
			}
		}

		//for the reward method
		void GiveRewardFor(string id)
		{

			switch (id)
			{
				//Enter your remove ads ID
				case "removeads":

					//this will remove all showing banners
					//AdManager.HideAllBanners();
					//AdManager.RemoveAds(); //this will remove ads from the game

					//documentation link
					//https://assetstore.essentialkit.voxelbusters.com/native-ui/usage
					AlertDialog dialog = AlertDialog.CreateInstance();
					dialog.Title = "Congratulations";
					dialog.Message = "All advertisemetns have been removed except rewarded ads !";
					dialog.AddCancelButton("Ok", () => {
						Debug.Log("Ok button clicked");
					});
					dialog.Show(); //Show the dialog
					break;

				case "unlockall":

				
					//documentation link
					//https://assetstore.essentialkit.voxelbusters.com/native-ui/usage
					AlertDialog dialog1 = AlertDialog.CreateInstance();
					dialog1.Title = "Congratulations";
					dialog1.Message = "Congratulations!";
					dialog1.AddCancelButton("Ok", () => {
						Debug.Log("Ok button clicked");
					});
					dialog1.Show(); //Show the dialog
					break;

				case "unlockall_level":

				
					//documentation link
					//https://assetstore.essentialkit.voxelbusters.com/native-ui/usage
					AlertDialog dialog2 = AlertDialog.CreateInstance();
					dialog2.Title = "Congratulations";
					dialog2.Message = "Congratulations!";
					dialog2.AddCancelButton("Ok", () => {
						Debug.Log("Ok button clicked");
					});
					dialog2.Show(); //Show the dialog
					break;
			}
		}
	}

}