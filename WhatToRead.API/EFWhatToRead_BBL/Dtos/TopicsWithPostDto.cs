using EFTopics.DAL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWhatToRead_BBL.Dtos
{
    public class TopicsWithPostsDto
    {
        public int TopicId { get; set; }
        public string Name { get; set; }
        public IEnumerable<PostDto> Posts { get; set; }
    }
}
