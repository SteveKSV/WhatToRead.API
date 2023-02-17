using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToRead.API.Core.Models;
using Dapper;
using WhatToRead.API.AdoNet.DB.Repositories.Interfaces;
using WhatToRead.API.AdoNet.DB.Repositories;
using System.Data;

namespace WhatToRead.Infrastructure.Repositories
{
    public class LanguageRepository : GenericRepository<Book_Language>, ILanguageRepository
    {
        public LanguageRepository(SqlConnection sqlConnection, IDbTransaction dbtransaction) : base(sqlConnection, dbtransaction, "[CatalogSchema].Book_Language") { }
    }
}
