﻿using E_Mutabakat.Business.Abstract;
using E_Mutabakat.Business.Constans;
using E_Mutabakat.Business.ValidationRules.FluentValidation;
using E_Mutabakat.Core.Aspect.Autofac.Transaction;
using E_Mutabakat.Core.CrossCuttingConcerns.Validation;
using E_Mutabakat.Core.Ultilities.Hashing;
using É_Mutabakat.Core.Ultilities.Result.Abstract;
using É_Mutabakat.Core.Ultilities.Result.Concrete;
using E_Mutabakat.Core.Ultilities.Security.Jwt;
using E_Mutabakat.Entities.Concrete;
using E_Mutabakat.Entities.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Mutabakat.Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IMailService _mailService;
        private readonly IMailParameterService _mailParameterService;
        private readonly ICompanyServices _companyservice;
        private readonly ITokenHelpers _tokenHelpers;
        private readonly IUserService _userService;
        public AuthManager(IUserService userService, ITokenHelpers tokenHelpers, IMailTemplateService mailTemplateService, ICompanyServices companyservice, IMailService mailService, IMailParameterService mailParameterService)
        {
            _mailTemplateService = mailTemplateService;
            _mailService = mailService;
            _mailParameterService = mailParameterService;
            _userService = userService;
            _tokenHelpers = tokenHelpers;
            _companyservice = companyservice;
        }

        public IResult CompanyExists(Company company)
        {
            var result = _companyservice.CompanyExists(company);
            if (result.Success == false)
            {
                return new ErrorResult(Messages.CompanyAllReadyExists);

            }
            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(User user, int companyid)
        {
            var claims = _userService.GetClaims(user, companyid);
            var accesstoken = _tokenHelpers.CreateToken(user, claims, companyid);
            return new SuccesDataResult<AccessToken>(accesstoken);

        }
        public IDataResult<User> GetById(int id)
        {
            return new SuccesDataResult<User>(_userService.GetById(id));
        }

        public IDataResult<User> GetByMailConfirmValue(string value)
        {
            return new SuccesDataResult<User>(_userService.GetByMailConfirmValue(value));
        }

        public IDataResult<User> Login(UserForLoginDto userForLogin)
        {
            var userToCheck = _userService.GetByMail(userForLogin.Email);
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(userForLogin.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }

            return new SuccesDataResult<User>(userToCheck, Messages.SuccessfulLogin);

        }
        [TransactionScopeAspect]
        public IDataResult<UserCompanyDto> Register(UserForRegisterDto userForRegister, string password, Company company)
        {
            // bunları encoidng hamcsha512 ile alacaz veri tabanında
            byte[] passwordHash, passwordsalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordsalt);
            var user = new User
            {
                Email = userForRegister.Email,
                AddedAt = DateTime.Now,
                IsActive = true,
                MailConfirm = false,
                MailConfirmDate = DateTime.Now,
                MailConfirmValue = Guid.NewGuid().ToString(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordsalt,
                Name = userForRegister.Name

            };

            ValidationTool.Validate(new UserValidator(), user);
            ValidationTool.Validate(new CompanyValidator(), company);

            _userService.Add(user);
            _companyservice.Add(company);
            _companyservice.UserCompanyAdd(user.Id, company.Id);
            UserCompanyDto userCompanyDto = new UserCompanyDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                AddedAt = user.AddedAt,
                companyid = company.Id,
                IsActive = true,
                MailConfirm = user.MailConfirm,
                MailConfirmDate = user.MailConfirmDate,
                MailConfirmValue = user.MailConfirmValue,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt
            };
            SendConfirmEmail(user);
            return new SuccesDataResult<UserCompanyDto>(userCompanyDto, Messages.UserRegistered);
        }
        void SendConfirmEmail(User user)
        {
            string Subject = "kullanici kayit onay maili";
            string link = "https://localhost:7219/api/Auth/confirmuser?value" + user.MailConfirmValue;
            string linkDescription = "kaydi onaylamak icin tiklayiniz";
            string Body = "kullanici sisteme kayit oldu kaydınınızı tamamlamak icin asagidaki baglantiyi tiklayiniz";
            var mailTemplate = _mailTemplateService.GetByTemplateName("kayit", 4);
            var mailparameter = _mailParameterService.Get(4);
            string TemplateBody = mailTemplate.Data.Value;
            TemplateBody = TemplateBody.Replace("{{title}}", Subject);
            TemplateBody = TemplateBody.Replace("{{message}}", Body);
            TemplateBody = TemplateBody.Replace("{{link}}", link);
            TemplateBody = TemplateBody.Replace("{{linkDescription}}", linkDescription);
            var mailParameter = _mailParameterService.Get(4);
            SendMailDtos sendMailDtos = new SendMailDtos()
            {
                mailParameter = mailparameter.Data,
                Email = user.Email,
                Subject = "kullanıcı kayit onay maili",
                Body = TemplateBody
            };
            _mailService.SendEmail(sendMailDtos);
            user.MailConfirmDate = DateTime.Now;
            _userService.Update(user);
        }
        

        public IDataResult<User> RegisterSecondAccount(UserForRegisterDto userForRegister, string password,int companyid)
        {
            byte[] passwordHash, passwordsalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordsalt);
            var user = new User
            {
                Email = userForRegister.Email,
                AddedAt = DateTime.Now,
                IsActive = true,
                MailConfirm = false,
                MailConfirmDate = DateTime.Now,
                MailConfirmValue = Guid.NewGuid().ToString(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordsalt,
                Name = userForRegister.Name

            };

            _userService.Add(user);
            _companyservice.UserCompanyAdd(user.Id,companyid);
            SendConfirmEmail(user);
            return new SuccesDataResult<User>(user, Messages.UserRegistered);

        }

        public IResult Update(User user)
        {
            _userService.Update(user);
            return new SuccessResult(Messages.UserMailConfirmSuccesfull);
        }

        public IResult UserExists(string email)
        {
           if(_userService.GetByMail(email)!=null)
            {
                return new ErrorResult(Messages.UserAlreadyExists);
            }
            return new SuccessResult();
        }

        IResult IAuthService.SentConfirmEmail(User user)
        {
            if(user.MailConfirm==true)
            {
                return new ErrorResult(Messages.MailAlreadyConfirm);
            }
            DateTime confirmmaildate = user.MailConfirmDate;
            DateTime now = DateTime.Now;
            if (confirmmaildate.ToShortDateString() == now.ToShortDateString())
            {
                if (confirmmaildate.Hour == now.Hour && confirmmaildate.AddMinutes(5).Minute <= now.Minute)
                {
                    SendConfirmEmail(user);
                    return new SuccessResult(Messages.MailConfirmTİmeHasNotExpired);
                }
                else
                {
                    return new ErrorResult(Messages.MailConfirmTİmeHasNotExpired);
                }
            }
            SendConfirmEmail(user);
            return new SuccessResult(Messages.MailConfirmSendSuccessfull);

        }

        public IDataResult<UserCompany> GetCompany(int userid)
        {
            return new SuccesDataResult<UserCompany>(_companyservice.GetCompany(userid).Data);
        }
    }
}
