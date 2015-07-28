﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace GuildfordBoroughCouncil.Web.Mvc.Html
{
    public static partial class DescriptionExtensions
    {
        public static string DescriptionTextFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);
            var description = metadata.Description;

            return description;
        }

        public static MvcHtmlString DescriptionFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);
            var description = metadata.Description;

            if (!String.IsNullOrWhiteSpace(description))
            {

                var span = new TagBuilder("span");

                span.SetInnerText(description);

                var attributes = new RouteValueDictionary(htmlAttributes);
                span.Attributes.Add(new KeyValuePair<string, string>("class", "help-block help-info"));
                span.MergeAttributes(attributes);

                return new MvcHtmlString(span.ToString(TagRenderMode.Normal));
            }

            return null;
        }
    }
}
