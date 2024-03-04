using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
//using Coolite.Ext.Web;
using Ext.Net.Examples;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Ext.Net;

/// <summary>
/// Summary description for clsTreeView
/// </summary>
public class clsTreeView
{
	public clsTreeView()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public TreeNodeCollection BuildTreeNodes(bool refreshSiteMap,string strGroupID)
    {
        //XmlDocument map = null;
        //XmlElement root = null;
        XmlElement examplesNode = null;
        //if (refreshSiteMap)
        //{
        //    map = new XmlDocument();
        //    XmlDeclaration dec = map.CreateXmlDeclaration("1.0", "utf-8", null);
        //    map.AppendChild(dec);

        //    root = map.CreateElement("siteMap");
        //    root.SetAttribute("xmlns", "http://schemas.microsoft.com/AspNet/SiteMap-File-1.0");
        //    map.AppendChild(root);

        //    examplesNode = map.CreateElement("siteMapNode");
        //    examplesNode.SetAttribute("title", "Examples");
        //    root.AppendChild(examplesNode);
        //}

        TreeNodeCollection result = BuildTreeLevel(examplesNode,strGroupID);
        
        return result;
    }

    private TreeNodeCollection BuildTreeLevel(XmlElement siteMap,string strGroupID)
    {

        clsExtractSysMenu objSysAdmin = new clsExtractSysMenu();
        TreeNodeCollection nodes = new TreeNodeCollection(false);
        TreeNode node = new TreeNode();
        XmlElement siteNode = null;
        DataSet dtsSysTreeMenu = new DataSet();
        ///##################
        dtsSysTreeMenu = objSysAdmin.GetRootMenu();
        ///##################
        try
        {
            foreach (DataRow pRow in dtsSysTreeMenu.Tables["CM_SYSTEM_MENU"].Rows)
            {
                string img = ApplicationRoot + "/resources/images/noimage.gif";
                string title = pRow["SYS_MENU_TITLE"].ToString();
                string desc = "No description";
                node.Text = pRow["SYS_MENU_TITLE"].ToString();
                //node.IconCls = iconCls;
                node.Expanded = true;
                node.SingleClickExpand = true;
                string qtip = string.Format("<div class='thumb-wrap' style='margin:0px;float:none;'><img src='{0}' title='{1}'/><div><h4>{1}</h4><p>{2}</p></div></div>",
                                            img, title, desc);
                node.Qtip = qtip;
                //string url = "frmTree.aspx";
                node.NodeID = pRow["SYS_MENU_ID"].ToString(); 
                //node.Href = url;
                
                node.Leaf = false;
                nodes.Add(node);

                LoadChildMenu(node, pRow["SYS_MENU_ID"].ToString(), strGroupID);
            }
        }
        catch (NullReferenceException ex)
        {
            return null;
        }
               
        return nodes;
    }
   
    private void LoadChildMenu(TreeNode root, string strMenuID,string strGroupID)
    {
        clsExtractSysMenu objSysAdmin = new clsExtractSysMenu();
        TreeNode child = new TreeNode();
        DataSet dtsSysTreeMenu = new DataSet();
        
        //strGroupId="10042601002001";
        /////##################
        dtsSysTreeMenu = objSysAdmin.GetChildMenu(strMenuID, strGroupID);
        /////##################
        try
        {
            foreach (DataRow pRow in dtsSysTreeMenu.Tables["CM_SYSTEM_MENU"].Rows)
            {
                child = new TreeNode();
                string img = ApplicationRoot + "/resources/images/noimage.gif";
                string title = pRow["SYS_MENU_TITLE"].ToString();
                string desc = "No description";
                child.Text = pRow["SYS_MENU_TITLE"].ToString();
                //node.IconCls = iconCls;
                child.SingleClickExpand = true;
                string qtip = string.Format("<div class='thumb-wrap' style='margin:0px;float:none;'><img src='{0}' title='{1}'/><div><h4>{1}</h4><p>{2}</p></div></div>",
                                            img, title, desc);
                child.Qtip = qtip;

                child.NodeID = pRow["SYS_MENU_ID"].ToString();
                
                if (pRow["SYS_MENU_TYPE"].ToString().Equals("MN"))
                {
                    child.Leaf = true;
                    string url = ApplicationRoot + "/" + pRow["SYS_MENU_FILE"].ToString(); ;
                    child.Href = url;
                }
                else
                {
                    child.Leaf = false;
                    
                }
                root.Nodes.Add(child);
                LoadChildMenu(child, pRow["SYS_MENU_ID"].ToString(), strGroupID);
            }
        }
        catch (NullReferenceException ex)
        {
            return;
        }
    }
    public static string PhysicalToVirtual(string physicalPath)
    {
        string pathOfWebRoot = HttpContext.Current.Server.MapPath("~/").ToLower();

        int index = physicalPath.IndexOf(pathOfWebRoot, StringComparison.InvariantCultureIgnoreCase);
        if (index == -1)
            throw new Exception("Physical path can't be mapped to the current application.");

        string relUrl = Path.DirectorySeparatorChar.ToString();

        index += pathOfWebRoot.Length;
        relUrl += physicalPath.Substring(index);

        relUrl = relUrl.Replace("\\", "/");

        return ApplicationRoot + relUrl;
    }

    public static string ApplicationRoot
    {
        get
        {
            string root = HttpContext.Current.Request.ApplicationPath;
            return root == "/" ? "" : root;
        }
    }
}
