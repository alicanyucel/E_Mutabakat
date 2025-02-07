﻿using É_Mutabakat.Core.Ultilities.Result.Abstract;
using E_Mutabakat.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Mutabakat.Business.Abstract
{
    public interface ICurrencyAccountService
    {
        IResult AddToExcel(string FileName,int companyId);
        IDataResult<List<CurrencyAccount>> GetList(int companyId);
        IDataResult<CurrencyAccount> Get(int id);
        IDataResult<CurrencyAccount> GetByCode(string code,int companyId);
        IResult Delete(CurrencyAccount currencyAccount);
        IResult Update(CurrencyAccount currencyAccount);
        IResult Add(CurrencyAccount currenycAccount);
    }
}
