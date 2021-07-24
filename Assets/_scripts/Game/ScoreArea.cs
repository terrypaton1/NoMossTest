using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreArea : MonoBehaviour
{
    public class LeaderboardData{
        public class LeaderboardPlayer{
            public int rank;
            public int score;
            public bool isPlayer;

            public LeaderboardPlayer(int r, int s, bool isMe){
                rank = r;
                score = s;
                isPlayer = isMe;
            }
        }

        public LeaderboardData(LeaderboardPlayer[] p){
            players = p;
        }

        public LeaderboardPlayer[] players;
    }

    public Color notUsColour;
    public Color usColour;

    public TMPro.TextMeshProUGUI[] scoreAreas;

    LeaderboardData currentData;

    public Animator anim;

    public void UpdateLeaderboardData(LeaderboardData data){
        currentData = data;

        anim.SetBool("hasLeaderboardData", true);
    }

    public void LeaderboardAboutToDisplay(){
        if(currentData == null){
            return;
        }

        for(int i = 0; i < scoreAreas.Length; i++){
            if(i >= currentData.players.Length ){
                //Not enough data for this one, so keep it empty
                scoreAreas[i].text = "";
            }else{
                LeaderboardData.LeaderboardPlayer player = currentData.players[i];
                if(player.isPlayer){
                    scoreAreas[i].color = usColour;
                    //scoreAreas[i].text = "YOU: " + player.score;
                    scoreAreas[i].text = player.rank + ": " + player.score;
                }else{
                    scoreAreas[i].text = player.rank + ": " + player.score;
                    scoreAreas[i].color = notUsColour;
                }
            }
        }

        currentData = null;
    }

    public void DisableLeaderboard(){
        anim.SetBool("hasLeaderboardData", false);
        anim.Play("HighScoreShown");
    }
}
