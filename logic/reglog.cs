using System;
using System.Collections.Generic;

class reglog{
    private Dictionary<Tuple<string, string>, string> userlog;


    public static void register(string username, string password, string email) {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) {
            throw new ArgumentException("Username and password cannot be empty");
        }
        registration(username, password, email);
    }

}