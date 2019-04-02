using System;
using System.IO;
using Wox.Plugin;
using System.Windows;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace WoxPlugins.SteamGuard {
    public class SteamGuard : IPlugin {
        private string _secret;
        private PluginInitContext _context;
        private string GetCurrentSteamGuardCode(string secret) {
            int[] steam_guard_code_table = { 50, 51, 52, 53, 54, 55, 56, 57, 66, 67, 68, 70, 71,
                                             72, 74, 75, 77, 78, 80, 81, 82, 84, 86, 87, 88, 89 };

            byte[] decoded_secret = Convert.FromBase64String(secret);
            long time_val = (long)((DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds / 30);
            byte[] time_bytes = BitConverter.GetBytes(time_val);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(time_bytes);
            HMACSHA1 hmac = new HMACSHA1(decoded_secret);
            byte[] hashed_data = hmac.ComputeHash(time_bytes);
            int b = hashed_data[19] & 0xF;
            int code_point = (hashed_data[b] & 0x7F) << 24 |
                             (hashed_data[b + 1] & 0xFF) << 16 |
                             (hashed_data[b + 2] & 0xFF) << 8 |
                             (hashed_data[b + 3] & 0xFF);
            List<char> guard_code_arr = new List<char>();
            for (int i = 0; i < 5; i++) {
                int char_code = steam_guard_code_table[code_point % steam_guard_code_table.Length];
                guard_code_arr.Add(Convert.ToChar(char_code));
                code_point = (int)Math.Floor((double)code_point / steam_guard_code_table.Length);
            }
            return new string(guard_code_arr.ToArray());
        }
        public void Init(PluginInitContext context) {
            string secret_path = Path.Combine(context.CurrentPluginMetadata.PluginDirectory, "SECRET");
            _secret = File.ReadAllText(secret_path).Trim();
            _context = context;
        }
        public List<Result> Query(Query query) {
            List<Result> results = new List<Result>();
            string guard_code = GetCurrentSteamGuardCode(_secret);
            results.Add(new Result() {
                Title = guard_code,
                SubTitle = "Steam 令牌",
                IcoPath = "img\\steam.png",
                Action = _ => {
                    Clipboard.SetText(guard_code);
                    _context.API.ShowMsg("已复制", guard_code, "img\\steam.png");
                    return true;
                }
            });
            return results;
        }
    }
}
