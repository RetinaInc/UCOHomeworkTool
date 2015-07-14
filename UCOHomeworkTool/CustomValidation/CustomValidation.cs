using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOHomeworkTool.Models;

namespace UCOHomeworkTool.CustomValidation
{
    public class RequiredWhenCreatingAttribute: ValidationAttribute, IClientValidatable 
    {
        protected override ValidationResult IsValid(object value,ValidationContext context)
        {
            var parent = context.ObjectInstance as EditProblemViewModel;
            bool valid = true;
            if (parent.Id == 0)
                valid = value!= null;
            if (!valid)
                return new ValidationResult(ErrorMessage);
            return ValidationResult.Success;
        }        
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context )
        {
            ModelClientValidationRule requiredWhenCreatingRule = new ModelClientValidationRule();
            requiredWhenCreatingRule.ErrorMessage = ErrorMessage;
            requiredWhenCreatingRule.ValidationType = "requiredwhencreating";
            yield return requiredWhenCreatingRule;
        }
    }
}