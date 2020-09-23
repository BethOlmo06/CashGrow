using CashGrow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace CashGrow.Extensions
{
    public static class InvitationExtensions
    {
        public static async Task SendInvitation(this Invitation invitation)
        {
            var Url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var callbackUrl = Url.Action("AcceptInvitation", "Account", new { recipientEmail = invitation.RecipientEmail, code = invitation.Code}, protocol: HttpContext.Current.Request.Url.Scheme);
            var from = $"CashGrow <betholmo@gmail.com>";

            var emailMessage = new MailMessage(from, invitation.RecipientEmail)
            {
                Subject = "You have been invited to join CashGrow.",
                Body = $"You can create a new account and join as a member by clicking this link: <a  href=\"{callbackUrl}\">Join</a><br /><hr />If you have already created an account, copy and past the following code to join the household. Code: {invitation.Code}",
                IsBodyHtml = true
            };

            var svc = new EmailService();
            await svc.SendAsync(emailMessage);
        }
    }
}