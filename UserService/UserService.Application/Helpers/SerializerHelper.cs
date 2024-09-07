using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UserService.Application.Helpers
{
    public class SerializerHelper
    {
        public static JsonSerializerOptions SerializerOptions()
        {
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            return serializeOptions;
        }

    }
}
