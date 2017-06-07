using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CloudinaryDotNet;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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

        public static IEnumerable<T> GetPaginationItems<T>(ref string page, ref string count, IEnumerable<T> items)
        {
            int _page, _count;
            if (page != null)
                _page = int.Parse(page);
            else
                _page = 0;
            if (count != null)
                _count = int.Parse(count);
            else
                _count = 3;
            page = _page.ToString(); count = _count.ToString();
            return items.Skip(_page * _count).Take(_count);
        }

    }
}
