using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nejire
{
    class Config
    {
        private const string pastacfg = "Resources";
        private const string arquivocfg = "config.json";
        public static NejireConfig Nejire;

        static Config()
        {
            if (!Directory.Exists(pastacfg))
                Directory.CreateDirectory(pastacfg);

            if (!File.Exists(pastacfg + "/" + arquivocfg))
            {
                Nejire = new NejireConfig();
                string json = JsonConvert.SerializeObject(Nejire, Formatting.Indented);
                File.WriteAllText(pastacfg + "/" + arquivocfg, json);
            }
            else
            {
                string json = File.ReadAllText(pastacfg + "/" + arquivocfg);
                Nejire = JsonConvert.DeserializeObject<NejireConfig>(json);
            }
        }
    }

    public struct NejireConfig
    {
        public string Token { get; set; }
        public string Prefix { get; set; }


    }
}
