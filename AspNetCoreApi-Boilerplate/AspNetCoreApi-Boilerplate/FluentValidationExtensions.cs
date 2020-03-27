using FluentValidation;
using System;
using System.Collections.Generic;

namespace AspNetCoreApi_Boilerplate
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> HideFromPreValidation<T, TProperty>(this IRuleBuilderOptions<T, TProperty> ruleBuilder)
        {
            return ruleBuilder.Configure(config =>
            {
                config.CurrentValidator.Options.ApplyCondition(p =>
                {
                    IDictionary<string, object> rootContextData = p.ParentContext.RootContextData;
                    if (rootContextData.ContainsKey("RemoveFromPreValidation") &&
                        Object.Equals(rootContextData["RemoveFromPreValidation"], true))
                    {
                        return false;
                    }

                    return true;
                });
            });
        }
    }
}
