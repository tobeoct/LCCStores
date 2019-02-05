using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace LCCStores.Configurations
{
    
        public class BrandConfiguration : EntityTypeConfiguration<Brand>
        {
            public BrandConfiguration()
            {
                HasKey(a => a.Id);
                Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

                Property(a => a.Name).IsRequired();
            Property(a => a.AddedById).IsRequired();
        }

        }
    
}