using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc.Properties;

namespace GuildfordBoroughCouncil.Web.Mvc.Html
{
    internal static class EnumHelper
    {
        internal static object GetModelStateValue(this HtmlHelper htmlHelper, string key, Type destinationType)
        {
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(key, out modelState))
            {
                if (modelState.Value != null)
                {
                    return modelState.Value.ConvertTo(destinationType, null /* culture */);
                }
            }
            return null;
        }

        internal static bool HasFlags(Type type)
        {
            Contract.Assert(type != null);

            Type checkedType = Nullable.GetUnderlyingType(type) ?? type;
            return HasFlagsInternal(checkedType);
        }

        private static bool HasFlagsInternal(Type type)
        {
            Contract.Assert(type != null);

            FlagsAttribute attribute = type.GetCustomAttribute<FlagsAttribute>(inherit: false);
            return attribute != null;
        }
    }
}
