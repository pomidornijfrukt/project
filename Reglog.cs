using System;
using System.Collections.Generic;

class Reglog{
    protected Dictionary<Tuple<string, string>, string> userlog = new Dictionary<Tuple<string, string>, string>();

    public Reglog(string username, string email, DateTime date){
        userlog.Add(Tuple.Create(username, email), date.ToString());
    }

}