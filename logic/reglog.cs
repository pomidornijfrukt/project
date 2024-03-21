using System;
using System.Collections.Generic;

class reglog{
    private Dictionary<Tuple<string, string>, string> userlog;


    public static void register(string username, string password, string email) {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email)) {
            throw new ArgumentException("Username or password or email cannot be empty");
        }
        registration(username, password, email);
    }

}