using System;
using System.Data;
using System.Data.SqlClient;

namespace Staad.Domain.Impl
{
    using Staad.Domain.Scheme;

    public class SqlRepositoryBase
    {
        public static StaadDbDataContext DataContext { get; private set; }

        public SqlRepositoryBase(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            if (DataContext != null)
            {
                return;
            }

            IDbConnection connection = new SqlConnection(connectionString);
            DataContext = new StaadDbDataContext(connection);
        }
    }
}