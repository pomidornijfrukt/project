using System;
using System.Collections.Generic;

class Reglog{
    protected Dictionary<Tuple<string, string>, string> userlog = new Dictionary<Tuple<string, string>, string>();
    protected bool ReglogStatus = false;

    public Reglog(string username, string email, DateTime date){
        userlog.Add(Tuple.Create(username, email), date.ToString());
    }

    public void ChangeLog(string username, string email, DateTime newdate, string newemail, string newusername){
        userlog.Remove(Tuple.Create(username, email));
        userlog.Add(Tuple.Create(newusername, newemail), newdate.ToString());
    }

    public void ShowLog(){
        foreach (var item in userlog){
            Console.WriteLine($"Username: {item.Key.Item1}, Email: {item.Key.Item2}, Date: {item.Value}");
        }
    }

    public void DeleteLog(string username, string email){
        userlog.Remove(Tuple.Create(username, email));
    }

    // public void RegLogStatus() {
    //     ReglogStatus = userlog./*isnull idk */ > 0 ? true : false;
    // }

}