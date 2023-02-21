using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.WebSockets;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace MisskeyLiveCommentViewer
{
    internal class Misskey
    {
        public static WebSocket4Net.WebSocket WebSocket;
        public static WebSocket4Net.WebSocket WebSocket1;
        public event EventHandler<EventArgs> ReceiveLiveComment;
        public string livetag = "";
        private string appSecret = string.Empty;
        private string i;
        public MisskeyReceiveUser Account;
        public string instanceurl { get; set; } = "https://misskey.io";
        private string token { get; set; } = string.Empty;
        public bool WebSocketConnectState
        {
            get
            {
                return WebSocket.State == WebSocket4Net.WebSocketState.Open || WebSocket.State == WebSocket4Net.WebSocketState.Connecting;
            }
        }
        public Misskey()
        {

        }
        public void ConnectAsync()
        {
            Connect1();
            Connect2();
        }
        public void Connect1()
        {
            if (WebSocket != null)
            {
                try
                {
                    WebSocket.MessageReceived -= WebSocket_MessageReceived;
                    WebSocket.Error -= WebSocket_Error;
                    WebSocket.Opened -= WebSocket_Opened;
                    WebSocket.Close();
                }
                finally
                {
                    WebSocket = new WebSocket4Net.WebSocket("wss://misskey.io/streaming",
                        sslProtocols: SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls);
                    WebSocket.MessageReceived += WebSocket_MessageReceived;
                    WebSocket.Error += WebSocket_Error;
                    WebSocket.Opened += WebSocket_Opened;

                }
            }
            else
            {
                WebSocket = new WebSocket4Net.WebSocket("wss://misskey.io/streaming",
                    sslProtocols: SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls);
                WebSocket.MessageReceived += WebSocket_MessageReceived;
                WebSocket.Error += WebSocket_Error;
                WebSocket.Opened += WebSocket_Opened;
            }
            WebSocket.Open();
        }
        public void Connect2()
        {
            if (WebSocket1 != null)
            {
                try
                {
                    WebSocket1.MessageReceived -= WebSocket_MessageReceived;
                    WebSocket1.Error -= WebSocket1_Error;
                    WebSocket1.Opened -= WebSocket_Opened;
                    WebSocket1.Close();
                }
                finally
                {
                    WebSocket1 = new WebSocket4Net.WebSocket("wss://misskey.io/streaming",
                        sslProtocols: SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls);

                    WebSocket1.MessageReceived += WebSocket_MessageReceived;
                    WebSocket1.Error += WebSocket1_Error;
                    WebSocket1.Opened += WebSocket_Opened;
                }
            }
            else
            {
                WebSocket1 = new WebSocket4Net.WebSocket("wss://misskey.io/streaming",
                    sslProtocols: SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls);
                WebSocket1.MessageReceived += WebSocket_MessageReceived;
                WebSocket1.Error += WebSocket1_Error;
                WebSocket1.Opened += WebSocket_Opened;
            }
            WebSocket1.Open();
        }

        private void WebSocket_Opened(object sender, EventArgs e)
        {
            MisskeyConnectObj senddata = new MisskeyConnectObj() { body = new MisskeyConnectBody() { channel = "hashtag", id = "3" }, type = "connect" };
            senddata.body.param = new Dictionary<string, List<List<string>>>();
            senddata.body.param.Add("q", new List<List<string>> { new List<string> { livetag } });
            string senddata_json = JsonConvert.SerializeObject(senddata);

            WebSocket.Send(senddata_json);
            Debug.WriteLine(sender.ToString());
        }

        private void WebSocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Debug.WriteLine(e.Exception.Message);
            Connect1();
        }
        private void WebSocket1_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Debug.WriteLine(e.Exception.Message);
            Connect2();
        }

        private void WebSocket_MessageReceived(object sender, WebSocket4Net.MessageReceivedEventArgs e)
        {
            EventHandler<EventArgs> eventHandler = ReceiveLiveComment;
            string txt = e.Message.ToString();
            Debug.WriteLine(txt);
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

        public async Task<bool> GetToken(string url)
        {
            var httpsmatch = System.Text.RegularExpressions.Regex.Match(url, "https://");
            string tmpurl = string.Empty;
            if (httpsmatch.Success)
            {
                tmpurl = url;
            }
            else
            {
                tmpurl = "https://" + url;
            }
            if (tmpurl != instanceurl)
            {
                appSecret = string.Empty;
                token = string.Empty;
                instanceurl = tmpurl;
                i = string.Empty;
            }
            if (appSecret == string.Empty && token == string.Empty)
            {

                string appcreateresponse = await CreateApp(url);
                if (appcreateresponse == null) return false;
                MisskeyReceiveCreateApp appres;
                MisskeyReceiveAuthSessionGenerate authsegenres;
                appres = JsonConvert.DeserializeObject<MisskeyReceiveCreateApp>(appcreateresponse);
                appSecret = appres.secret;
                string authres = await AuthSessionGen(appres.secret);
                authsegenres = JsonConvert.DeserializeObject<MisskeyReceiveAuthSessionGenerate>(authres);
                token = authsegenres.token;
                try
                {
                    System.Diagnostics.Process.Start(authsegenres.url);
                    return true;
                }
                catch
                {
                    if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
                    {
                        url = authsegenres.url.Replace("&", "^&");
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                        return true;
                    }
                    else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
                    {
                        System.Diagnostics.Process.Start("xdg-open", url);
                        return true;

                    }
                    else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
                    {
                        System.Diagnostics.Process.Start("open", url);
                        return true;

                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                string authseuserres = await AuthSessionUserkey();
                MisskeyReceiveAuthSessionUserkey authSessionUserkeyRes;
                try
                {
                    authSessionUserkeyRes = JsonConvert.DeserializeObject<MisskeyReceiveAuthSessionUserkey>(authseuserres);
                    if (await CheckToken(authSessionUserkeyRes.accessToken))
                    {
                        i = authSessionUserkeyRes.accessToken;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;
            }
        }
        public string GetI()
        {
            return i ?? String.Empty;
        }
        public async Task<string> CreateApp(string url)
        {
            MisskeySendCreateApp postdata = new MisskeySendCreateApp()
            {
                name = "MisskeyLiveCommentViewer",
                description = "POSTNote MisskeyLiveCommentViewer CreatedBy @nenohi@misskey.io",
                permission = new List<string> { "write:notes" }
            };
            var postjson = JsonConvert.SerializeObject(postdata);
            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(postjson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(instanceurl + "/api/app/create", content);
                var res = await response.Content.ReadAsStringAsync();
                return res;
            }
        }
        public async Task<string> AuthSessionGen(string appSecret)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("appSecret", appSecret);
            var postjson = JsonConvert.SerializeObject(data);
            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(postjson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(instanceurl + "/api/auth/session/generate", content);
                var res = await response.Content.ReadAsStringAsync();
                return res;
            }
        }
        public async Task<string> AuthSessionUserkey()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("appSecret", appSecret);
            data.Add("token", token);
            var postjson = JsonConvert.SerializeObject(data);
            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(postjson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(instanceurl + "/api/auth/session/userkey", content);
                var res = await response.Content.ReadAsStringAsync();
                return res;
            }
        }

        public async Task<bool> CheckToken(string itoken)
        {
            using (HttpClient client = new HttpClient())
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("i", itoken);
                var postjson = JsonConvert.SerializeObject(data);
                var content = new StringContent(postjson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(instanceurl + "/api/i", content);
                var res = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    Account = JsonConvert.DeserializeObject<MisskeyReceiveUser>(res);
                    i = itoken;
                    return true;
                }
                return false;
            }
        }
        public async Task PostNote(string Text)
        {
            if (i == String.Empty) return;
            string txt;
            if (string.IsNullOrEmpty(livetag))
            {
                txt = Text;
            }
            else
            {
                txt = $"{Text}\r\n#MisskeyLive #{livetag.Replace("ml", "ML")}\r\nhttps://live.misskey.io/{livetag.Replace("ml", "@")}";
            }
            Dictionary<string, string> postdata = new Dictionary<string, string>();
            postdata.Add("text", txt);
            postdata.Add("visibility", "home");
            postdata.Add("i", i);
            var postjson = JsonConvert.SerializeObject(postdata);
            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(postjson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(instanceurl + "/api/notes/create", content);
            }
        }
        public void Setting(string AppSecret,string i,string token,string instanceurl,string livetag)
        {
            this.appSecret = AppSecret;
            this.i = i;
            this.token = token;
            this.instanceurl = instanceurl;
            this.livetag = livetag;
        }
        public string AppSecret { get => appSecret; }
        public string I { get => i; }
        public string Token { get => token; }
        public string Instanceurl { get => instanceurl; }
        public string Livetag { get => livetag; }
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
        public Dictionary<string, int> reactions { get; set; }
        [JsonProperty("tags")]
        public List<string> tags { get; set; } = new List<string>();
        [JsonProperty("emojis")]
        public List<Dictionary<string, string>> emojis { get; set; } = new List<Dictionary<string, string>>();
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
        public string iconUrl { get; set; } = string.Empty;
        [JsonProperty("faviconUrl")]
        public string faviconUrl { get; set; } = string.Empty;
        [JsonProperty("themeColor")]
        public string themeColor { get; set; } = string.Empty;
    }
    public class MisskeySendCreateApp
    {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("description")]
        public string description { get; set; }
        [JsonProperty("permission")]
        public List<string> permission { get; set; }
        [JsonProperty("callbackUrl")]
        public string callbackUrl { get; set; }
    }
    public class MisskeyReceiveCreateApp
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("callbackUrl")]
        public string callbackUrl { get; set; }
        [JsonProperty("permission")]
        public List<string> permission { get; set; }
        [JsonProperty("secret")]
        public string secret { get; set; }
        [JsonProperty("isAuthorized")]
        public bool isAuthorized { get; set; }
    }
    public class MisskeySendAuthSessionGenerate
    {
        [JsonProperty("appSecret")]
        public string appSecret { get; set; }
    }
    public class MisskeyReceiveAuthSessionGenerate
    {
        [JsonProperty("token")]
        public string token { get; set; }
        [JsonProperty("url")]
        public string url { get; set; }
    }
    public class MisskeyReceiveAuthSessionUserkey
    {
        [JsonProperty("accessToken")]
        public string accessToken { get; set; }
        [JsonProperty("user")]
        public MisskeyReceiveUser user { get; set; }
    }
}
