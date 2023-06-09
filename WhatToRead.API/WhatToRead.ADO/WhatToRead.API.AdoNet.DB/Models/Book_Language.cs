using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToRead.API.AdoNet.DB.Models;

namespace WhatToRead.API.Core.Models
{
    public class Book_Language : BaseEntity
    {
        public string Language_Name { get; set;}
    }
}
