using Shared.Models;
using System.Security.Cryptography;
using System.Text;

namespace Shared.Services
{
    public class MerkleService
    {
        private readonly SHA256 _sha = SHA256.Create();

        private byte[] Hash(byte[] input) => _sha.ComputeHash(input);

        private string Serialize(Row r)
            => $"{r.Id}|{r.Value}|{r.Timestamp}";

        public RangeMerkleNode Build(SortedDictionary<string, Row> data)
        {
            var leaves = data.Values.Select(r =>
            {
                var hash = Hash(Encoding.UTF8.GetBytes(Serialize(r)));

                return new RangeMerkleNode
                {
                    Hash = hash,
                    StartKey = r.Id,
                    EndKey = r.Id
                };
            }).ToList();

            return BuildTree(leaves);
        }

        private RangeMerkleNode BuildTree(List<RangeMerkleNode> nodes)
        {
            if (nodes.Count == 1) return nodes[0];

            var parents = new List<RangeMerkleNode>();

            for (int i = 0; i < nodes.Count; i += 2)
            {
                var left = nodes[i];
                var right = (i + 1 < nodes.Count) ? nodes[i + 1] : left;

                parents.Add(new RangeMerkleNode
                {
                    Hash = Hash(left.Hash.Concat(right.Hash).ToArray()),
                    StartKey = left.StartKey,
                    EndKey = right.EndKey,
                    Left = left,
                    Right = right
                });
            }

            return BuildTree(parents);
        }
    }
}
