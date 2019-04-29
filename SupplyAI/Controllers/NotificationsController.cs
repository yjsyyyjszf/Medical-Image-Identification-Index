﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MI3.Models;

namespace MI3.Controllers
{
    public class NotificationsController : Controller
    {
        // GET: Notifications
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Review(string notificationId)
        {
            Database db = new Database();
            Notification notification = db.FindOne<Notification>(Notification.MongoCollectionName, doc => doc.Id == notificationId);
            Abstract attachment = db.FindOne<Abstract>("Abstracts", doc => doc.Id == notification.AttachmentIdString);

            Type typeOfNotification = Type.GetType(notification.NotificationType + ", " + Database.AppName); // target type
            Object dynamicNotificationObject = Activator.CreateInstance(typeOfNotification); // an instance of target type
            INotifiable notificationType = (INotifiable)dynamicNotificationObject;
            string title = notificationType.Title;

            NewAbstractViewModel viewModel = new NewAbstractViewModel
            {
                Title = title,
                UserName = notification.UserName,
                Url = attachment.Url,
                AttachmentId = attachment.Id,
                NotificationId = notification.Id,
                Rehost = attachment.Rehost,
                PublicAccess = attachment.PublicAccess,
                Content = attachment.Content,
                DatasetTitle = attachment.DatasetTitle,
                Authors = attachment.Authors,
                Source = attachment.Source,
                NumberOfStudies = attachment.NumberOfStudies,
                SameModalityAndManuf = attachment.SameModalityAndManuf,
                Modality = attachment.Modality,
                Manufacturer = attachment.Manufacturer,
                HasLabels = attachment.HasLabels,
                LabelType = attachment.LabelType,
                LabelFormat = attachment.LabelFormat,
                LabelsReviewed = attachment.LabelsReviewed,
                ClinicalIssues = attachment.ClinicalIssues,
                IsAnonymized = attachment.IsAnonymized,
                HowAnonymized = attachment.HowAnonymized,
                Country = attachment.Country,
                USRegion = attachment.USRegion,
                State = attachment.State,
                PixelDataShifted = attachment.PixelDataShifted,
                HowPixelDataShifted = attachment.HowPixelDataShifted,
                PixelDataSynthesized = attachment.PixelDataSynthesized,
                HowPixelDataSynthesized = attachment.HowPixelDataSynthesized,
                MultipleModalitiesAndManuf = attachment.MultipleModalitiesAndManuf,
                ModelsAndVersions = attachment.ModelsAndVersions,
                DateGenerated = attachment.DateGenerated
            };

            return View(viewModel);
        }
    }
}