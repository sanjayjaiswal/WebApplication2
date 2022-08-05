using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                //Menu menu = (Menu)Page.Master.FindControl("NavigationMenuAdmin");
                //menu.MenuItemClick += MenuEventHandler;
           
        }
        //protected void MenuEventHandler(object sender, MenuEventArgs Events)
        //{
        //    Menu Menu = (Menu)sender;
        //    MenuItem selectedItem = Menu.SelectedItem;
        //    Label1.Text = Menu.SelectedItem.Text;
        //    HiddenField1.Value = Menu.SelectedItem.Text;
        //    Response.Write("Selected Item is: " + Menu.SelectedItem.Text + ".");
        //}

        //protected void NavigationMenuAdmin_MenuItemClick(object sender, MenuEventArgs e)
        //{
        //    Menu Menu = (Menu)sender;
        //    MenuItem selectedItem = Menu.SelectedItem;
        //    Label1.Text = Menu.SelectedItem.Text;
        //    HiddenField1.Value = Menu.SelectedItem.Text;
        //    Response.Write("Selected Item is: " + Menu.SelectedItem.Text + ".");
        //}
    }
}