﻿using E_Mutabakat.Business.Abstract;
using E_Mutabakat.Business.Constans;
using É_Mutabakat.Core.Ultilities.Result.Abstract;
using É_Mutabakat.Core.Ultilities.Result.Concrete;
using E_Mutabakat.DataAccess.Abstract;
using E_Mutabakat.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Mutabakat.Business.Concrete
{
    internal class MailTemplateManager : IMailTemplateService
    {
        private readonly IMailTemplateDal _mailTemplateDal;

        public MailTemplateManager(IMailTemplateDal mailTemplateDal)
        {
            _mailTemplateDal = mailTemplateDal;
        }
        public IResult Add(MailTemplate mailTemplate)
        {
            _mailTemplateDal.Add(mailTemplate);
            return new SuccessResult(Messages.MailTemplateAdded);
        }

        public IResult Delete(MailTemplate mailTemplate)
        {
            _mailTemplateDal.Delete(mailTemplate);
            return new SuccessResult(Messages.MailTemplateDeleted);
        }

        public IDataResult<MailTemplate> Get(int id)
        {
            return new SuccesDataResult<MailTemplate>(_mailTemplateDal.Get(m => m.Id == id));
        }

        public IDataResult<List<MailTemplate>> GetAll(int companyId)
        {
            return new SuccesDataResult<List<MailTemplate>>(_mailTemplateDal.GetList(m => m.CompanyId == companyId));
        }

        public IDataResult<MailTemplate> GetByTemplateName(string name, int companyId)
        {
            return new SuccesDataResult<MailTemplate>(_mailTemplateDal.Get(m => m.Type == name && m.CompanyId == companyId));
        }

        public IResult Update(MailTemplate mailTemplate)
        {
            _mailTemplateDal.Update(mailTemplate);
            return new SuccessResult(Messages.MailTemplateUpdated);
        }
    }
}
