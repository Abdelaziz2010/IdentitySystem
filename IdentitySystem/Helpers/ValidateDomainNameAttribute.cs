using System.ComponentModel.DataAnnotations;

namespace IdentitySystem.Helpers
{
    public class ValidateDomainNameAttribute : ValidationAttribute
    {
        private readonly string _allowedDomainName;

        public ValidateDomainNameAttribute(string allowedDomainName)
        {
            this._allowedDomainName = allowedDomainName;
        }
        public override bool IsValid(object? value)
        {
            string[] vals = value.ToString().Split('@');

            return vals[1].ToUpper() == _allowedDomainName.ToUpper();
        }
    }
}
