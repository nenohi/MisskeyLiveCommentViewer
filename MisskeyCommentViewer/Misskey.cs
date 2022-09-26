using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;

namespace MisskeyCommentViewer
{
	internal class Misskey
	{
		public static ClientWebSocket ws = new ClientWebSocket();
		public static WebSocket4Net.WebSocket WebSocket;
		public event EventHandler<EventArgs> ReceiveLiveComment;
		public string livetag = "";
		public bool WebSocketConnectState
		{
			get
			{
				return WebSocket.State == WebSocket4Net.WebSocketState.Open||WebSocket.State == WebSocket4Net.WebSocketState.Connecting;
			}
		}
		public Misskey()
		{

		}
		public void ConnectAsync()
		{
			if (WebSocket == null || WebSocket.State != WebSocket4Net.WebSocketState.Open)
			{
				WebSocket = new WebSocket4Net.WebSocket("wss://misskey.io/streaming");
				WebSocket.MessageReceived += WebSocket_MessageReceived;
				WebSocket.Error += WebSocket_Error;
				WebSocket.Opened += WebSocket_Opened;
				WebSocket.Open();
			}
			else
			{
				WebSocket.Close();
				WebSocket = new WebSocket4Net.WebSocket("wss://misskey.io/streaming");
				WebSocket.MessageReceived += WebSocket_MessageReceived;
				WebSocket.Error += WebSocket_Error;
				WebSocket.Opened += WebSocket_Opened;
				WebSocket.Open();
			}
		}

		private void WebSocket_Opened(object sender, EventArgs e)
		{
			MisskeyConnectObj senddata = new MisskeyConnectObj() { body = new MisskeyConnectBody() { channel = "hashtag", id = "3" }, type = "connect"};
			senddata.body.param = new Dictionary<string, List<List<string>>>();
			senddata.body.param.Add("q", new List<List<string>> { new List<string> { livetag } });
			string senddata_json = JsonConvert.SerializeObject(senddata);
			
			WebSocket.Send(senddata_json);
			Console.WriteLine(sender.ToString());
		}

		private void WebSocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
		{
			ConnectAsync();
            Console.WriteLine(e.Exception.Message);
		}

		private void WebSocket_MessageReceived(object sender, WebSocket4Net.MessageReceivedEventArgs e)
		{
			EventHandler<EventArgs> eventHandler = ReceiveLiveComment;
			string txt = e.Message.ToString();
			Console.WriteLine(txt);
			MisskeyReceiveObj misskeyReceiveObj = JsonConvert.DeserializeObject<MisskeyReceiveObj>(txt);
			bool tagflag = false;
			if (misskeyReceiveObj == null) return;
			if (misskeyReceiveObj.body.body.text == String.Empty || misskeyReceiveObj.body.body.text == null) return;
			if (misskeyReceiveObj.body.body.tags.Count == 0) return;
			foreach (string tag in misskeyReceiveObj.body.body.tags)
			{
				if (tag.ToLower() == livetag.ToLower())
				{
					tagflag = true;
				}
			}
			if (tagflag)
			{
				eventHandler?.Invoke(txt, new EventArgs());
			}
		}
	}
	public class MisskeyConnectObj
	{
		[JsonProperty("type")]
		public string type { get; set; } = string.Empty;
		[JsonProperty("body")]
		public MisskeyConnectBody body { get; set; } = new MisskeyConnectBody();
	}
	public class MisskeyConnectBody
	{
		[JsonProperty("channel")]
		public string channel { get; set; } = string.Empty;
		[JsonProperty("id")]
		public string id { get; set; } = string.Empty;
		[JsonProperty("params")]
		public Dictionary<string, List<List<string>>> param;
	}
	public class MisskeyReceiveObj
	{
		[JsonProperty("type")]
		public string type { get; set; } = string.Empty;
		[JsonProperty("body")]
		public MisskeyReceiveBody_1 body { get; set; } = new MisskeyReceiveBody_1();
	}
	public class MisskeyReceiveBody_1
	{
		[JsonProperty("id")]
		public string id { get; set; } = string.Empty;
		[JsonProperty("type")]
		public string type { get; set; } = string.Empty;
		[JsonProperty("body")]
		public MisskeyReceiveBody_2 body { get; set; } = new MisskeyReceiveBody_2();
	}
	public class MisskeyReceiveBody_2
	{
		[JsonProperty("id")]
		public string id { get; set; } = string.Empty;
		[JsonProperty("createdAt")]
		public string createdAt { get; set; } = string.Empty;
		[JsonProperty("userId")]
		public string userId { get; set; } = string.Empty;
		[JsonProperty("user")]
		public MisskeyReceiveUser user { get; set; } = new MisskeyReceiveUser();
		[JsonProperty("text")]
		public string text { get; set; }
		[JsonProperty("cw")]
		public string cw { get; set; }
		[JsonProperty("visibility")]
		public string visibility { get; set; } = string.Empty;
		[JsonProperty("renoteCount")]
		public int renoteCount { get; set; } = 0;
		[JsonProperty("repliesCount")]
		public int repliesCount { get; set; } = 0;
		[JsonProperty("reactions")]
		public Dictionary<string,int> reactions { get; set; }
		[JsonProperty("tags")]
		public List<string> tags { get; set; } = new List<string>();
		[JsonProperty("emojis")]
		public List<Dictionary<string,string>> emojis { get; set; } = new List<Dictionary<string, string>>();
		[JsonProperty("fileIds")]
		public List<string> fileIds { get; set; } = new List<string>();
		[JsonProperty("files")]
		public List<(string, string)> files { get; set; } = new List<(string, string)>();
		[JsonProperty("replayId")]
		public string replayId { get; set; }
		[JsonProperty("renoteId")]
		public string renoteId { get; set; }
	}
	public class MisskeyReceiveUser
	{
		[JsonProperty("id")]
		public string id { get; set; } = string.Empty;
		[JsonProperty("name")]
		public string name { get; set; } = string.Empty;
		[JsonProperty("username")]
		public string username { get; set; } = string.Empty;
		[JsonProperty("host")]
		public string host { get; set; }
		[JsonProperty("avatarUrl")]
		public string avatarUrl { get; set; }
		[JsonProperty("avatarBlurhash")]
		public string avatarBlurhash { get; set; }
		[JsonProperty("avatarColor")]
		public string avatarColor { get; set; }
		[JsonProperty("instance")]
		public MisskeyReceiveInstance instance { get; set; }
		[JsonProperty("isModerator")]
		public bool isModerator { get; set; } = false;
		[JsonProperty("isCat")]
		public bool isCat { get; set; } = false;
		[JsonProperty("onlineStatus")]
		public string onlineStatus { get; set; } = "unknown";
	}
	public class MisskeyReceiveInstance
	{
		[JsonProperty("name")]
		public string name { get; set; } = string.Empty;
		[JsonProperty("softwareName")]
		public string softwareName { get; set; } = string.Empty;
		[JsonProperty("softwareVersion")]
		public string softwareVersion { get; set; } = string.Empty;
		[JsonProperty("iconUrl")]
		public string iconUrl { get;set;} = string.Empty;
		[JsonProperty("faviconUrl")]
		public string faviconUrl { get;set;} = string.Empty;
		[JsonProperty("themeColor")]
		public string themeColor { get;set;} = string.Empty;
	}

}
