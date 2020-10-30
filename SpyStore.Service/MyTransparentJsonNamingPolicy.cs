using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
namespace SpyStore.Service
{
    public class MyTransparentJsonNamingPolicy :JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name;
        }
    }
}

