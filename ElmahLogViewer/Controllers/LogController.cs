using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using ElmahLogViewer.Data;
using ElmahLogViewer.Models;
using ElmahLogViewer.Models.ELMAH;

namespace ElmahLogViewer.Controllers
{
    public class LogController : Controller
    {
        private ElmahConfigDataContext context = new ElmahConfigDataContext();

        private Func<Guid, ServerForm> getServer = s =>
        {
            return new ElmahConfigDataContext()
                .ELMAH_Servers.Where(i => i.ServerId == s).Select(i => new ServerForm
            {
                ServerId = i.ServerId,
                Name = i.Name,
                Environment = i.Environment,
                ConnectionString = i.ConnectionString,
            }).FirstOrDefault();
        };

        #region Server List

        //
        // GET: /Log/
        [HttpGet]

        public ActionResult Index(
            int startIndex = 0,
            int perPage = 15,
            string sort = "Name",
            SortDir sortDir = SortDir.ASC)
        {
            ViewData["startIndex"] = startIndex;
            ViewData["perPage"] = perPage;
            ViewData["sort"] = sort;
            ViewData["sortDir"] = sortDir;

            var v = context.ELMAH_Servers;
            return View(new ServerList
            {
                Results = v
                    .OrderBy(string.Format("{0} {1}", sort, sortDir))
                    .Skip(startIndex).Take(perPage)
                    .Select(i => new ServerForm
                    {
                        ServerId = i.ServerId,
                        Name = i.Name,
                        ConnectionString = i.ConnectionString,
                        Environment = i.Environment,
                    }),
                Constraints = new SetConstraints<string>
                {
                    Sort = sort,
                    SortDir = sortDir,
                    StartIndex = startIndex,
                    PerPage = perPage,
                    Total = v.Count(),
                    Page = (startIndex / perPage) + 1,
                    Filter = string.Empty,
                },
            });
        }

        public ActionResult Filter(
            string filter,
            int startIndex = 0,
            int perPage = 15,
            string sort = "Name",
            SortDir sortDir = SortDir.ASC)
        {
            ViewData["startIndex"] = startIndex;
            ViewData["perPage"] = perPage;
            ViewData["sort"] = sort;
            ViewData["sortDir"] = sortDir;
            ViewData["filter"] = filter;
            var v = context.ELMAH_Servers.Where(i =>
                                    i.Name.Contains(filter) ||
                                    i.Environment.Contains(filter) ||
                                    i.ConnectionString.Contains(filter));
            return View("Index", new ServerList
            {
                Results = v
                    .OrderBy(string.Format("{0} {1}", sort, sortDir))
                    .Skip(startIndex).Take(perPage)
                    .Select(i => new ServerForm
                    {
                        ServerId = i.ServerId,
                        Name = i.Name,
                        ConnectionString = i.ConnectionString,
                        Environment = i.Environment,
                    }),
                Constraints = new SetConstraints<string>
                {
                    Sort = sort,
                    SortDir = sortDir,
                    StartIndex = startIndex,
                    PerPage = perPage,
                    Total = v.Count(),
                    Page = (startIndex / perPage) + 1,
                    Filter = filter,
                },
            });
        }

        public ActionResult AdvancedFilter(
            string environment = null,
            string name = null,
            string connectionString = null,
            int startIndex = 0,
            int perPage = 15,
            string sort = "Name",
            SortDir sortDir = SortDir.ASC)
        {
            ViewData["startIndex"] = startIndex;
            ViewData["perPage"] = perPage;
            ViewData["sort"] = sort;
            ViewData["sortDir"] = sortDir;
            ViewData["environment"] = environment;
            ViewData["name"] = name;
            ViewData["connectionString"] = connectionString;
            var v = context.ELMAH_Servers.Where(i =>
                        (string.IsNullOrWhiteSpace(environment) || i.Environment.Contains(environment)) &&
                        (string.IsNullOrWhiteSpace(name) || i.Name.Contains(name)) &&
                        (string.IsNullOrWhiteSpace(connectionString) || i.ConnectionString.Contains(connectionString))
                    );

            return View(new ServerList
            {
                Results = v
                    .OrderBy(string.Format("{0} {1}", sort, sortDir))
                    .Skip(startIndex).Take(perPage)
                    .Select(i => new ServerForm
                    {
                        ServerId = i.ServerId,
                        Name = i.Name,
                        ConnectionString = i.ConnectionString,
                        Environment = i.Environment,
                    }),
                Constraints = new SetConstraints<ServerForm>
                {
                    Sort = sort,
                    SortDir = sortDir,
                    StartIndex = startIndex,
                    PerPage = perPage,
                    Total = v.Count(),
                    Page = (startIndex / perPage) + 1,
                    Filter = new ServerForm
                    {
                        Environment = environment,
                        Name = name,
                        ConnectionString = connectionString
                    }
                },
            });
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new ServerForm());
        }

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
            SortDir sortDir = SortDir.DESC)
        {
            var server = getServer(serverId);
            if (server == null) return RedirectToAction("Index");

            ViewData["startIndex"] = startIndex;
            ViewData["perPage"] = perPage;
            ViewData["sort"] = sort;
            ViewData["sortDir"] = sortDir;
            ViewData["serverId"] = server.ServerId;

            var logContext = new ElmahConfigDataContext(server.ConnectionString);

            var v = logContext.ELMAH_Errors;

            return View(new ErrorList
            {
                Server = server,
                Results = v
                    .OrderBy(string.Format("{0} {1}", sort, sortDir))
                    .Skip(startIndex)
                    .Take(perPage)
                    .Select(i => new ElmahError
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
                        ServerId = server.ServerId,
                    }),
                Constraints = new SetConstraints<string>
                {
                    Sort = sort,
                    SortDir = sortDir,
                    StartIndex = startIndex,
                    PerPage = perPage,
                    Total = v.Count(),
                    Page = (startIndex / perPage) + 1,
                    Filter = string.Empty,
                }
            });
        }

        public ActionResult Search(
            Guid serverId,
            string filter,
            int startIndex = 0,
            int perPage = 15,
            string sort = "sequence",
            SortDir sortDir = SortDir.DESC)
        {
            var server = getServer(serverId);
            if (server == null) return RedirectToAction("Index");

            ViewData["startIndex"] = startIndex;
            ViewData["perPage"] = perPage;
            ViewData["sort"] = sort;
            ViewData["sortDir"] = sortDir;
            ViewData["filter"] = filter;
            ViewData["serverId"] = server.ServerId;

            var logContext = new ElmahConfigDataContext(server.ConnectionString);

            var v = logContext.ELMAH_Errors.Where(i =>
                i.AllXml.Contains(filter) ||
                i.Application.Contains(filter) ||
                i.Host.Contains(filter) ||
                i.Message.Contains(filter) ||
                i.Source.Contains(filter) ||
                i.Type.Contains(filter) ||
                i.User.Contains(filter)
                );

            return View("ErrorLog", new ErrorList
            {
                Server = server,
                Results = v
                    .OrderBy(string.Format("{0} {1}", sort, sortDir))
                    .Skip(startIndex)
                    .Take(perPage)
                    .Select(i => new ElmahError
                    {
                        Application = i.Application,
                        Host = i.Host,
                        ErrorId = i.ErrorId,
                        Message = i.Message,
                        Sequence = i.Sequence,
                        TimeUtc = i.TimeUtc,
                        Source = i.Source,
                        StatusCode = i.StatusCode,
                        User = i.User,
                        Type = i.Type,
                        ServerId = server.ServerId,
                    }),
                Constraints = new SetConstraints<string>
                {
                    Sort = sort,
                    SortDir = sortDir,
                    StartIndex = startIndex,
                    PerPage = perPage,
                    Total = v.Count(),
                    Page = (startIndex / perPage) + 1,
                    Filter = filter,
                }
            });
        }

        public ActionResult Error(Guid serverId, Guid errorId)
        {
            var server = getServer(serverId);
            if (server == null) return RedirectToAction("Index");

            var logContext = new ElmahConfigDataContext(server.ConnectionString);

            var error = logContext.ELMAH_Errors.Where(i => i.ErrorId == errorId).FirstOrDefault();

            if (error == null) return RedirectToAction("Index");

            ViewData["serverId"] = server.ServerId;

            var e = Elmah.ErrorXml.DecodeString(error.AllXml);

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

            var logContext = new ElmahConfigDataContext(server.ConnectionString);

            var error = logContext.ELMAH_Errors.Where(i => i.ErrorId == errorId).FirstOrDefault();

            if (error == null) return RedirectToAction("Index");

            var e = Elmah.ErrorXml.DecodeString(error.AllXml);

            // TODO: figure out how to generate the original html page.  Investigate the Elmah source code.

            return Content(e.WebHostHtmlMessage);
        }

        #endregion Error Log
    }
}