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
    public partial class PostController {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected PostController(Dummy d) { }

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
        public System.Web.Mvc.ActionResult Index() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Index);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult New() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.New);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Create() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Create);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Edit() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Edit);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Update() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Update);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Delete() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Delete);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Show() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Show);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public PostController Actions { get { return MVC.Post; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Post";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Post";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass {
            public readonly string Index = "Index";
            public readonly string New = "New";
            public readonly string Create = "Create";
            public readonly string Edit = "Edit";
            public readonly string Update = "Update";
            public readonly string Delete = "Delete";
            public readonly string Show = "Show";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants {
            public const string Index = "Index";
            public const string New = "New";
            public const string Create = "Create";
            public const string Edit = "Edit";
            public const string Update = "Update";
            public const string Delete = "Delete";
            public const string Show = "Show";
        }


        static readonly ActionParamsClass_Index s_params_Index = new ActionParamsClass_Index();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Index IndexParams { get { return s_params_Index; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Index {
            public readonly string nickname = "nickname";
        }
        static readonly ActionParamsClass_New s_params_New = new ActionParamsClass_New();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_New NewParams { get { return s_params_New; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_New {
            public readonly string nickname = "nickname";
            public readonly string blogId = "blogId";
        }
        static readonly ActionParamsClass_Create s_params_Create = new ActionParamsClass_Create();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Create CreateParams { get { return s_params_Create; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Create {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_Edit s_params_Edit = new ActionParamsClass_Edit();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Edit EditParams { get { return s_params_Edit; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Edit {
            public readonly string nickname = "nickname";
            public readonly string blogId = "blogId";
            public readonly string postId = "postId";
        }
        static readonly ActionParamsClass_Update s_params_Update = new ActionParamsClass_Update();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Update UpdateParams { get { return s_params_Update; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Update {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_Delete s_params_Delete = new ActionParamsClass_Delete();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Delete DeleteParams { get { return s_params_Delete; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Delete {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_Show s_params_Show = new ActionParamsClass_Show();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Show ShowParams { get { return s_params_Show; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Show {
            public readonly string postLinkViewModel = "postLinkViewModel";
        }
        static readonly ViewNames s_views = new ViewNames();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewNames Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewNames {
            public readonly string _EditPost = "~/Views/Post/_EditPost.cshtml";
            public readonly string _PostLayout = "~/Views/Post/_PostLayout.cshtml";
            public readonly string _ShowCommentsForPost = "~/Views/Post/_ShowCommentsForPost.cshtml";
            public readonly string _ShowSinglePost = "~/Views/Post/_ShowSinglePost.cshtml";
            public readonly string Edit = "~/Views/Post/Edit.cshtml";
            public readonly string Index = "~/Views/Post/Index.cshtml";
            public readonly string InvalidDelete = "~/Views/Post/InvalidDelete.cshtml";
            public readonly string New = "~/Views/Post/New.cshtml";
            public readonly string Show = "~/Views/Post/Show.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class T4MVC_PostController: MBlog.Controllers.PostController {
        public T4MVC_PostController() : base(Dummy.Instance) { }

        public override System.Web.Mvc.ActionResult Index(string nickname) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Index);
            callInfo.RouteValueDictionary.Add("nickname", nickname);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult New(string nickname, int blogId) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.New);
            callInfo.RouteValueDictionary.Add("nickname", nickname);
            callInfo.RouteValueDictionary.Add("blogId", blogId);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Create(MBlog.Models.Post.EditPostViewModel model) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Create);
            callInfo.RouteValueDictionary.Add("model", model);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Edit(string nickname, int blogId, int postId) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Edit);
            callInfo.RouteValueDictionary.Add("nickname", nickname);
            callInfo.RouteValueDictionary.Add("blogId", blogId);
            callInfo.RouteValueDictionary.Add("postId", postId);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Update(MBlog.Models.Post.EditPostViewModel model) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Update);
            callInfo.RouteValueDictionary.Add("model", model);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Delete(MBlog.Models.Post.EditPostViewModel model) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Delete);
            callInfo.RouteValueDictionary.Add("model", model);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Show(MBlog.Models.Post.PostLinkViewModel postLinkViewModel) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Show);
            callInfo.RouteValueDictionary.Add("postLinkViewModel", postLinkViewModel);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
