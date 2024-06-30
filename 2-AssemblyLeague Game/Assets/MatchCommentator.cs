using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchCommentator : MonoBehaviour
{
    public static MatchCommentator PublicAccess;
    public bool Test = true;
    public RobotMeta TrackedRobot;
    
    public enum EnumAnouncement
    {
        BattleStarted = 0,
        LetTheBattleBegin = 1,
        TenSceondsRemain = 2,
        BattleOver = 3,
        MatchIsOver = 4,
        YouHaveWon = 5,
        YouCame2nd = 6,
        YouCame3rd = 7,
        YouHaveLost = 8,
        YouAreInTheLead = 9,
        YouHaveLostTheLead = 10,
        YouHaveMovedIntoLastPlace = 11,
        YouHaveMovedIntoFirstPlace = 12
    }
    // Use this for initialization
    void Start()
    {
        PublicAccess = this;
    }

    float updateTime = 0.5f;
    float currentTime = 1f;
    RobotMeta lastTracked;
    public void TrackRobot(RobotMeta aRobot)
    {
        TrackedRobot = aRobot;

    }
    public bool wasInLead = false;
    public bool isInLead = false;
    bool hasDisplayedEnded = false;
    // Update is called once per frame

    float speakDelay = 1f;
    bool isSameRobot = false;
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= updateTime)
        {
            currentTime = 0;

            
            if (Match.PublicAccess != null && GameObjectFollower.PublicAccess != null)
            {
                {
                    if (GameObjectFollower.PublicAccess.GameObjectToFollow != null)
                    {
                        RobotMeta aRobot = GameObjectFollower.PublicAccess.GameObjectToFollow.GetComponent<RobotMeta>();
                        TrackRobot(aRobot);
                    }
                }
                
                if (Match.PublicAccess != null && Match.PublicAccess.IsInIntroductionMode)
                {
                    hasDisplayedEnded = false;

                }
               
                if (Match.PublicAccess.HasMatchEnded && TrackedRobot != null)
                {
                    if (hasDisplayedEnded == false)
                    {
                        hasDisplayedEnded = true;

                        if (TrackedRobot.RuntimeRank == 1)
                        {
                            DoAnnouncement(EnumAnouncement.YouHaveWon, speakDelay);

                        }
                        if (TrackedRobot.RuntimeRank == 2)
                        {
                            DoAnnouncement(EnumAnouncement.YouCame2nd, speakDelay);

                        }
                        if (TrackedRobot.RuntimeRank == 3)
                        {
                            DoAnnouncement(EnumAnouncement.YouCame3rd, speakDelay);

                        }
                        if (TrackedRobot.RuntimeRank > 3)
                        {
                            DoAnnouncement(EnumAnouncement.YouHaveLost, speakDelay);

                        }
                    }


                }
            }
            if (TrackedRobot != null)
            {
                if (TrackedRobot.RuntimeRank == 1)
                {
                    isInLead = true;
                }
                else
                {
                    isInLead = false;

                }
                if (wasInLead == true && isInLead == false)
                {
                    DoAnnouncement(EnumAnouncement.YouHaveLostTheLead);
                }
                if (wasInLead == false && isInLead == true)
                {
                    DoAnnouncement(EnumAnouncement.YouAreInTheLead, speakDelay);
                }
            }
            wasInLead = isInLead;
            lastTracked = TrackedRobot;
        }
    }
    public void DoAnnouncement(EnumAnouncement anAnouncement)
    {
        string audioClip = ((int)anAnouncement) + "A";
        if (audioClip.Length < 4)
        {
            audioClip = "0" + audioClip;
        }
        if (audioClip.Length < 4)
        {
            audioClip = "0" + audioClip;
        }
        if (audioClip.Length < 4)
        {
            audioClip = "0" + audioClip;
        }
        if (audioClip.Length < 4)
        {
            audioClip = "0" + audioClip;
        }
        AuxAudio.PublicAccess.PlayInMatchAnnouncement(audioClip);

    }
    public void DoAnnouncement(EnumAnouncement anAnouncement, float delay)
    {
        string audioClip = ((int)anAnouncement) + "A";
        if (audioClip.Length < 4)
        {
            audioClip = "0" + audioClip;
        }
        if (audioClip.Length < 4)
        {
            audioClip = "0" + audioClip;
        }
        if (audioClip.Length < 4)
        {
            audioClip = "0" + audioClip;
        }
        if (audioClip.Length < 4)
        {
            audioClip = "0" + audioClip;
        }
        AuxAudio.PublicAccess.PlayInMatchAnnouncement(audioClip, delay);

    }


}
