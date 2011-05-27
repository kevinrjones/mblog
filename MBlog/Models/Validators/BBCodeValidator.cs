using System;
using System.ComponentModel.DataAnnotations;
using CodeKicker.BBCode;

namespace MBlog.Models.Validators
{
    public class BBCodeValidator : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                BBCode.ToHtml(value.ToString());
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}