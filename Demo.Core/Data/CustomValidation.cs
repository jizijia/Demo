using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Data
{
    /// <summary>
    /// 自定义验证类BusinessValidations
    /// </summary>
    public static class Validations
    {
        /// <summary>
        /// 验证description不包括！:) :( 等符号
        /// </summary>
        public static ValidationResult DescriptionRules(string value)
        {
            var errors = new System.Text.StringBuilder();
            if (value != null)
            {
                var description = value as string;
                if (description.Contains("!"))
                {
                    errors.AppendLine("Description should not contain '!'.");
                }
                if (description.Contains(":)") || description.Contains(":("))
                {
                    errors.AppendLine("Description should not contain emoticons.");
                }
            }
            if (errors.Length > 0)
                return new ValidationResult(errors.ToString());
            else
                return ValidationResult.Success;
        }
    }
}
