using Microsoft.AspNetCore.Identity;
using RuthMo.Models;

namespace RuthMo.Controllers;

public class NoOpEmailSender<T> : IEmailSender<User>
{
    public Task SendConfirmationLinkAsync(T user, string email, string confirmationLink) => Task.CompletedTask;
    public Task SendPasswordResetLinkAsync(T user, string email, string resetLink) => Task.CompletedTask;
    public Task SendPasswordResetCodeAsync(T user, string email, string resetCode) => Task.CompletedTask;

    public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        throw new NotImplementedException();
    }

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        throw new NotImplementedException();
    }

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        throw new NotImplementedException();
    }
}