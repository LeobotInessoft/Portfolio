using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RobotServ
{
    public class ClassThreadDB
    {
        public Tutorial CreateTutorial(DataClassesInterfaceDataContext cc, string email, string password, int currentID, string heading, string text)
        {
            Tutorial ret = null;
            // bool success = false;
            Player pl = GetPlayerFromDB(cc, email, password);
            if (pl != null)
            {
                if (pl.ID == 2)
                {
                    ret = cc.Tutorials.FirstOrDefault(x => x.ID == currentID);
                    if (ret == null)
                    {
                        ret = new RobotServ.Tutorial();
                        ret.ID_Player = pl.ID;
                        ret.CreateDate = DateTime.UtcNow;
                        ret.TotalViews = 0;
                        cc.Tutorials.InsertOnSubmit(ret);
                        //  ret.Detail
                    }
                    ret.Detail = text;
                    ret.Heading = heading;
                    cc.SubmitChanges();
                }

            }
            return ret;
        }
        public TutorialReply CreateTutorialReply(DataClassesInterfaceDataContext cc, string email, string password, int threadID, string text)
        {
            TutorialReply ret = null;
            // bool success = false;
            Player pl = GetPlayerFromDB(cc, email, password);
            if (pl != null)
            {
                Tutorial thread = cc.Tutorials.FirstOrDefault(x => x.ID == threadID); ;

                if (ret == null)
                {
                    ret = new TutorialReply();
                    ret.ID_Thread = thread.ID;
                    ret.ID_Player = pl.ID;
                    ret.CreateDate = DateTime.UtcNow;
                    cc.TutorialReplies.InsertOnSubmit(ret);
                    //  ret.Detail
                }
                thread.CreateDate = DateTime.UtcNow;
                ret.ReplyText = text;
                cc.SubmitChanges();

            }
            return ret;
        }

        public ThreadForum CreateThread(DataClassesInterfaceDataContext cc, string email, string password, int currentID, string heading, string text)
        {
            ThreadForum ret = null;
            // bool success = false;
            Player pl = GetPlayerFromDB(cc, email, password);
            if (pl != null)
            {
                ret = cc.ThreadForums.FirstOrDefault(x => x.ID == currentID);
                if (ret == null)
                {
                    ret = new RobotServ.ThreadForum();
                    ret.ID_Player = pl.ID;
                    ret.CreateDate = DateTime.UtcNow;
                    ret.TotalViews = 0;
                    cc.ThreadForums.InsertOnSubmit(ret);
                    //  ret.Detail
                }
                ret.Detail = text;
                ret.Heading = heading;
                cc.SubmitChanges();

            }
            return ret;
        }
        public ThreadReply CreateThreadReply(DataClassesInterfaceDataContext cc, string email, string password, int threadID, string text)
        {
            ThreadReply ret = null;
            // bool success = false;
            Player pl = GetPlayerFromDB(cc, email, password);
            if (pl != null)
            {
                ThreadForum thread = cc.ThreadForums.FirstOrDefault(x => x.ID == threadID); ;

                if (ret == null)
                {
                    ret = new ThreadReply();
                    ret.ID_Thread = thread.ID;
                    ret.ID_Player = pl.ID;
                    ret.CreateDate = DateTime.UtcNow;
                    cc.ThreadReplies.InsertOnSubmit(ret);
                    //  ret.Detail
                }
                thread.CreateDate = DateTime.UtcNow;
                ret.ReplyText = text;
                cc.SubmitChanges();

            }
            return ret;
        }
        private Player GetPlayerFromDB(DataClassesInterfaceDataContext cc, string email, string password)
        {
            Player aPlayer = null;
            aPlayer = cc.Players.FirstOrDefault(x => x.Email.Trim().ToLower() == email.Trim().ToLower() && x.Password == password);
            return aPlayer;
        }
        public Player Login(DataClassesInterfaceDataContext cc, string email, string password)
        {
            Player aPlayer = null;
            aPlayer = cc.Players.FirstOrDefault(x => x.Email.Trim().ToLower() == email.Trim().ToLower() && x.Password == password);
            return aPlayer;
        }
        public Player Register(DataClassesInterfaceDataContext cc,string displayName, string email, string password)
        {
            Player aPlayer = null;


            aPlayer = cc.Players.FirstOrDefault(x => x.Email.Trim().ToLower() == email.Trim().ToLower());
            if (aPlayer == null)
            {
                aPlayer = new Player();
                aPlayer.DisplayName = displayName;
                aPlayer.Email = email.Trim();
                aPlayer.Password = password;
                aPlayer.ImageExt = "";
                aPlayer.Money = 0;
                aPlayer.MoneyWon = 0;
                aPlayer.ResetCode = "";
                aPlayer.IsPlayer = false;
                aPlayer.SteamID = "";
                
                cc.Players.InsertOnSubmit(aPlayer);
                cc.SubmitChanges();

            }
            else
            {
                aPlayer = null;
            }
            return aPlayer;
        }


    }
}