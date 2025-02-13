﻿using É_Mutabakat.Core.Ultilities.Result.Abstract;
using E_Mutabakat.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Mutabakat.Business.Abstract
{
   public interface IMailTemplateService
    {
        IResult Add(MailTemplate mailTemplate);
        IResult Update(MailTemplate mailTemplate);
        IResult Delete(MailTemplate mailTemplate);
        IDataResult<MailTemplate> Get(int id);
        IDataResult<MailTemplate> GetByTemplateName(string name,int companyId);
        IDataResult<List<MailTemplate>> GetAll(int companyId);
    }
}
