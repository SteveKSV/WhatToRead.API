using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToRead.API.AdoNet.BBL.Dtos
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Language_Id { get; set; }
        public int NumberOfPages { get; set; }
        public DateTime Publication_Date { get; set; }
        public int Publisher_Id { get; set; }
        public int Author_Id { get; set; }
    }
}
