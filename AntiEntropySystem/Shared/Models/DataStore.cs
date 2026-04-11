using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models
{
    public class DataStore
    {
        private readonly SortedDictionary<string, Row> _data = new();

        public void Put(Row row)
        {
            _data[row.Id] = row;
        }

        public SortedDictionary<string, Row> GetAll() => _data;

        public IEnumerable<Row> GetRange(string start, string end)
        {
            return _data.Values.Where(r =>
                string.Compare(r.Id, start) >= 0 &&
                string.Compare(r.Id, end) <= 0);
        }
    }
}
