using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCodeDev.NetCMS.Compiler.Core.Builder
{
    public class CommentBuilder
    {
        public List<string> Comments { get; set; } = new List<string>();


        public void AddLine(string line){
            Comments.Add(line);
        }

        public override string ToString()
        {
            if (Comments.Count > 1)
            {
                return $"/*{Environment.NewLine}{String.Join(Environment.NewLine, Comments)}{Environment.NewLine}*/{Environment.NewLine}";
            }

            return $"//{Environment.NewLine}{String.Join("", Comments)}{Environment.NewLine}";
        }
    }
}
