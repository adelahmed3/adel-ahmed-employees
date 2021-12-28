using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEmployeesPair.Common
{
    public class Helper
    {
        static ConcurrentDictionary<string, IConfigurationRoot> ConfigMap = new ConcurrentDictionary<string, IConfigurationRoot>();

        public static IConfiguration GetConfig(string fileName)
        {
            return ConfigMap.GetOrAdd(fileName, key =>
            {
                return new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile(key + ".json",
                        optional: true,
                        reloadOnChange: false)
                    .Build();
            });
        }
    }
}
