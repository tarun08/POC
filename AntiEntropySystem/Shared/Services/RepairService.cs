using Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Services
{
    public class RepairService
    {
        public void Repair(Row source, Row target)
        {
            if (source.Timestamp > target.Timestamp)
            {
                target.Value = source.Value;
                target.Timestamp = source.Timestamp;
            }
        }
    }
}
