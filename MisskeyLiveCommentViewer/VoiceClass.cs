using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MisskeyLiveCommentViewer
{
    public class VoiceClass
    {
        [JsonProperty("fn_index")]
        public int fn_index;
        [JsonProperty("Language")]
        public List<string> Language;
        [JsonProperty("Voice")]
        public List<string> Voice;
    }
}
