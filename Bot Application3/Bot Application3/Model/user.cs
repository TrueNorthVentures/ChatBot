﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application3.Model
{
    public class user
    {

        private static BotContext ec = new BotContext();
        private static BotContext ac = null;
        public static bool Add_UserInfo(UsersInfo ui)
        {
            ac = new BotContext();
            ac.UsersInfoes.Add(ui);
            try
            {
                ac.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static UsersInfo Exist_User(string id)
        {
            ac = new BotContext();
            // var res = ac.UsersInfoes.Any(x => x.User_ID == id);
            var query = (from exs in ac.UsersInfoes
                         where exs.User_ID == id
                         select exs).SingleOrDefault();
            return query;
        }
    }

}