using OpenCodeDev.NetCMS.Compiler.Core.Builder;
using OpenCodeDev.NetCMS.Compiler.Core.Builder.JsonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace OpenCodeDev.NetCMS.Compiler.Core.Extracter
{

    public class UsingsExtractor
    {
        public List<UsingBuilder> _Usings { get; private set; } = new List<UsingBuilder>();

        public UsingsExtractor(string json) : this(JsonSerializer.Deserialize<UsingsModel>(json).Usings) {}

        public UsingsExtractor(List<string> usings)
        {
            LoopUsings(usings);
        }

        private void LoopUsings(List<string> items)
        {
            _Usings = items.Select(p => new UsingBuilder(p.ToString())).ToList();
        }

        public List<UsingBuilder> ToList()
        {
            return _Usings;
        }

        public override string ToString()
        {
            return $"{String.Join(" ", _Usings.Select(p=>p.ToString()))}";
        }
    }
}
