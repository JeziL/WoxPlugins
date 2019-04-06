using System;
using Wox.Plugin;
using System.Windows;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WoxPlugins.NumberBase {
    public class NumberBase : IPlugin {
        class NumBase {
            public int val { get; set; }
            public string name { get; set; }
        }
        public void Init(PluginInitContext context) { }
        private bool isValid(Query query) {
            if (query.Terms.Length != 1) return false;
            if (!Regex.Match(query.FirstSearch, @"^0([xX][\dA-Fa-f]+|[dD]\d+|[bB][01]+)$").Success) return false;
            return true;
        }
        public List<Result> Query(Query query) {
            List<Result> results = new List<Result>();
            if (!isValid(query)) return results;
            var baseDict = new Dictionary<string, NumBase>() {
                { "0b", new NumBase { val=2, name="二进制" } },
                { "0d", new NumBase { val=10, name="十进制" } },
                { "0x", new NumBase { val=16, name="十六进制" } }
            };
            string input = query.FirstSearch.Substring(2);
            int numBase = baseDict[query.FirstSearch.Substring(0, 2)].val;
            int dValue = Convert.ToInt32(input, numBase);
            foreach(var nb in baseDict) {
                if (numBase == nb.Value.val) continue;
                string result = Convert.ToString(dValue, nb.Value.val);
                results.Add(new Result() {
                    Title = result,
                    SubTitle = nb.Value.name,
                    IcoPath = "img\\icon.png",
                    Action = _ => {
                        Clipboard.SetText(result);
                        return true;
                    }
                });
            }
            return results;
        }
    }
}
