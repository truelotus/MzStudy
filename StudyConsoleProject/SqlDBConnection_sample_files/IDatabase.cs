using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoSPA.DataMiner.Core.Data;

namespace GoSPA.DataMiner.Core.Database
{
    public interface IDatabase
    {
        void TryConnection();

        decimal InsertApprovedT(ApprovedT item);

        void InsertOrderT(OrderT item);

				void InsertData(System.Collections.IEnumerable dbDataList);

				void UpdateData(System.Collections.IEnumerable dbDataList);
		}
}
