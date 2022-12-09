using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisskeyLiveCommentViewer
{
    internal class Moetts
    {
        public static WebSocket4Net.WebSocket WebSocket;
        public string MoettsWSUrl;
        public Queue<VoiceData> strings = new Queue<VoiceData>();
        public void Play(string voiceText)
        {
            WebSocket = new WebSocket4Net.WebSocket(MoettsWSUrl);
            WebSocket.MessageReceived += WebSocket_MessageReceived;
            WebSocket.Opened += WebSocket_Opened;
            WebSocket.Closed += WebSocket_Closed;
            WebSocket.Open();
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
                    WebSocket.Send(JsonClassToObject(new SessionStartClass()));
                    return;
                case "estimation":
                    return;
                case "send_data":

                case "process_starts":
                case "process_completed":

                default:
                    break;
            }
        }
        private string JsonClassToObject(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        private void SoundPlay(string Data)
        {
            System.Media.SoundPlayer soundPlayer = new System.Media.SoundPlayer();
        }
        private SessionStartClass VoiceDataSet(VoiceData VoiceData)
        {
            SessionStartClass sessionStartClass = new SessionStartClass();

            return sessionStartClass;
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
        public string sessionhash = "6lirauqhl1";
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
