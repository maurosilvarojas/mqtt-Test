using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
//using uPLibrary.Networking.M2Mqtt;
//using uPLibrary.Networking.M2Mqtt.Messages;

using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Text;
using System.Threading.Tasks;
using static Android.Widget.SeekBar;
using Java.Lang;
using System.Threading;

namespace mqtt_Test
{
    [Activity(Label = "mqtt_Test", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private MqttClient client;
        private int count;
        private byte[] message;
        
        



        protected override void OnCreate(Bundle bundle)
        //public void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            ActionBar.Hide();


            //mqtt conf
            this.client = new MqttClient("broker.mqttdashboard.com");
            this.client.Connect(Guid.NewGuid().ToString());

            this.client.Subscribe(new string[] { "/MQTTTester" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            //this.client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            
            //ui instances

            Button button = FindViewById<Button>(Resource.Id.MyButton);
            SeekBar bar = FindViewById<SeekBar>(Resource.Id.seekBar1);
            TextView textMQTT = FindViewById<TextView>(Resource.Id.textMQTT);
            TextView sliderText = FindViewById<TextView>(Resource.Id.textSlider);

            
            //bar event
            bar.ProgressChanged += delegate
            {
                string newValue = bar.Progress.ToString();
                sliderText.Text = newValue;
                this.client.Publish("/MQTTTester", Encoding.UTF8.GetBytes(newValue));
                //textMQTT.Text = receiverData;
            };

            //button Event
            button.Click += delegate
            {
                button.Text = string.Format("{0} clicks!", count++);
                this.client.Publish("/MQTTTester", Encoding.UTF8.GetBytes(button.Text));
            };

            //Mqtt Publish Event 
            client.MqttMsgPublishReceived += (object sender, MqttMsgPublishEventArgs e) =>
            {
                message = e.Message;
                System.Text.StringBuilder parsingMsg = new System.Text.StringBuilder();
                foreach (var value in message)
                {
                    parsingMsg.Append(Char.ConvertFromUtf32(value));
                }
                this.RunOnUiThread(() => {
                   textMQTT.Text=parsingMsg.ToString();
                });
                
            };
            
        }//finish OnCreate

        

       



    }

         
        
    

} 
    


