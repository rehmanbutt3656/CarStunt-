/***************************************************************************\
Project:      Daily Rewards
Copyright (c) Niobium Studios.
Author:       Guilherme Nunes Barbosa (gnunesb@gmail.com)
\***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
/* 
 * Daily Reward Object UI representation
 */
namespace NiobiumStudios
{
    /** 
     * The UI Representation of a Daily Reward.
     * 
     *  There are 3 states:
     *  
     *  1. Unclaimed and available:
     *  - Shows the Color Claimed
     *  
     *  2. Unclaimed and Unavailable
     *  - Shows the Color Default
     *  
     *  3. Claimed
     *  - Shows the Color Claimed
     *  
     **/
    public class DailyRewardUI : MonoBehaviour
    {
        public bool showRewardName;

        [Header("UI Elements")]
        public Text textDay;                // Text containing the Day text eg. Day 12
        public Text textReward;             // The Text containing the Reward amount
        public Image imageRewardBackground; // The Reward Image Background
        public Image unAvailableReward; // The Reward Image Background
        public Image imageReward;           // The Reward Image
        public Image OnlyShowReward;           // The Reward Image
        public Image ClaimRewardImage;           // The Reward Image
        public Color colorClaim;            // The Color of the background when claimed
        private Color colorUnclaimed;       // The Color of the background when not claimed

        [Header("Internal")]
        public int day;

        [HideInInspector]
        public Reward reward;

        public DailyRewardState state;

        // The States a reward can have
        public enum DailyRewardState
        {
            UNCLAIMED_AVAILABLE,
            UNCLAIMED_UNAVAILABLE,
            CLAIMED
        }

        void Awake()
        {
            colorUnclaimed = imageReward.color;
            if (!PlayerPrefs.HasKey("RewardWeek"))
            {
                PlayerPrefs.SetInt("RewardWeek", 1);
            }
        }

        public void Initialize()
        {
            if (MainMenuController.Instance.awakeCallforDailyRewards) { textDay.text = "DAY " + string.Format("{0}", day.ToString()); }
            else { textDay.text = "DAY " + string.Format("{0}", day.ToString()); }
            if (reward.reward > 0)
            {
                if (showRewardName)
                {
                    textReward.text = reward.reward + " " + reward.unit;
                }
                else
                {
                    textReward.text = reward.reward.ToString();
                }
            }
            else
            {
                textReward.text = "";/*reward.unit.ToString()*/
            }
            imageReward.sprite = reward.sprite;
        }

        // Refreshes the UI
        public void Refresh()
        {
            if (DailyRewards.dr_Instance.lastReward < 7) { PlayerPrefs.SetInt("RewardWeek", 1); }
            else if (DailyRewards.dr_Instance.lastReward >= 7 && DailyRewards.dr_Instance.lastReward < 14) { PlayerPrefs.SetInt("RewardWeek", 2); }
            else if (DailyRewards.dr_Instance.lastReward >= 14 && DailyRewards.dr_Instance.lastReward < 21) { PlayerPrefs.SetInt("RewardWeek", 3); }
            else if (DailyRewards.dr_Instance.lastReward >= 21) { PlayerPrefs.SetInt("RewardWeek", 4); }

            int week = PlayerPrefs.GetInt("RewardWeek", 1);

            switch (state)
            {
                case DailyRewardState.UNCLAIMED_AVAILABLE:
                    unAvailableReward.gameObject.SetActive(false);
                    imageReward.gameObject.SetActive(true);
                    ClaimRewardImage.gameObject.SetActive(false);
                    transform.GetChild(0).GetComponent<bubbleEffect>().enabled = true;
                    transform.GetChild(1).gameObject.SetActive(true);
                    break;
                case DailyRewardState.UNCLAIMED_UNAVAILABLE:
                    if (day <= (7 * week))
                    {
                        unAvailableReward.gameObject.SetActive(false);
                        imageReward.gameObject.SetActive(true);
                        ClaimRewardImage.gameObject.SetActive(false);
                    }
                    else
                    {
                        unAvailableReward.gameObject.SetActive(true);
                        imageReward.gameObject.SetActive(false);
                        ClaimRewardImage.gameObject.SetActive(false);
                    }
                    transform.GetChild(1).gameObject.SetActive(false);
                    transform.GetChild(0).GetComponent<bubbleEffect>().enabled = false;
                    break;
                case DailyRewardState.CLAIMED:
                    unAvailableReward.gameObject.SetActive(false);
                    imageReward.gameObject.SetActive(false);
                    transform.GetChild(1).gameObject.SetActive(false);
                    ClaimRewardImage.gameObject.SetActive(true);
                    break;
            }
        }
    }
}