﻿using É_Mutabakat.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Mutabakat.Entities.Concrete
{
    public class Company : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string TaxDepartment { get; set; }
        public string TaxIdNumber { get; set; }
        public string IdentityNumber{ get; set; }
        public DateTime AddetAt { get; set; }
        public bool IsActive { get; set; }

    }
}
