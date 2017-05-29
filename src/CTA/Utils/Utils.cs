using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTA.Utils
{
    public static class Utils
    {

        public enum CarType
        {
            New=1,
            Used=2,
            Disaster=3
        }

        public static IEnumerable Errors(this ModelStateDictionary modelState)
        {
            if(!modelState.IsValid)
            {
                return modelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors
                                                   .Select(e => e.ErrorMessage).ToArray())
                                                .Where(m => m.Value.Count() > 0);
            }
            return null;
        }
    }
}
