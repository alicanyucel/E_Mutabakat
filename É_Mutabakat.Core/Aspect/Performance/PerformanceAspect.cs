﻿using Castle.DynamicProxy;
using E_Mutabakat.Core.Ultilities.InterCeptors;
using E_Mutabakat.Entities.Concrete;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using E_Mutabakat.Core.Ultilities.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace E_Mutabakat.Core.Aspect.Performance
{
    public class PerformanceAspect : MethodInterCeption
    {
        private int _interval;
        private Stopwatch _stopwatch;

        public PerformanceAspect(int interval)
        {
            _interval = interval;
            _stopwatch = ServiceTool.ServiceProvider.GetService<Stopwatch>();
        }
        protected override void OnBefore(IInvocation invocation)
        {
            _stopwatch.Start();
        }

        protected override void OnAfter(IInvocation invocation)
        {
            if (_stopwatch.Elapsed.TotalSeconds > _interval)
            {
                string body = $"Performance : {invocation.Method.DeclaringType.FullName}.{invocation.Method.Name}--> {_stopwatch.Elapsed.TotalSeconds}";
                SendConfirmEmail(body);
            }
            _stopwatch.Reset();
        }

        void SendConfirmEmail(string body)
        {
            string subject = "Performans Maili";

            SendMailDto sendMailDto = new SendMailDto()
            {
                Email = "emutabakat09@gmail.com",
                Password = "eMutabakat0900",
                Port = 587,
                SMTP = "smtp.gmail.com",
                SSL = true,
                email = "emutabakat09@gmail.com",
                subject = subject,
                body = body
            };

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(sendMailDto.Email);
                mail.To.Add(sendMailDto.email);
                mail.Subject = sendMailDto.subject;
                mail.Body = sendMailDto.body;
                mail.IsBodyHtml = true;
                //mail.Attachments.Add();

                using (SmtpClient smtp = new SmtpClient(sendMailDto.SMTP))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(sendMailDto.Email, sendMailDto.Password);
                    smtp.EnableSsl = sendMailDto.SSL;
                    smtp.Send(mail);
                }
            }
        }
    }
}