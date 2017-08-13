using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TelerikMvcWebMail.Models;

namespace TelerikMvcWebMail.DataLayer
{
    public class UsersDataLayer
    {
        public User ValidateUser(string UserName,string PassWord)
        {
            User _User = new User();
            string Result = string.Empty;
            using (var Entity = new WebMailEntities())
            {
                _User = Entity.Users.Where(s => s.Email.ToUpper() == UserName.ToUpper() && s.Password.ToUpper() == PassWord.ToUpper()).FirstOrDefault();
                
            }
                return _User;
        }
    }

    public class CommonFunctions
    {
        public List<SelectListItem> MailBoxList(int UserId,string request)
        {
            List<MailBoxModel> _Model =new  List<MailBoxModel>();
            List<MailBoxModel> AssinedMailBox = new List<MailBoxModel>();
            List<SelectListItem> _MainBoxList = new List<SelectListItem>();
            string Result = string.Empty;
            using (var Entity = new WebMailEntities())
            {
                _Model = Entity.MailBoxes.Where(x=>x.UserId==UserId).Select(s => new MailBoxModel
                {
                    MailBoxId = s.MailBoxId,
                    MailBoxName = s.MailBoxName,
                    MailBoxSequence = s.MailBoxSequence ,
                    UserId = s.UserId,
                    Owener="YES"
                }).OrderBy(x=>x.MailBoxSequence).ToList();

            }
            using (var Entity = new WebMailEntities())
            {
                AssinedMailBox = Entity.MailBoxAccesses.Where(x=>x.UserId== UserId).Select(s => new MailBoxModel
                {
                    MailBoxId = s.MailBoxId,
                    MailBoxName = s.MailBox.MailBoxName +" : "+s.MailBox.User.FullName,
                    MailBoxSequence = s.MailBox.MailBoxSequence,
                    UserId = s.UserId,
                    Owener = "NO"
                }).OrderBy(x => x.MailBoxSequence).ToList();
            }
            if (AssinedMailBox.Count() > 0)
            {
             _Model.AddRange(AssinedMailBox);
            }
            if (request=="DRP")
            {
                
                _MainBoxList = _Model.Select(s => new SelectListItem
                {
                    Text = s.MailBoxName,
                    Value = s.MailBoxId.ToString(),
                    Selected = s.MailBoxSequence == 1 ? true : false
                }).ToList();
          };
            
            return _MainBoxList;
        }
        public List<MailBoxFolderModel> MailBoxFolderList(long MailBoxId,string UserId)
        {
            long _UserId = Convert.ToInt32(UserId);
            List<MailBoxFolderModel> Model = new List<MailBoxFolderModel>();
            using (var Entity = new WebMailEntities())
            {
                Model = Entity.MailBoxFolders.Select(s => new MailBoxFolderModel
                {
                    MailBoxFolderId = s.MailBoxFolderId,
                    MailBoxFolderName = s.MailBoxFolderName,
                    MailBoxId = s.MailBoxId,
                    Owner=s.MailBox.UserId== _UserId?"YES":"NO",
                    Sequence =s.Sequence,
                }).Where(x=>x.MailBoxId== MailBoxId).OrderBy(x => x.Sequence).ToList();

            }
            return Model;
        }
    }
}