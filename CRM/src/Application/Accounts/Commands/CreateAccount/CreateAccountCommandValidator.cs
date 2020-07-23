using CRM.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CRM.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateAccountCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
                .MustAsync(HaveAUniqueName).WithMessage("The specified account name already exists.");

            RuleFor(v => v.Website)
                .MaximumLength(500).WithMessage("Website must not exceed 500 characters.");
            
            RuleFor(v => v.Email)
                .EmailAddress().WithMessage("Email must be valid.")
                .MaximumLength(50).WithMessage("Email must not exceed 50 characters.");

            RuleFor(v => v.PhoneNumber)
                .MaximumLength(50).WithMessage("PhoneNumber must not exceed 50 characters.");
        }

        public async Task<bool> HaveAUniqueName(string name, CancellationToken cancellationToken)
        {
            return await _context.Accounts.AllAsync(l => l.Name != name);
        }
    }
}
