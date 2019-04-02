using Wox.Plugin;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;

namespace WoxPlugins.YouDaoDict {
    public class YDResult {
        public string msg { get; set; }
        public int code { get; set; }
    }
    public class YDEntry {
        public string explain { get; set; }
        public string entry { get; set; }
    }
    public class YDData {
        public List<YDEntry> entries { get; set; }
        public string query { get; set; }
        public string language { get; set; }
        public string type { get; set; }
    }
    public class YDResponse {
        public YDResult result { get; set; }
        public YDData data { get; set; }
    }
    public class YouDaoDict : IPlugin {
        private static readonly HttpClient _client = new HttpClient();
        public void Init(PluginInitContext context) { }
        public List<Result> Query(Query query) {
            var results = new List<Result>();
            string url = string.Format("http://dict.youdao.com/suggest?q={0}&le=eng&num=6&ver=2.0&doctype=json&keyfrom=mdict.7.2.0.android&model=honor&mid=5.6.1&imei=659135764921685&vendor=wandoujia&screen=1080x1800&ssid=fsasakfn&abtest=2", query.Search);
            try {
                string responseBody = _client.GetStringAsync(url).Result;
                YDResponse resp = JsonConvert.DeserializeObject<YDResponse>(responseBody);
                if (resp.result.code == 200 && resp.data.entries.Any()) {
                    resp.data.entries.ForEach((e) => {
                        results.Add(new Result() {
                            Title = e.entry,
                            SubTitle = e.explain,
                            IcoPath = "img\\youdao.ico",
                            Action = _ => {
                                Clipboard.SetText(e.entry);
                                return true;
                            }
                        });
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
