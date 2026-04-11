using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models
{
    public class RangeMerkleNode
    {
        public byte[] Hash { get; set; }
        public string StartKey { get; set; }
        public string EndKey { get; set; }

        public RangeMerkleNode Left { get; set; }
        public RangeMerkleNode Right { get; set; }

        public bool IsLeaf => Left == null && Right == null;
    }
}
