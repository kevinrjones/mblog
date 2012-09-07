using System;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using MBlog.Filters;
using MBlog.Infrastructure;
using MBlog.Models.User;
using MBlogModel;
using MBlogServiceInterfaces;
using System.Linq;

public class BasicAuthorizeAttribute : AuthorizeAttribute
{
    private bool _RequireSsl = true;

    public bool RequireSsl
    {
        get { return _RequireSsl; }
        set { _RequireSsl = value; }
    }

    public IUserService UserService { get; set; }
    private AuthorizationContext _filterContext;

    private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
    {
        validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
    }

    public override void OnAuthorization(AuthorizationContext filterContext)
    {
        if (filterContext == null) throw new ArgumentNullException("filterContext");

        _filterContext = filterContext;

        if (!Authenticate(filterContext.HttpContext))
        {
            filterContext.Result = new HttpBasicUnauthorizedResult();
        }
        else
        {
            if (AuthorizeCore(filterContext.HttpContext))
            {
                HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
                cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                cachePolicy.AddValidationCallback(CacheValidateHandler, null);
            }
            else
            {
                filterContext.Result = new HttpBasicUnauthorizedResult();
            }
        }
    }

    private bool Authenticate(HttpContextBase context)
    {
        if (RequireSsl && !context.Request.IsSecureConnection && !context.Request.IsLocal) return false;

        if (!context.Request.Headers.AllKeys.Contains("Authorization")) return false;

        string authHeader = context.Request.Headers["Authorization"];

        IPrincipal principal;
        if (TryGetPrincipal(authHeader, out principal))
        {
            HttpContext.Current.User = Thread.CurrentPrincipal = principal;
            return true;
        }
        return false;
    }

    private bool TryGetPrincipal(string authHeader, out IPrincipal principal)
    {
        var creds = ParseAuthHeader(authHeader);
        if (creds != null)
        {
            if (TryGetPrincipal(creds[0], creds[1], out principal)) return true;
        }

        principal = null;
        return false;
    }

    private string[] ParseAuthHeader(string authHeader)
    {
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Basic")) return null;

        string base64Credentials = authHeader.Substring(6);
        string[] credentials =
            Encoding.ASCII.GetString(Convert.FromBase64String(base64Credentials)).Split(new char[] { ':' });

        if (credentials.Length != 2 || string.IsNullOrEmpty(credentials[0]) || string.IsNullOrEmpty(credentials[0]))
            return null;

        return credentials;
    }

    private bool TryGetPrincipal(string userName, string password, out IPrincipal principal)
    {
        User user = UserService.GetUser(userName);

        if (user != null && user.MatchPassword(password))
        {
            UserViewModel userViewModel = UpdateCookies(user);
            principal = userViewModel;
            return true;
        }
        principal = null;
        return false;
    }

    private UserViewModel UpdateCookies(User user)
    {
        var userViewModel = new UserViewModel { Email = user.Email, Name = user.Name, IsLoggedIn = true, Id = user.Id };

        if (_filterContext.HttpContext.Request.Cookies[GetCookieUserFilterAttribute.UserCookieName] == null)
        {
            byte[] cipherText = user.Id.ToString().Encrypt();
            string base64CipherText = Convert.ToBase64String(cipherText);
            _filterContext.HttpContext.Response.Cookies.Add(new HttpCookie(GetCookieUserFilterAttribute.UserCookieName,
                                                                           base64CipherText));
        }
        userViewModel.AddNicknamesToUser(user);
        return userViewModel;
    }

}
