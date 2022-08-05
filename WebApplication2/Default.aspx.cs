using System;
using System.Diagnostics;
using System.Threading;
using System.Web.UI;

namespace WebApplication2
{
    public partial class _Default : Page
    {
        //System.Threading.CancellationTokenSource cTokenSource = new System.Threading.CancellationTokenSource();
        //private CancellationTokenSource _canceller;
        //private BackgroundWorker _worker = null;
        //Random rnd_out_loop = new Random();

        string[] message = { "how", "are", "you" };
        int counterTimerValue = 0;
        //private static System.Timers.Timer aTimer;
        //private static System.Timers.Timer displayTimer;
        //static System.Windows.Forms.Timer displayTimer = new System.Windows.Forms.Timer();
        protected void Page_Load(object sender, EventArgs e)
        {
            InitTimer();
        }
        private System.Windows.Forms.Timer timer1;
        public void InitTimer()
        {
            //for (int i = 0; i < message.Length; i++)
            //{
                //counterTimerValue = message.Length;
                //timer1 = new System.Windows.Forms.Timer();
                //timer1.Tick += new EventHandler(timer1_Tick);
                //timer1.Interval = 2000; // in miliseconds
                //timer1.Start();
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            counterTimerValue--;
            TextBox2.Text = message[counterTimerValue].ToString();
            if (counterTimerValue == 0)
            {
                timer1.Stop();
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < message.Length; i++)
            {
                Stopwatch stopWatch = new Stopwatch();
                //stopWatch.Start();
                
                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = stopWatch.Elapsed;

                // Format and display the TimeSpan value. 
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                Label1.Text = "RunTime " + i.ToString();
                Thread.Sleep(1000);
                //stopWatch.Stop();
            }
        }
        

        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine(string.Format("The Elapsed event was raised at {0}", e.SignalTime));
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            //cTokenSource.Cancel();
            //_worker.CancelAsync();
            //_canceller.Cancel();
        }

        //protected void displayTimer_Tick(object sender, EventArgs e)
        //{

        //}

        //public static async Task Loop()
        //{
        //    while (true)
        //    {
        //        // Commenting this out makes the code loop indefinitely!
        //        await Task.Delay(TimeSpan.FromMilliseconds(1));

        //        // This doesn't matter.
        //        await DoWork();
        //    }
        //}

        //public static async Task DoWork()
        //{
        //    await Task.CompletedTask;
        //}

        [System.Web.Services.WebMethod]
        public static string GetCurrentTime(string name)
        {
            return "Hello " + name + Environment.NewLine + "The Current Time is: "
                + DateTime.Now.ToString();
        }
    }
}