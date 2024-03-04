using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing.Imaging;
public partial class Turing : System.Web.UI.Page
{
    private void Page_Load(object sender, System.EventArgs e)
    {
        Random rnd = new Random();
        int strRandomValue = rnd.Next(5, 99999);
        this.Session["CaptchaImageText"] = strRandomValue.ToString();
        RandomImage ci = new RandomImage(this.Session["CaptchaImageText"].ToString(), 220, 40);
        this.Response.Clear();
        this.Response.ContentType = "image/jpeg";
        ci.Image.Save(this.Response.OutputStream, ImageFormat.Jpeg);
        ci.Dispose();   
    }
}
