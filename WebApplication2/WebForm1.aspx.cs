using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
                Menu Menu = (Menu)Page.Master.FindControl("NavigationMenuAdmin");
                Menu.MenuItemClick += MenuEventHandler;
           
        }
        private void MenuEventHandler(object sender, MenuEventArgs Events)
        {
            Menu Menu = (Menu)sender;
            MenuItem selectedItem = Menu.SelectedItem;
            Response.Write("Selected Item is: " + Menu.SelectedItem.Text + ".");
        }
        [System.Web.Services.WebMethod]
        public static int GetCurrentTime(int name)
        {
            return name;
            //"Hello " + name + Environment.NewLine + "The Current Time is: "
            //+ DateTime.Now.ToString();
        }
        public class names
        {
            public string name;
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            //List<names> name = new List<names>();
            //name.Add(new names()
            //{
            //    name = "Sanjay"
            //});
            //name.Add(new names()
            //{
            //    name = "AA"
            //});
            //name.Add(new names()
            //{
            //    name = "BB"
            //});

            //name.ForEach((name) => {
            //    setTimeout(() => {
            //        display(name);
            //    }, 1000);
            //});
            //for (int i = 0; i < 5; i++)
            //{

            //    TextBox1.Text = i.ToString();
            //    System.Threading.Thread.Sleep(2000);

            //}
            Session["Name"] = "Sanjay";
            //for (int i = 0; i < 6; i++)
            //{
            //    var a = i;
            //    var b = i + 1;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "jobs", "ShowCurrentTime();", true);
            //}
        }
        //protected void UpdateTimer_Tick(object sender, EventArgs e)
        //{
        //    //Any Action 
        //    //Business Logic
        //    lbl1.Text = DateTime.Now.ToString();
        //}
        //protected void btnGetTime_Click(object sender, EventArgs e)
        //{
        //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "jobs", "ShowCurrentTime()", true);
        //}

        //protected void Button2_Click(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "jobstop", "StopCurrentTime();", true);
        //}
    }
}