using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Leaderboard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    public void RetrieveLeaderboard()
    {
        if (PlayerPrefs.HasKey("leaderboard"))
        {
            var leaderboardPairs = new List<string>(PlayerPrefs.GetString("leaderboard").Split(','));
            var names = new List<string>();
            var scores = new List<int>();
            var leaderboardText = "Leaderboard\n\n";
            var i = 1;
            foreach (var leaderboardPair in leaderboardPairs)
            {
                string[] split = leaderboardPair.Split(':');
                leaderboardText += string.Format("{0}. {1} - {2} pts\n", i, split[0], split[1]);
                i++;
            }
            text.text = leaderboardText;
        }
    }

    public void ClearLeaderboard()
    {
        if (PlayerPrefs.HasKey("leaderboard"))
        {
            PlayerPrefs.DeleteKey("leaderboard");
            text.text = "";
        }
    }
}
