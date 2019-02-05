using LCCStores.Logic;
using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.Helper
{
    public class Updates
    {
        public bool ProductsUpdate()
        {
            try
            {
                var _logic = new EntityLogic<ProductUpdate>();
                var updateTime = _logic.GetSingle(c => c.Id == 1);
                if (updateTime != null)
                {
                    updateTime.LastUpdateTime = DateTime.Now;
                    _logic.Update(updateTime);
                    return true;
                }
                var upTime = new ProductUpdate()
                {
                    LastUpdateTime = DateTime.Now
                };
                _logic.Insert(upTime);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool BrandsUpdate()
        {
            try
            {
                var _logic = new EntityLogic<BrandUpdate>();
                var updateTime = _logic.GetSingle(c => c.Id == 1);
                if (updateTime != null)
                {
                    updateTime.LastUpdateTime = DateTime.Now;
                    _logic.Update(updateTime);
                    return true;
                }
                var upTime = new BrandUpdate()
                {
                    LastUpdateTime = DateTime.Now
                };
                _logic.Insert(upTime);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }

        }

    }
}