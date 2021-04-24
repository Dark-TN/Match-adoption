using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models.Peticion
{
    public class OReCaptcha
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
        [JsonProperty("challenge_ts")]
        public DateTime ChallengeTS { get; set; }
        [JsonProperty("hostname")]
        public string Hostname { get; set; }
    }
}