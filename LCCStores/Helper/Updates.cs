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

        public bool CustomersUpdate()
        {
            try
            {
                var _logic = new EntityLogic<CustomerUpdate>();
                var updateTime = _logic.GetSingle(c => c.Id == 1);
                if (updateTime != null)
                {
                    updateTime.LastUpdateTime = DateTime.Now;
                    _logic.Update(updateTime);
                    return true;
                }
                var upTime = new CustomerUpdate()
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
        public bool CartsUpdate()
        {
            try
            {
                var _logic = new EntityLogic<CartUpdate>();
                var updateTime = _logic.GetSingle(c => c.Id == 1);
                if (updateTime != null)
                {
                    updateTime.LastUpdateTime = DateTime.Now;
                    _logic.Update(updateTime);
                    return true;
                }
                var upTime = new CartUpdate()
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
        public bool ReviewsUpdate()
        {
            try
            {
                var _logic = new EntityLogic<ReviewUpdate>();
                var updateTime = _logic.GetSingle(c => c.Id == 1);
                if (updateTime != null)
                {
                    updateTime.LastUpdateTime = DateTime.Now;
                    _logic.Update(updateTime);
                    return true;
                }
                var upTime = new ReviewUpdate()
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
        public bool OrdersUpdate()
        {
            try
            {
                var _logic = new EntityLogic<OrdersUpdate>();
                var updateTime = _logic.GetSingle(c => c.Id == 1);
                if (updateTime != null)
                {
                    updateTime.LastUpdateTime = DateTime.Now;
                    _logic.Update(updateTime);
                    return true;
                }
                var upTime = new OrdersUpdate()
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
        public bool CouriersUpdate()
        {
            try
            {
                var _logic = new EntityLogic<CouriersUpdate>();
                var updateTime = _logic.GetSingle(c => c.Id == 1);
                if (updateTime != null)
                {
                    updateTime.LastUpdateTime = DateTime.Now;
                    _logic.Update(updateTime);
                    return true;
                }
                var upTime = new CouriersUpdate()
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
        public bool SuppliersUpdate()
        {
            try
            {
                var _logic = new EntityLogic<SuppliersUpdate>();
                var updateTime = _logic.GetSingle(c => c.Id == 1);
                if (updateTime != null)
                {
                    updateTime.LastUpdateTime = DateTime.Now;
                    _logic.Update(updateTime);
                    return true;
                }
                var upTime = new SuppliersUpdate()
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