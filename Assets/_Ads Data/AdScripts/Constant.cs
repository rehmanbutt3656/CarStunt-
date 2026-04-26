using UnityEngine;
using System.Collections;
using GameAnalyticsSDK;
using UnityEngine.Analytics;
using System.Collections.Generic;

public delegate void BasicCallBackWithParams( int pValue );
public delegate void BasicCallBack();

public static class Constant  {

	
	/// <summary>
	/// Logs the business event.
	/// </summary>
	/// <param name="amount">Amount of Purchaseable in Cents i.e. 0.99$ = 99cents .</param>
	/// <param name="itemId">Give your inApp item Key..cncoins1 , cncoins2 etc </param>
	//public static void LogBusinessEvent(string inAppType,int amount, InAppItemName itemType, string itemId, string receipt , string signature ){
        
 //           #if UNITY_IPHONE
 //           GameAnalytics.NewBusinessEventIOS("USD", amount, "CoinsPack", itemId, "InAPP" , receipt);

 //   		LogResourceEvent (GameAnalyticsSDK.GAResourceFlowType.Source, Constant.Currency , GetCoinsForInApp(itemId) , InAPP , itemId);
 //   		#endif
 //   		#if UNITY_ANDROID
	//	Debug.Log("Business Is:: " + inAppType +":" + amount +":" +  itemId  );  
	//		GameAnalytics.NewBusinessEventGooglePlay("USD", amount, itemType.ToString (), itemId, "InAPP" , receipt, signature);
    		
	//	// }
 //   		#endif

	//}



	public static void LogDesignEvent(string eventName){
       
            #if UNITY_IPHONE
    		Debug.Log("event Is:: "+eventName);   
    		GameAnalytics.NewDesignEvent(eventName);
//		FB.LogAppEvent(eventName);
#endif

#if UNITY_ANDROID

    			Debug.Log("event Is:: "+eventName);   
    			GameAnalytics.NewDesignEvent(eventName);
    //FB.LogAppEvent(eventName);
#endif

	}

	//	public static void LogProgressionEvent(GAProgressionStatus status,string world, string point,string phase, int prize){

	//#if UNITY_IPHONE
	//    		//  Debug.Log("event Is:: "+eventName);   
	//    		//GameAnalytics.NewProgressionEvent(status, world, point, phase, prize);
	//    		Debug.Log("Prog:: "+status.ToString() +":"+ point+ CurrEP + ":"+ phase);   

	//    		GameAnalytics.NewProgressionEvent(status, world, point + currentEpisode, phase, prize);
	//#endif
	//#if UNITY_ANDROID

	//                Debug.Log("Prog:: "+status.ToString() +":"+ point+ currentEpisode + ":"+ phase);   

	//    			GameAnalytics.NewProgressionEvent(status, world, point + currentEpisode, phase, prize);


	//    		#endif

	//	}

	/// <summary>
	/// Logs the info of Virtual currency(Coins) Gained ( source ) or spent (sink) resource event.
	/// </summary>
	/// <param name="flowType">Flow type can be GAResourceFlowType.Sink in case of spending or GAResourceFlowType.Source in case of earning ..</param>
	/// <param name="currency">Currency = Pass Constant.currency </param>
	/// <param name="amount">Amount = coins amount earned from RewardedVid,LevelComplete,InAPP OR Spent on Buying store Items.   </param>
	/// <param name="itemType"> for Earning = RewardedVid,LevelComplete,InAPP : For Spending = Store </param>
	/// <param name="itemId"> for earning it can be empty For spending it should be in store = Pack1 , Pack2 , Pack3 , pack4..  </param>
	public static void LogResourceEvent(GAResourceFlowType flowType, string currency, float amount, string itemType, string itemId  )
	{
		var iapParameters = new Dictionary<string, object>();
		iapParameters["PurchasedItem"] = itemId;
#if UNITY_IPHONE
		Debug.Log("Prog: : "+  GAResourceFlowType.Sink.ToString() +":"+ currency+":"+amount.ToString() + ":"+  itemType + ":"+ itemId ) ;   
    		GameAnalytics.NewResourceEvent(flowType  , currency, amount,   itemType , itemId );
		//FB.LogPurchase(amount, "USD", iapParameters);
#endif
#if UNITY_ANDROID
    			Debug.Log("resource:: "+  flowType.ToString() +":"  + ":" + currency + ":"+ amount.ToString() + ":"+  itemType + ":"+ itemId ) ;   

    			GameAnalytics.NewResourceEvent(flowType  , currency, amount,   itemType , itemId );
				//FB.LogPurchase(amount, "USD", iapParameters);

#endif

	}


	public const string World = "FightingGame";
	public const string Point = "Episode";
	public const string Phase = "Mission";

	// For resource event
	public const string Currency = "Coins";
	public const string Currency_Star = "Stars";
	public const string RewardedVid = "RewardedVid";
	public const string StarInGameplay = "StarInGameplay";
	public const string LevelUnlock = "LevelUnlock";
	public const string HintReveal = "HintReveal";



	public const string  LevelComplete = "LevelComplete";
	public const string  LevelComplete_rAd = "LevelComplete_rAd";
	public const string  InAPP = "InAPP";
	public const string  Store = "Store";
	public const string Tutorial = "Tutorial";


    public static void LogResourceEvent(bool isAdd, float Ammount, string ItemType)
    {
        if (isAdd)
        {
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "Coins", Ammount, ItemType, "");
        }
        else
        {
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "Coins", Ammount, ItemType, "");
        }
    }

    public static void LogEvent(string Message)
    {
        Analytics.CustomEvent(Message);
        GameAnalytics.NewDesignEvent(Message);
        Firebase.Analytics.FirebaseAnalytics.LogEvent(Message);
        // FB.LogAppEvent(Message);
    }

    public static void OpenShopEventLog()
    {
        Analytics.CustomEvent("OpenShop");
        GameAnalytics.NewDesignEvent("OpenShop");
        Firebase.Analytics.FirebaseAnalytics.LogEvent("OpenShop");
        //FB.LogAppEvent("OpenShop");
    }

    public static void MissionOrLevelStartedEventLog(string GameMode, int LevelNumber)
    {
        Analytics.CustomEvent("LevelStarted_" + GameMode + "_Level_" + LevelNumber.ToString());
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "_" + GameMode, "Level_" + LevelNumber);
        Firebase.Analytics.FirebaseAnalytics.LogEvent("LevelStarted_" + GameMode + "_Level_" + LevelNumber.ToString());
        // FB.LogAppEvent("LevelStarted_GameMode_" + GameMode + "_Level_Number_" + LevelNumber.ToString());
    }
    public static void MissionOrLevelFailEventLog(string GameMode, int LevelNumber)
    {
        Analytics.CustomEvent("LevelFail_" + GameMode + "_Level_Number_" + LevelNumber.ToString());
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "_" + GameMode, "Level_" + LevelNumber);
        Firebase.Analytics.FirebaseAnalytics.LogEvent("LevelFail_" + GameMode + "_Level_" + LevelNumber.ToString());
        //FB.LogAppEvent("LevelFail_GameMode_" + GameMode + "_Level_Number_" + LevelNumber.ToString());
    }

    public static void MissionOrLevelCompletedEventLog(string GameMode, int LevelNumber)
    {
        Analytics.CustomEvent("LevelComplete_" + GameMode + "_Level_" + LevelNumber);
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "_" + GameMode, "_Level_" + LevelNumber);
        Firebase.Analytics.FirebaseAnalytics.LogEvent("LevelComplete_" + GameMode + "_Level_" + LevelNumber);

        //FB.LogAppEvent("LevelComplete_GameMode" + GameMode + "_LevelNumber_" + LevelNumber);
    }

    public static void ItemPurchaseEventLog(string ItemName, int ItemPrice)
    {
        Analytics.CustomEvent("Item_" + ItemName + "_Price" + ItemPrice.ToString());
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "_Coins_", ItemPrice, ItemName, "");
        Firebase.Analytics.FirebaseAnalytics.LogEvent("Item_" + ItemName + "_Price" + ItemPrice.ToString());

        // FB.LogAppEvent("ItemPurchased_Name" + ItemName + "_ItemPrice" + ItemPrice.ToString());
    }

    public static void DesignEvent(string str)
    {
        Analytics.CustomEvent(str);
        GameAnalytics.NewDesignEvent(str);

        // FB.LogAppEvent(str);
    }
    public static void FirebaseEvent(string str)
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent(str);
    }

}
