﻿using E_Mutabakat.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Mutabakat.Entities.Dtos
{
    public class UserCompanyRegisterDto
    {
        public UserForRegisterDto UserForRegister { get; set; }
       public  Company company { get; set; }
    }
}
