﻿using É_Mutabakat.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Mutabakat.Entities.Concrete
{
    public class MailTemplate:IEntity
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

    }
}
