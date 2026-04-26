using UnityEngine;

public class ShowIntersetial : MonoBehaviour
{
    private void OnEnable()
    {
        AdmobAdsManager.Instance.ShowInterstitial();
    }
}
