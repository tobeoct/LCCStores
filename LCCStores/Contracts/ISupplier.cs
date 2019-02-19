using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCStores.Contracts
{
    public interface ISupplier
    {
        int Id { get; set; }
        int PersonalInfoId { get; set; }
        PersonalInformation PersonalInfo { get; set; }
        string CompanyName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        int ProductId { get; set; }
        Product Product { get; set; }
        // string Title { get; set; }
        int AddedById { get; set; }
        Admin AddedBy { get; set; }
    }
}
