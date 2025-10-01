using System.ComponentModel.DataAnnotations;

namespace OrderingServiceWeb.Attributes
{
    public class ApplicationLogEventAttribute : ValidationAttribute
    {
        private readonly string[] _allowedValues = [
            "LOGIN_SUCCESS",
            "LOGIN_FAILURE",
            "CUSTOMER_CREATED",
            "CUSTOMER_UPDATED",
            "ORDER_CREATED",
            "ITEM_CREATED",
            "ITEM_UPDATED",
            "ITEM_DELETED",
            "ITEM_VISITED",
            "LOGS_ACCESSED"
        ];

        public ApplicationLogEventAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is string stringValue)
            {
                if (_allowedValues.Contains(stringValue, StringComparer.OrdinalIgnoreCase))
                    return ValidationResult.Success;

                return new ValidationResult($"Value must be one of: {string.Join(", ", _allowedValues)}");
            }

            return new ValidationResult("Value is required");
        }
    }
}
