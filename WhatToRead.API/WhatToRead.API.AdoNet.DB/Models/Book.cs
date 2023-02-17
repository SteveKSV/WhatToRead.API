using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToRead.API.AdoNet.DB.Models;

namespace WhatToRead.Core.Models
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public int Language_Id { get; set; }
        public int NumberOfPages { get; set; }
        public DateTime Publication_Date { get; set; }
        public int Publisher_Id { get; set;
        }
    }
}
