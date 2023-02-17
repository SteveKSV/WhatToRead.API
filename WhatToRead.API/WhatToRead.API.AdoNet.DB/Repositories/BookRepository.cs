using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToRead.API.AdoNet.DB.Repositories;
using WhatToRead.API.AdoNet.DB.Repositories.Interfaces;
using WhatToRead.Core.Models;

namespace WhatToRead.Infrastructure.Repositories
{
    public class BookRepository : GenericRepository<Book>,IBookRepository
    {
        public BookRepository(SqlConnection sqlConnection, IDbTransaction dbtransaction) : base(sqlConnection, dbtransaction, "[CatalogSchema].Book") { }
    }
}
