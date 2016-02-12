namespace AsiaticIndustries.Core.Infrastructure.Utility
{
    public class EmailUtility
    {

        //public static bool SendEmail<T>(int notificationTypeID, long? loginUserID, T tokens, string emailID, string path,
        //                                string subject) where T : class, new()
        //{
        //    int templateID = Common.GetTempateNotificationMapping()[notificationTypeID];

        //    if (templateID != 0)
        //    {
        //        string emailBody = "";
        //        if (File.Exists(path))
        //        {
        //            emailBody = File.ReadAllText(path);
        //        }

        //        string emailSubject = subject;

        //        switch (notificationTypeID)
        //        {
        //            case (int) Common.NotificationType.SendEmailTemplate:
        //                {
        //                    emailBody = TokenReplace.ReplaceTokens(emailBody, tokens);
        //                    return Common.SendEmail(emailSubject, "", emailID, emailBody);
        //                }

        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //    return false;
        //}

    }
}