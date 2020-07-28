using AutoMapper;
using CRM.Application.Common.Mappings;
using CRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CRM.Application.Accounts.Queries.GetAccountById
{
    public class AccountDto : IMapFrom<Account>
    {
        public AccountDto()
        {
            Contacts = new List<ContactDto>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }

        public IList<ContactDto> Contacts { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Account, AccountDto>()
                .ForMember(
                    d => d.Contacts,
                    opt => opt.MapFrom(s =>
                        s.AccountContact.Select(ac =>
                            new ContactDto()
                            {
                                Id = ac.Contact.Id,
                                FirstName = ac.Contact.FirstName,
                                LastName = ac.Contact.LastName,
                                Website = ac.Contact.Website,
                                Email = ac.Contact.Email,
                                PhoneNumber = ac.Contact.PhoneNumber,
                                IsActive = ac.Contact.IsActive,
                                IsPrimary = ac.IsPrimary,
                                SortOrder = ac.SortOrder
                            }
                        )
                    )
                );
        }
    }
}
