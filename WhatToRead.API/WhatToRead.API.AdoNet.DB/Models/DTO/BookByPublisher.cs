using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToRead.API.AdoNet.DB.Models.DTO
{
    public class BookByPublisher
    {
        public string Title { get; set; }
        public int NumberOfPages { get; set; }
        public DateTime Publication_Date { get; set; }
        public string Publisher_Name { get; set; }

        public int Publisher_Id { get; set; }

    }
}
