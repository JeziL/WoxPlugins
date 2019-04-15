using Wox.Plugin;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections.Generic;

namespace WoxPlugins.FrDict {
    public class FrDict : IPlugin {
        public class FREntry {
            public int cgformidx { get; set; }
            public string hlword { get; set; }
            public bool iscghint { get; set; }
            public bool ischt { get; set; }
            public string label { get; set; }
            public string recordid { get; set; }
            public string recordtype { get; set; }
            public string tag { get; set; }
            public string value { get; set; }
        }

        private static readonly HttpClient _client = new HttpClient();

        public void Init(PluginInitContext context) { }

        public List<Result> Query(Query query) {
            var results = new List<Result>();
            string url = string.Format("http://www.frdic.com/dicts/prefix/{0}", query.Search);
            try {
                string responseBody = _client.GetStringAsync(url).Result;
                var resp = JsonConvert.DeserializeObject<List<FREntry>>(responseBody);
                if (resp.Any()) {
                    resp.ForEach((e) => {
                        bool isCg = (e.tag == "CgSuggestion" && e.recordtype == "CG");
                        if (e.label.Length > 0) {
                            results.Add(new Result() {
                                Title = e.value,
                                SubTitle = e.label,
                                IcoPath = "img\\frdict.png",
                                Action = _ => {
                                    string resultUrl = string.Format("http://www.frdic.com/dicts/fr/{0}?recordid={1}", e.value, e.recordid);
                                    if (isCg) {
                                        resultUrl += string.Format("&forcecg=true&cgformidx={0}", e.cgformidx);
                                    }
                                    Process.Start(resultUrl);
                                    return true;
                                }
                            });
                        }
                    });
                }
                return results;
            }
            catch {
                return results;
            }
        }
    }
}
