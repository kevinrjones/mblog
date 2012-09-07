// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace MBlog.Controllers {
    public partial class FeedController {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected FeedController(Dummy d) : base(d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Rss() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Rss);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Atom() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Atom);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public FeedController Actions { get { return MVC.Feed; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Feed";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Feed";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass {
            public readonly string Rss = "Rss";
            public readonly string Atom = "Atom";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants {
            public const string Rss = "Rss";
            public const string Atom = "Atom";
        }


        static readonly ActionParamsClass_Rss s_params_Rss = new ActionParamsClass_Rss();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Rss RssParams { get { return s_params_Rss; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Rss {
            public readonly string nickname = "nickname";
        }
        static readonly ActionParamsClass_Atom s_params_Atom = new ActionParamsClass_Atom();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Atom AtomParams { get { return s_params_Atom; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Atom {
            public readonly string nickname = "nickname";
        }
        static readonly ViewNames s_views = new ViewNames();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewNames Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewNames {
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class T4MVC_FeedController: MBlog.Controllers.FeedController {
        public T4MVC_FeedController() : base(Dummy.Instance) { }

        public override System.Web.Mvc.ActionResult Rss(string nickname) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Rss);
            callInfo.RouteValueDictionary.Add("nickname", nickname);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Atom(string nickname) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Atom);
            callInfo.RouteValueDictionary.Add("nickname", nickname);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591