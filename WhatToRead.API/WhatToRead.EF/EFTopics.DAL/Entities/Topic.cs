﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EFTopics.BBL.Entities
{
    public class Topic
    {
        public int TopicId { get; set; }

        public string Name { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<PostBlog> PostBlogs { get; set; }
    }
}
