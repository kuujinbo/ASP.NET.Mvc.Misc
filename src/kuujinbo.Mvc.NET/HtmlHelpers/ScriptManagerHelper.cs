﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace kuujinbo.Mvc.NET.HtmlHelpers
{
    /// <summary>
    /// Solve the **HUGE** ASP.NET MVC script manager omission.
    /// </summary>
    public static class ScriptManagerHelper
    {
        public const string InvalidAddInlineScriptParameter = "script";

        public static readonly string ItemsKey = typeof(ScriptManagerHelper).ToString();
        public static readonly string SrcKey = typeof(ScriptManagerHelper).ToString() + "_SRC";

        public static MvcHtmlString AddScriptSrc(this HtmlHelper helper, string src)
        {
            if (helper.ViewContext.HttpContext.Items[SrcKey] != null)
            {
                ((IList<string>)helper.ViewContext.HttpContext.Items[SrcKey]).Add(src);
            }
            else
            {
                helper.ViewContext.HttpContext.Items[SrcKey] = new List<string>() { src };
            }

            return new MvcHtmlString(String.Empty);
        }

        /// <summary>
        /// Add JavaScript to any view
        /// </summary>
        /// <remarks>
        /// </remarks>
        public static MvcHtmlString AddInlineScript(this HtmlHelper helper, 
            string script, 
            string scriptKey = null)
        {
            if (string.IsNullOrWhiteSpace(script)) throw new ArgumentException(InvalidAddInlineScriptParameter);

            if (scriptKey != null)
            {
                if (helper.ViewContext.HttpContext.Items.Contains(scriptKey))
                {
                    return new MvcHtmlString(String.Empty);
                }
                helper.ViewContext.HttpContext.Items[scriptKey] = null;
            }

            if (helper.ViewContext.HttpContext.Items[ItemsKey] != null)
            {
                ((IList<string>)helper.ViewContext.HttpContext.Items[ItemsKey]).Add(script);
            }
            else
            {
                helper.ViewContext.HttpContext.Items[ItemsKey] = new List<string>() { script };
            }

            return new MvcHtmlString(String.Empty);
        }

        /// <summary>
        /// Render JavaScript from all AddInlineScript() calls.
        /// Use the Helper in ~/Views/Shared/_Layout.cshtml
        /// </summary>
        /// <remarks>
        /// SAMPLE USAGE in 
        /// <!--
        /// @Scripts.Render("~/bundles/0")
        /// @Scripts.Render("~/bundles/1")
        /// @RenderSection("scripts", required: false)
        /// @Html.RenderViewScripts()
        /// </body>
        /// </html>
        /// -->
        /// </remarks>
        public static MvcHtmlString RenderViewScripts(this HtmlHelper helper)
        {
            if (helper.ViewContext.HttpContext.Items[ItemsKey] != null)
            {
                var scripts = (IList<string>)helper.ViewContext.HttpContext.Items[ItemsKey];

                helper.ViewContext.Writer.WriteLine(@"<script type='text/javascript'>");
                foreach (var script in scripts)
                {
                    helper.ViewContext.Writer.WriteLine(script);
                }
                helper.ViewContext.Writer.WriteLine("</script>");
            }

            if (helper.ViewContext.HttpContext.Items[SrcKey] != null)
            {
                var urls = (IList<string>)helper.ViewContext.HttpContext.Items[SrcKey];
                foreach (var url in urls)
                {
                    helper.ViewContext.Writer.WriteLine(string.Format(
                        "<script type='text/javascript' src='{0}'></script>", url
                    ));
                }
            }

            return new MvcHtmlString(String.Empty);
        }
    }
}