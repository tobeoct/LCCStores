using LCCStores.Logic;
using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace LCCStores.Helper
{
    public class Validations<T> where T : class, new()
    {
        EntityLogic<T> _entityLogic;
        EntityLogic<ProductImage> _entityLogicImage;
        EntityLogic<ProductDetail> _entityLogicDetail;
        string _error = "";

        public Validations()
        {
            _entityLogic = new EntityLogic<T>();
            _entityLogicImage = new EntityLogic<ProductImage>();
            _entityLogicDetail = new EntityLogic<ProductDetail>();

        }
        public string ValidateData(T data)
        {
          
            //VALIDATE PRODUCT
           
            PropertyInfo[] properties = data.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))//&& property.GetValue(product) != null)
                {
                    ProcessString(property.GetValue(data).ToString(), property.Name);
                }
                if (property.PropertyType == typeof(int))//&& property.GetValue(product) != null)
                {
                    ProcessNumbers(property.GetValue(data).ToString(), property.Name);
                }
                if (property.PropertyType == typeof(decimal) )//&& property.GetValue(product) != null)
                {
                    ProcessDecimals(property.GetValue(data).ToString(), property.Name);
                }
            }
            return _error;
        }
        public void ProcessString(string value, string name)
        {
            
            if (String.IsNullOrEmpty(value))
            {
                _error = _error+  $"{name} cannot be null or empty";
            }
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");

            if (!regexItem.IsMatch(value)) {
                _error = _error + $"{name} cannot contain special characters only numbers[0-9] and letters[a-z]";
            }

        }
        public void ProcessNumbers(string value, string name)
        {
            

                if (!IsNumber(value))
            {
                _error =_error+ $"-{name} only accepts numbers";
            }
                if(Convert.ToInt64(value)<0)
            {
                _error = _error + $"-{name} cannot be less than 0";
            }


        }
        public void ProcessDecimals(string value, string name)
        {

          
            decimal number;
            if (!Decimal.TryParse(value, out number))
            {
                _error = _error + $"-{name} only accepts numbers";
            }
            if (Convert.ToDecimal(value) < 0)
            {
                _error = _error + $"-{name} cannot be less than 0";
            }

        }
        public static bool IsNumber(string s)
        {
            return s.All(char.IsDigit);
        }
    }
}