using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI;

/// <summary>
/// Summary description for clsSecurity
/// </summary>
public class clsSecurity
{
	public clsSecurity()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public bool validatePhone(string phoneNo)
    {
        Regex elevenDigit=new Regex(@"^\d{11}$");
        if (elevenDigit.IsMatch(phoneNo))
        {
            Regex objPhonePattern = new Regex("^[0-9]{10,12}$");
            return objPhonePattern.IsMatch(phoneNo);
        }
        else
            return false;
    }
    public bool validateUserNAme(string id)
    {
         //lettersOnly = new Regex("^[a-zA-Z]{1,25}$");
       // Regex.IsMatch(id, @"^[a-zA-Z0-9]+$")
        if (Regex.IsMatch(id, @"^[a-zA-Z0-9]+$"))
        {
            return true;
        }
        else
            return false;
    }
    public bool validatePin(string pin)
    {
        int n;
        bool isnumeric=int.TryParse(pin,out n);
        return isnumeric;
    }
    public bool validateAmount(string amount)
    {
        int n;
        bool isnumeric = int.TryParse(amount, out n);
        return isnumeric;
    }
    public void clearSessionCach(Page page)
    {
        page.Session.Abandon();
        page.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
        page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        page.Response.Cache.SetNoStore();
    }
}