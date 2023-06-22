
using BAL.Models;
using DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace UserCrud
{
    
    public class SchedulePost
    {

        //private Timer _timer;
        //private IRepository _repository;

        //public void Start(IRepository repository, HttpContextBase httpContext)
        //{
        //    _repository = repository;
        //    _timer = new Timer(state => CheckScheduledPosts(httpContext), null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        //}


        //private void CheckScheduledPosts(object state)
        //{



        //    var userId = Convert.ToInt64(httpContext.Session["id"].ToString());
        //    DateTime currentTime = DateTime.Now;
        //    List<PostViewModel> scheduledPosts = _repository.GetScheduledPost(currentTime);

        //    foreach (PostViewModel post in scheduledPosts)
        //    {

        //        if (post.UserID == userId)
        //        {
        //            SendNotificationToCurrentUser("Your post has been published!", httpContext);
        //        }
        //        _repository.PublishPost(post.ID);
        //    }
        //}



        //public void SendNotificationToCurrentUser(string message, HttpContextBase httpContext)
        //{

        //    var toastOptions = new
        //    {
        //        closeButton = true,
        //        progressBar = true,
        //        positionClass = "toast-bottom-right"
        //    };


        //    var script = $@"toastr.success('{message}', '', {JsonConvert.SerializeObject(toastOptions)});";
        //    var scriptTag = new TagBuilder("script");
        //    scriptTag.InnerHtml = script;
        //    httpContext.Response.Write(scriptTag.ToString());
        //}



    }
}