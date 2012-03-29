using System;
using System.Configuration;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using ElmahLogViewer.Areas.Elmah.Data;
using ElmahLogViewer.Areas.Elmah.Models;
using ErrorXml = Elmah.ErrorXml;

namespace ElmahLogViewer.Areas.Elmah.Controllers
{
    public class LogController : Controller
    {
        #region Internals

        private DataContext context = new DataContext(ConfigurationManager.ConnectionStrings[Properties.Settings.Default.ConectionStringKey].ConnectionString);

        private Func<Guid, ServerForm> getServer = s =>
        {
            return new DataContext(ConfigurationManager.ConnectionStrings[Properties.Settings.Default.ConectionStringKey].ConnectionString)
                .ELMAH_Servers.Where(i => i.ServerId == s).Select(i => new ServerForm
                {
                    ServerId = i.ServerId,
                    Name = i.Name,
                    Environment = i.Environment,
                    ConnectionString = i.ConnectionString,
                }).FirstOrDefault();
        };

        private SetConstraints GetConstraints(int startIndex, int perPage, string sort, SortDirection sortDir, int total)
        {
            return new SetConstraints
            {
                Sort = sort,
                SortDir = sortDir,
                StartIndex = startIndex,
                PerPage = perPage,
                Total = total,
                Page = startIndex / perPage,
            };
        }

        private IQueryable<ServerForm> GetServerResults(int startIndex, int perPage, string sort, SortDirection sortDir, IQueryable<ELMAH_Server> v)
        {
            return v.GetQueryResults(startIndex, perPage, sort, sortDir, i => new ServerForm
                {
                    ServerId = i.ServerId,
                    Name = i.Name,
                    ConnectionString = i.ConnectionString,
                    Environment = i.Environment,
                });
        }

        private ErrorList GetErrorList(int startIndex, int perPage, string sort, SortDirection sortDir, ServerForm server, IQueryable<ELMAH_Error> v)
        {
            return new ErrorList
            {
                Server = server,
                Results = GetErrorResults(startIndex, perPage, sort, sortDir, server.ServerId, v),
                Constraints = GetConstraints(startIndex, perPage, sort, sortDir, v.Count()),
            };
        }

        private IQueryable<ElmahError> GetErrorResults(int startIndex, int perPage, string sort, SortDirection sortDir, Guid serverId, IQueryable<ELMAH_Error> v)
        {
            return v.GetQueryResults(startIndex, perPage, sort, sortDir, i => new ElmahError
                {
                    Application = i.Application,
                    Host = i.Host,
                    ErrorId = i.ErrorId,
                    Message = (i.Message.Contains("ViewState:")) ? i.Message.Substring(0, i.Message.IndexOf("ViewState: ")) + "ViewState: ..." : i.Message,
                    Sequence = i.Sequence,
                    TimeUtc = i.TimeUtc,
                    Source = i.Source,
                    StatusCode = i.StatusCode,
                    User = i.User,
                    Type = i.Type,
                    ServerId = serverId,
                });
        }

        #region Set View Data

        private void SetPageingViewData(int startIndex, int perPage, string sort, SortDirection sortDir)
        {
            ViewData["startIndex"] = startIndex;
            ViewData["perPage"] = perPage;
            ViewData["sort"] = sort;
            ViewData["sortDir"] = sortDir;
        }

        private void SetFilterViewData(string filter)
        {
            ViewData["filter"] = filter;
        }

        private void SetServerViewData(ServerForm server)
        {
            ViewData["serverId"] = server.ServerId;
        }

        private void SetFilterViewData(string environment, string name, string connectionString)
        {
            ViewData["environment"] = environment;
            ViewData["name"] = name;
            ViewData["connectionString"] = connectionString;
        }

        private void SetFilterViewData(string application, string errorId, string host, string message, int? fromSequence, int? toSequence, string source, int? statusCode, DateTime? fromTimeUtc, DateTime? toTimeUtc, string type, string user)
        {
            ViewData["application"] = application;
            ViewData["errorId"] = errorId;
            ViewData["host"] = host;
            ViewData["message"] = message;
            ViewData["fromSequence"] = fromSequence;
            ViewData["toSequence"] = toSequence;
            ViewData["source"] = source;
            ViewData["statusCode"] = statusCode;
            ViewData["fromTimeUtc"] = fromTimeUtc;
            ViewData["toTimeUtc"] = toTimeUtc;
            ViewData["type"] = type;
            ViewData["user"] = user;
        }

        private void SetExtendedFilterViewData(
            string application,
            string errorId,
            string host,
            string message,
            int? fromSequence,
            int? toSequence,
            string source,
            int? statusCode,
            DateTime? fromTimeUtc,
            DateTime? toTimeUtc,
            string type,
            string user,
            DataContext context)
        {
            var applications = ((from i in context.ELMAH_Errors
                                 where string.IsNullOrWhiteSpace(host) || host == i.Host
                                 select new
                                 {
                                     Text = i.Application,
                                     Value = i.Application,
                                     Selected = !string.IsNullOrWhiteSpace(application) && i.Application == application,
                                 }).Distinct().ToArray().Select(i => new SelectListItem
                                 {
                                     Text = (i.Text.Where(j => j == '/').Count() > 1) ?
                                        (
                                            i.Text.EndsWith("/Root") ?
                                            i.Text.Split("/".ToCharArray()).Reverse().Skip(1).FirstOrDefault() :
                                            i.Text.Split("/".ToCharArray()).LastOrDefault()
                                        ) :
                                        i.Text,
                                     Value = i.Value,
                                     Selected = i.Selected,
                                 })
                                .Union(new SelectListItem[] {
                                    new SelectListItem {
                                        Text="ALL",
                                        Value=string.Empty,
                                        Selected=string.IsNullOrWhiteSpace(application)
                                    }
                                })).OrderBy(i => i.Value);

            var hosts = ((from i in context.ELMAH_Errors
                          where string.IsNullOrWhiteSpace(application) || application == i.Application
                          select new SelectListItem
                          {
                              Text = i.Host,
                              Value = i.Host,
                              Selected = !string.IsNullOrWhiteSpace(host) && i.Host == host,
                          }).Distinct().ToArray()
                         .Union(new SelectListItem[] {
                             new SelectListItem {
                                Text="ALL",
                                Value=string.Empty,
                                Selected=string.IsNullOrWhiteSpace(host)
                             }
                         })).OrderBy(i => i.Value);
            var sources = ((from i in context.ELMAH_Errors
                            where (string.IsNullOrWhiteSpace(host) || host == i.Host) &&
                            (string.IsNullOrWhiteSpace(application) || application == i.Application)
                            select new SelectListItem
                            {
                                Text = i.Source,
                                Value = i.Source,
                                Selected = !string.IsNullOrWhiteSpace(source) && i.Type == source,
                            }).Distinct().ToArray()
                            .Union(new SelectListItem[] {
                                new SelectListItem {
                                    Text="ALL",
                                    Value=string.Empty,
                                    Selected=string.IsNullOrWhiteSpace(source)
                                }
                            })).OrderBy(i => i.Value);

            var statusCodes = ((from i in context.ELMAH_Errors
                                where (string.IsNullOrWhiteSpace(host) || host == i.Host) &&
                                (string.IsNullOrWhiteSpace(application) || application == i.Application)
                                select new SelectListItem
                                {
                                    Text = i.StatusCode.ToString(),
                                    Value = i.StatusCode.ToString(),
                                    Selected = statusCode != null && i.StatusCode == statusCode,
                                }).Distinct().ToArray()
                                .Union(new SelectListItem[] {
                                    new SelectListItem {
                                        Text = "ALL",
                                        Value = string.Empty,
                                        Selected = statusCode == null,
                                    }
                                })).OrderBy(i => i.Value);

            var types = ((from i in context.ELMAH_Errors
                          where (string.IsNullOrWhiteSpace(host) || host == i.Host) &&
                          (string.IsNullOrWhiteSpace(application) || application == i.Application)
                          select new SelectListItem
                          {
                              Text = i.Type,
                              Value = i.Type,
                              Selected = !string.IsNullOrWhiteSpace(type) && i.Type == type,
                          }).Distinct().ToArray()
                            .Union(new SelectListItem[] {
                                new SelectListItem {
                                    Text="ALL",
                                    Value=string.Empty,
                                    Selected=string.IsNullOrWhiteSpace(type)
                                }
                            })).OrderBy(i => i.Value);

            ViewData["applications"] = applications;
            ViewData["hosts"] = hosts;
            ViewData["sources"] = sources;
            ViewData["statusCodes"] = statusCodes;
            ViewData["types"] = types;
        }

        #endregion Set View Data

        #endregion Internals

        #region Server List

        //
        // GET: /Log/
        [HttpGet]
        public ActionResult Index(
            int startIndex = 0,
            int perPage = 15,
            string sort = "Name",
            SortDirection sortDir = SortDirection.Ascending)
        {
            SetPageingViewData(startIndex, perPage, sort, sortDir);
            var v = context.ELMAH_Servers.AsQueryable();
            return View(new ServerList
            {
                Results = GetServerResults(startIndex, perPage, sort, sortDir, v),
                Constraints = GetConstraints(startIndex, perPage, sort, sortDir, v.Count()),
            });
        }

        public ActionResult Filter(
            string filter,
            int startIndex = 0,
            int perPage = 15,
            string sort = "Name",
            SortDirection sortDir = SortDirection.Ascending)
        {
            SetPageingViewData(startIndex, perPage, sort, sortDir);
            SetFilterViewData(filter);
            var v = context.ELMAH_Servers.Where(i =>
                                    i.Name.Contains(filter) ||
                                    i.Environment.Contains(filter) ||
                                    i.ConnectionString.Contains(filter));
            return View("Index", new ServerList
            {
                Results = GetServerResults(startIndex, perPage, sort, sortDir, v),
                Constraints = GetConstraints(startIndex, perPage, sort, sortDir, v.Count()),
            });
        }

        public ActionResult AdvancedFilter(
            string environment = null,
            string name = null,
            string connectionString = null,
            int startIndex = 0,
            int perPage = 15,
            string sort = "Name",
            SortDirection sortDir = SortDirection.Ascending)
        {
            SetPageingViewData(startIndex, perPage, sort, sortDir);
            SetFilterViewData(environment, name, connectionString);

            var v = context.ELMAH_Servers.Where(i =>
                    (string.IsNullOrWhiteSpace(environment) || i.Environment.Contains(environment)) &&
                    (string.IsNullOrWhiteSpace(name) || i.Name.Contains(name)) &&
                    (string.IsNullOrWhiteSpace(connectionString) || i.ConnectionString.Contains(connectionString))
                );

            return View(new ServerList
            {
                Results = GetServerResults(startIndex, perPage, sort, sortDir, v),
                Constraints = GetConstraints(startIndex, perPage, sort, sortDir, v.Count()),
            });
        }

        [HttpGet]
        public ActionResult Create() { return View(new ServerForm()); }

        [HttpPost]
        public ActionResult Create(ServerForm server)
        {
            if (!this.ModelState.IsValid)
                return View(server);

            try
            {
                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(server.ConnectionString))
                {
                    conn.Open();
                }
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("ConnectionString", ex);
            }

            if (context.ELMAH_Servers.Where(i => i.Name.ToLower() == server.Name).Count() > 0)
                this.ModelState.AddModelError("Name", "Server Name is already in use.");

            if (!this.ModelState.IsValid)
                return View(server);

            ELMAH_Server s = new ELMAH_Server
            {
                Name = server.Name,
                ConnectionString = server.ConnectionString,
                Environment = server.Environment,
                ServerId = Guid.NewGuid(),
            };

            context.ELMAH_Servers.InsertOnSubmit(s);
            context.SubmitChanges();

            return RedirectToAction("ErrorLog", new { serverId = s.ServerId });
        }

        [HttpGet]
        public ActionResult Delete(Guid serverId)
        {
            return View(context.ELMAH_Servers.Where(i => i.ServerId == serverId).Select(i => new ServerForm
            {
                ServerId = i.ServerId,
                Environment = i.Environment,
                ConnectionString = i.ConnectionString,
                Name = i.Name
            }).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Delete(Guid serverId, bool confirm)
        {
            if (confirm)
            {
                if (context.ELMAH_Servers.Where(i => i.ServerId == serverId).Count() == 0)
                    this.ModelState.AddModelError("serverId", "Server does not exist.");

                if (!this.ModelState.IsValid)
                    return View(serverId);

                context.ELMAH_Servers.DeleteOnSubmit(
                    context.ELMAH_Servers.Where(i => i.ServerId == serverId).FirstOrDefault());

                context.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

        #endregion Server List

        #region Error Log

        public ActionResult ErrorLog(
            Guid serverId,
            int startIndex = 0,
            int perPage = 15,
            string sort = "sequence",
            SortDirection sortDir = SortDirection.Descending)
        {
            var server = getServer(serverId);
            if (server == null) return RedirectToAction("Index");

            SetPageingViewData(startIndex, perPage, sort, sortDir);
            SetServerViewData(server);

            var logContext = new DataContext(server.ConnectionString);

            SetExtendedFilterViewData(null, null, null, null, null, null, null, null, null, null, null, null, logContext);

            var v = logContext.ELMAH_Errors.AsQueryable();

            return View(GetErrorList(startIndex, perPage, sort, sortDir, server, v));
        }

        public ActionResult Search(
            Guid serverId,
            string filter,
            int startIndex = 0,
            int perPage = 15,
            string sort = "sequence",
            SortDirection sortDir = SortDirection.Descending)
        {
            var server = getServer(serverId);
            if (server == null) return RedirectToAction("Index");

            if (string.IsNullOrWhiteSpace(filter))
                return RedirectToAction("ErrorLog", new { serverId = serverId });

            SetPageingViewData(startIndex, perPage, sort, sortDir);
            SetFilterViewData(filter);
            SetServerViewData(server);

            var logContext = new DataContext(server.ConnectionString);

            SetExtendedFilterViewData(null, null, null, null, null, null, null, null, null, null, null, null, logContext);

            var v = logContext.ELMAH_Errors.Where(i =>
                i.AllXml.Contains(filter) ||
                i.Application.Contains(filter) ||
                i.Host.Contains(filter) ||
                i.Message.Contains(filter) ||
                i.Source.Contains(filter) ||
                i.Type.Contains(filter) ||
                i.User.Contains(filter)
                );

            return View("ErrorLog", GetErrorList(startIndex, perPage, sort, sortDir, server, v));
        }

        public ActionResult AdvancedSearch(
            Guid serverId,
            string application = null,
            string errorId = null,
            string host = null,
            string message = null,
            int? fromSequence = null,
            int? toSequence = null,
            string source = null,
            int? statusCode = null,
            DateTime? fromTimeUtc = null,
            DateTime? toTimeUtc = null,
            string type = null,
            string user = null,
            int startIndex = 0,
            int perPage = 15,
            string sort = "sequence",
            SortDirection sortDir = SortDirection.Descending
            )
        {
            var server = getServer(serverId);
            if (server == null) return RedirectToAction("Index");

            SetPageingViewData(startIndex, perPage, sort, sortDir);
            SetServerViewData(server);
            SetFilterViewData(application, errorId, host, message, fromSequence, toSequence, source, statusCode, fromTimeUtc, toTimeUtc, type, user);

            var logContext = new DataContext(server.ConnectionString);

            SetExtendedFilterViewData(application, errorId, host, message, fromSequence, toSequence, source, statusCode, fromTimeUtc, toTimeUtc, type, user, logContext);

            var v = logContext.ELMAH_Errors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(application)) v = v.Where(i => i.Application.Contains(application.Trim()));
            if (!string.IsNullOrWhiteSpace(errorId)) v = v.Where(i => i.ErrorId.ToString().Contains(errorId.Trim()));
            if (!string.IsNullOrWhiteSpace(host)) v = v.Where(i => i.Host.Contains(host.Trim()));
            if (!string.IsNullOrWhiteSpace(message)) v = v.Where(i => i.Message.Contains(message.Trim()));
            if (fromSequence.HasValue) v = v.Where(i => i.Sequence >= fromSequence.Value);
            if (toSequence.HasValue) v = v.Where(i => i.Sequence <= toSequence.Value);
            if (!string.IsNullOrWhiteSpace(source)) v = v.Where(i => i.Source.Contains(source.Trim()));
            if (statusCode.HasValue) v = v.Where(i => i.StatusCode == statusCode.Value);
            if (fromTimeUtc.HasValue) v = v.Where(i => i.TimeUtc >= fromTimeUtc.Value);
            if (toTimeUtc.HasValue) v = v.Where(i => i.TimeUtc <= toTimeUtc.Value);
            if (!string.IsNullOrWhiteSpace(type)) v = v.Where(i => i.Type.Contains(type.Trim()));
            if (!string.IsNullOrWhiteSpace(user)) v = v.Where(i => i.User.Contains(user.Trim()));

            return View("ErrorLog", GetErrorList(startIndex, perPage, sort, sortDir, server, v));
        }

        public ActionResult Error(Guid serverId, Guid errorId)
        {
            var server = getServer(serverId);
            if (server == null) return RedirectToAction("Index");

            var logContext = new DataContext(server.ConnectionString);

            var error = logContext.ELMAH_Errors.Where(i => i.ErrorId == errorId).FirstOrDefault();

            if (error == null) return RedirectToAction("Index");

            ViewData["serverId"] = server.ServerId;

            var e = ErrorXml.DecodeString(error.AllXml);

            return View(
                new Error
                {
                    ServerDetails = server,
                    ElmahError = e,
                    ErrorDetails = new ElmahError
                    {
                        Application = error.Application,
                        Host = error.Host,
                        ErrorId = error.ErrorId,
                        Message = error.Message,
                        Sequence = error.Sequence,
                        TimeUtc = error.TimeUtc,
                        Source = error.Source,
                        StatusCode = error.StatusCode,
                        User = error.User,
                        Type = error.Type,
                        ServerId = server.ServerId,
                    }
                });
        }

        public ActionResult Html(Guid serverId, Guid errorId)
        {
            var server = getServer(serverId);

            if (server == null) return RedirectToAction("Index");

            var logContext = new DataContext(server.ConnectionString);

            var error = logContext.ELMAH_Errors.Where(i => i.ErrorId == errorId).FirstOrDefault();

            if (error == null) return RedirectToAction("Index");

            var e = ErrorXml.DecodeString(error.AllXml);

            if (string.IsNullOrWhiteSpace(e.WebHostHtmlMessage))
                return RedirectToAction("Error", new { serverId = serverId, errorId = errorId });

            return Content(e.WebHostHtmlMessage);
        }

        #endregion Error Log
    }
}