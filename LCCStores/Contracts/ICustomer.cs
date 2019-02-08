using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface ICustomer
    {
        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        int PersonalInfoId { get; set; }
        PersonalInformation PersonalInfo { get; set; }
        int? BillingInfoId { get; set; }
        BillingInformation BillingInfo { get; set; }
    }
}
