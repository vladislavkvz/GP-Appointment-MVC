namespace GPAppointment.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class IsEqualAttribute : ValidationAttribute
    {
        private string targetProperty;

        public IsEqualAttribute(string targetProperty)
        {
            this.targetProperty = targetProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(this.targetProperty);
            var referenceProperty = (string)otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

            if (value == null)
                value = "";

            if (value.ToString() != referenceProperty)
                return new ValidationResult("The " + otherPropertyInfo.Name + " doesn't match!");

            return base.IsValid(value, validationContext);
        }

        public override bool IsValid(object value)
        {
            return String.IsNullOrEmpty(this.ErrorMessage);
        }
    }
}