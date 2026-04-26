using GameAnalyticsSDK;
using Firebase.Analytics;

public static class FireBase_GA_EventCalls
{
    public static void LogEvents(string eventStr, string modeNo, string envNo, string levelNo)
    {
        string newStr = eventStr + "_MODE" + modeNo + "_ENV" + envNo + "_Level" + levelNo;
        string finalStr = newStr.Replace(" ", "_");
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Undefined, finalStr);
        FirebaseAnalytics.LogEvent(finalStr);
#if UNITY_EDITOR
        UnityEngine.MonoBehaviour.print(finalStr);
#endif
    }

    public static void LogEvents(string eventStr, string itemNo)
    {
        string newStr = eventStr + "_ITEM" + itemNo;
        string finalStr = newStr.Replace(" ", "_");
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Undefined, finalStr);
        FirebaseAnalytics.LogEvent(finalStr);
#if UNITY_EDITOR
        UnityEngine.MonoBehaviour.print(finalStr);
#endif
    }
}
