using EFTopics.DAL.Data;
using EFTopics.DAL.Entities;
using EFWhatToRead_DAL.Repositories.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWhatToRead_DAL.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(ApplicationContext _dbcontext) : base(_dbcontext)
        {
        }


    }
}
