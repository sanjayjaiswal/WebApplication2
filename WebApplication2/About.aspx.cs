using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Threading;
using CoreScanner;

namespace WebApplication2
{
    public partial class About : Page
    {
        // Declare CoreScannerClass
        //static CCoreScannerClass cCoreScannerClass;
        protected void Page_Load(object sender, EventArgs e)
        {
            string stringInput = "one, Two, Three, Four";

            string[] sen = stringInput.Split(',') ;


            string mreverse = String.Empty;

            for (int j = 0; j < sen.Length; j++)
            {
                string reverse = String.Empty;
                char[] stringArr = sen[j].ToCharArray();
                for(int i = stringArr.Length-1; i > -1; i--)
                {
                    reverse += stringArr[i];
                }
                mreverse += " " + reverse;
            }

            Response.Write(mreverse);

            // logcal check valid and invalid string

            var sString = "ppabbba";

            var reversedValue = sString.ToCharArray()
                         .Select(ch => ch.ToString())
                         .Aggregate<string>((xs, x) => x + xs);

            char[] myChar = sString.ToCharArray();


            int rCount = 0;
            string singleChar = string.Empty;
            string rValue = string.Empty;
            string invalidChar = string.Empty;

            for (int i = 0; i < myChar.Length; i++)
            {
                int counter = 0;

                singleChar = myChar[i].ToString();
                for (int j = 0; j < myChar.Length; j++)
                {
                    if (singleChar == myChar[j].ToString())
                    {
                        counter++;
                    }
                }
                if (rCount != counter && rCount > 0)
                {
                    rValue = "Invalid";
                    invalidChar = singleChar;
                    break;
                }
                else
                    rValue = "Valid";
                rCount = counter;

            }

            Console.WriteLine(rValue + " - " + invalidChar);

        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            var cCoreScannerClass = new CCoreScanner();
            //Call Open API
            short[] scannerTypes = new short[1]; // Scanner Types you are interested in
            scannerTypes[0] = 1; // 1 for all scanner types
            short numberOfScannerTypes = 1; // Size of the scannerTypes array
            int status; // Extended API return code
            cCoreScannerClass.Open(0, scannerTypes, numberOfScannerTypes, out status);
            // Lets list down all the scanners connected to the host
            short numberOfScanners; // Number of scanners expect to be used
            int[] connectedScannerIDList = new int[255];
            // List of scanner IDs to be returned
            string outXML; //Scanner details output
            cCoreScannerClass.GetScanners(out numberOfScanners, connectedScannerIDList, out outXML, out status);
            //try
            //{
            //    //Instantiate CoreScanner Class
            //    CCoreScanner cCoreScannerClass = new CCoreScanner();
            //    //Call Open API
            //    short[] scannerTypes = new short[1];//Scanner Types you are interested in
            //    scannerTypes[0] = 1; // 1 for all scanner types
            //    short numberOfScannerTypes = 1; // Size of the scannerTypes array
            //    int status; // Extended API return code
            //    cCoreScannerClass.Open(0, scannerTypes, numberOfScannerTypes, out status);
            //    // Subscribe for barcode events in cCoreScannerClass
            //    //cCoreScannerClass.BarcodeEvent += new _ICoreScannerEvents_BarcodeEventEventHandler(OnBarcodeEvent);
            //    // Let's subscribe for events
            //    int opcode = 1001; // Method for Subscribe events
            //    string outXML; // XML Output
            //    string inXML = "<inArgs>" +
            //    "<cmdArgs>" +
            //    "<arg-int>1</arg-int>" + // Number of events you want to subscribe
            //    "<arg-int>1</arg-int>" + // Comma separated event IDs
            //    "</cmdArgs>" +
            //    "</inArgs>";
            //    cCoreScannerClass.ExecCommand(opcode, ref inXML, out outXML, out status);
            //    //Console.WriteLine(outXML);
            //    Response.Write(outXML);
            //}
            //catch (Exception exp)
            //{
            //    Console.WriteLine("Something wrong please check... " + exp.Message);
            //}
        }

        //public void OnBarcodeEvent(Object eventType, ref string pscanData)
        //{
        //    string barcode = pscanData;
        //    eventType.Invoke((MethodInvoker)delegate { TextBox1.Text = barcode; });
        //}


    }
}