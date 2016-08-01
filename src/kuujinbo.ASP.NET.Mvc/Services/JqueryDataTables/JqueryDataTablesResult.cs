﻿/* ============================================================================
 * convenience class to write JqueryDataTables results in following formats:
 * -- JSON      => XHR call for MVC view
 * -- binary    => excel file dump
 * ============================================================================
 */
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using kuujinbo.ASP.NET.Mvc.Services.Json;

namespace kuujinbo.ASP.NET.Mvc.Services.JqueryDataTables
{
    public sealed class JqueryDataTablesResult : ActionResult
    {
        public static readonly string CONTENT_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public static readonly string FILENAME = string.Format("{0:yyyyMMdd-HHmm}.xlsx", DateTime.Now);

        private ITable _table;
        public JqueryDataTablesResult(ITable table)
        {
            if (table == null) throw new ArgumentNullException("table");
            _table = table;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;
            if (_table.SaveAs)
            {
                response.ContentType = CONTENT_TYPE;
                response.AddHeader(
                    "Content-Disposition",
                    string.Format("attachment; filename={0}", FILENAME)
                );

                var json = new JsonNetSerializer().Get(_table.Data, true);

                var data = JsonConvert.DeserializeObject<List<List<object>>>(json);
                var f = new SimpleExcelFile().Create(data);
                response.OutputStream.Write(f, 0, f.Length);
            }
            else
            {
                response.ContentType = "application/json";
                response.Write(new JsonNetSerializer().Get(
                    new
                    {
                        draw = _table.Draw,
                        recordsTotal = _table.RecordsTotal,
                        recordsFiltered = _table.RecordsFiltered,
                        data = _table.Data
                    },
                    true
                ));            
            }
        }
    }
}