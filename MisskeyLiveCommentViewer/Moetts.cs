using Newtonsoft.Json;
using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MisskeyLiveCommentViewer
{
    internal class Moetts
    {
        public static WebSocket4Net.WebSocket WebSocket;
        public string MoettsWSUrl;
        public SessionStartClass SessionStartClass = new SessionStartClass();
        private SendDataMessage SendDataMessage = new SendDataMessage();
        public Queue<VoiceData> strings = new Queue<VoiceData>();
        private ReceiveDataMessage ReceiveDataMessage = new ReceiveDataMessage();
        public void Open()
        {
            WebSocket = new WebSocket4Net.WebSocket(MoettsWSUrl);
            WebSocket.MessageReceived += WebSocket_MessageReceived;
            WebSocket.Opened += WebSocket_Opened;
            WebSocket.Closed += WebSocket_Closed;
            WebSocket.Open();
        }
        public void Start(string Text,string Name,int fnindex,string LanguageText)
        {
            SessionStartClass.fn_index = fnindex;
            SessionStartClass.session_hash = sessionhashcreate();
            SendDataMessage.fn_index = fnindex;
            if (!string.IsNullOrEmpty(LanguageText)) Text = $"{LanguageText}{Text}{LanguageText}";
            SendDataMessage.data.Add(Text);
            SendDataMessage.data.Add(Name);
            SendDataMessage.data.Add(1);
            SendDataMessage.data.Add(false);
            SendDataMessage.session_hash = SessionStartClass.session_hash;
            Open();
        }

        private void WebSocket_Closed(object sender, EventArgs e)
        {

        }

        private void WebSocket_Opened(object sender, EventArgs e)
        {

        }

        private void WebSocket_MessageReceived(object sender, WebSocket4Net.MessageReceivedEventArgs e)
        {
            string txt = e.Message.ToString();
            var jsondata = JsonConvert.DeserializeObject<NormalMessage>(txt);
            switch (jsondata.msg)
            {
                case "send_hash":
                    WebSocket.Send(JsonClassToObject(SessionStartClass));
                    return;
                case "estimation":
                    return;
                case "send_data":
                    WebSocket.Send(JsonClassToObject(SendDataMessage));
                    //WebSocket.Send();
                    break;
                case "process_starts":
                    break;
                case "process_completed":
                    ReceiveDataMessage = JsonConvert.DeserializeObject<ReceiveDataMessage>(txt);
                    if (ReceiveDataMessage.success)
                    {
                        SoundPlay(ReceiveDataMessage.output.data[1]);
                    }
                    WebSocket.Close();
                    break;
                default:
                    break;
            }
        }
        private string sessionhashcreate()
        {
            var txt = "abcdefghijklmnopqrstuvwxyz0123456789";
            var n = 10;
            var random = new Random();
            string result = "";
            for (int i = 0; i < n; i++)
            {
                result += txt[random.Next(txt.Length)];
            }
            return result;
        }
        private string JsonClassToObject(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        private void SoundPlay(string Data)
        {
            System.Media.SoundPlayer soundPlayer = new System.Media.SoundPlayer();
            string sounddatabase64 = System.Text.RegularExpressions.Regex.Match(Data,"[^data:audio\\/wav;base64,](.*)").Value;
            Encoding enc;
            var sounddata = Convert.FromBase64String(sounddatabase64);
            soundPlayer.Stream = new MemoryStream(sounddata); ;
            soundPlayer.Load();
            soundPlayer.Play();
        }
        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
    public class VoiceData
    {
        public int voicenum;
        public string Name;
        public string Text;
    }
    public class SessionStartClass
    {
        [JsonProperty("session_hash")]
        public string session_hash = "6lirauqhl1";
        [JsonProperty("fn_index")]
        public int fn_index = 1;
    }
    public class NormalMessage
    {
        [JsonProperty("msg")]
        public string msg = string.Empty;
    }
    public class StartMessage
    {
        [JsonProperty("msg")]
        public string msg;
        [JsonProperty("rank")]
        public int rank;
        [JsonProperty("queue_size")]
        public int queue_size;
        [JsonProperty("avg_event_process_time")]
        public float avg_event_process_time;
        [JsonProperty("avg_event_concurrent_process_time")]
        public float avg_event_concurrent_process_time;
        [JsonProperty("rank_eta")]
        public float rank_eta;
        [JsonProperty("queue_eta")]
        public int queue_eta;
    }
    public class SendDataMessage
    {
        [JsonProperty("fn_index")]
        public int fn_index;
        [JsonProperty("data")]
        public List<object> data = new List<object>();
        [JsonProperty("session_hash")]
        public string session_hash;
    }
    public class ReceiveDataMessage
    {
        [JsonProperty("msg")]
        public string msg;  //"process_completed"
        [JsonProperty("output")]
        public ReceiveDataOutPut output;
        [JsonProperty("success")]
        public bool success;
    }
    public class ReceiveDataOutPut
    {
        [JsonProperty("average_duration")]
        public float average_duration;
        [JsonProperty("data")]
        public List<string> data;
        [JsonProperty("duration")]
        public float duration;
        [JsonProperty("is_generating")]
        public bool is_generating;
    }
}
