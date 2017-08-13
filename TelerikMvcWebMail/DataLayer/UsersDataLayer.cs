﻿using System;
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
                  //  Selected = s.MailBoxSequence == 1 ? true : false
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
                Model = Entity.MailBoxFolders.Where(x => x.MailBoxId == MailBoxId && x.IsActive==true).Select(s => new MailBoxFolderModel
                {
                    MailBoxFolderId = s.MailBoxFolderId,
                    MailBoxFolderName = s.MailBoxFolderName,
                    MailBoxId = s.MailBoxId,
                    Owner=s.MailBox.UserId== _UserId?"YES":"NO",
                    Sequence =s.Sequence,
                }).OrderBy(x => x.Sequence).ToList();

            }
            return Model;
        }

        public MailBoxFolderModel GetFolderDeatiles(long FolderId)
        {
           
            MailBoxFolderModel Model = new MailBoxFolderModel();
            using (var Entity = new WebMailEntities())
            {
                Model = Entity.MailBoxFolders.Select(s => new MailBoxFolderModel
                {
                    MailBoxFolderId = s.MailBoxFolderId,
                    MailBoxFolderName = s.MailBoxFolderName,
                    MailBoxId = s.MailBoxId,                  
                    Sequence = s.Sequence,
                }).Where(x => x.MailBoxFolderId == FolderId).FirstOrDefault();

            }
            return Model;
        }
        public List<MailBoxFolderModel> AllUserFolderList(string UserId)
        {
            long _UserId = Convert.ToInt32(UserId);
            List<MailBoxFolderModel> Model = new List<MailBoxFolderModel>();
            using (var Entity = new WebMailEntities())
            {
                Model = Entity.MailBoxFolders.Where(a=>a.MailBox.UserId== _UserId && a.IsActive==true).Select(s => new MailBoxFolderModel
                {
                    MailBoxFolderId = s.MailBoxFolderId,
                    MailBoxFolderName = s.MailBoxFolderName,
                    MailBoxId = s.MailBoxId,
                    Owner = s.MailBox.UserId == _UserId ? "YES" : "NO",
                    Sequence = s.Sequence,
                    MailBoxName=s.MailBox.MailBoxName
                }).OrderBy(x => x.MailBoxId).ThenBy(x=>x.Sequence).ToList();

            }
            return Model;
        }
        public List<SelectListItem> MailBoxList(int UserId)
        {        
            List<SelectListItem> _MainBoxList = new List<SelectListItem>();
            
            using (var Entity = new WebMailEntities())
            {
                _MainBoxList = Entity.MailBoxes.Where(x => x.UserId == UserId).Select(s => new SelectListItem
                {
                    Text = s.MailBoxName,
                    Value = s.MailBoxId.ToString(),                    
                }).ToList();

            }
            return _MainBoxList;
        }
        public bool AddEditFolder(MailBoxFolderModel Model)
        {
            try
            {
                MailBoxFolder _MailBoxFolders = new MailBoxFolder();
                //For Add
                if (string.IsNullOrEmpty(Model.MailBoxFolderId.ToString()) || Model.MailBoxFolderId==0)
                {
                    using (var Entity = new WebMailEntities())
                    {
                        _MailBoxFolders.MailBoxFolderName = Model.MailBoxFolderName;
                        _MailBoxFolders.MailBoxId = Model.MailBoxId;
                        _MailBoxFolders.Sequence = 0;
                        _MailBoxFolders.IsActive = true;
                        Entity.MailBoxFolders.Add(_MailBoxFolders);
                        Entity.SaveChanges();
                    }
                }
                else
                {
                    using (var Entity = new WebMailEntities())
                    {
                        MailBoxFolder _MailBoxUpdate = Entity.MailBoxFolders.Where(s => s.MailBoxFolderId == Model.MailBoxFolderId).FirstOrDefault();
                        if(_MailBoxUpdate !=null)
                        {
                            _MailBoxUpdate.MailBoxFolderName= Model.MailBoxFolderName;
                            _MailBoxUpdate.MailBoxId = Model.MailBoxId;
                            Entity.SaveChanges();
                        }
                    }
                }
                using (var entity = new WebMailEntities())
                {
                    var SequenceArry = Model.Sequenseids.Split(',');
                    for (int i = 0; i < SequenceArry.Count(); i++)
                    {
                        MailBoxFolder _UpdateSequence = new MailBoxFolder();
                        if (SequenceArry[i].ToString() == "NEWREC")
                        {                           
                            _UpdateSequence = entity.MailBoxFolders.SingleOrDefault(b => b.MailBoxFolderId == _MailBoxFolders.MailBoxFolderId);
                        }
                        else
                        {
                            Int32 id = Convert.ToInt32(SequenceArry[i].ToString());
                            _UpdateSequence = entity.MailBoxFolders.SingleOrDefault(b => b.MailBoxFolderId == id);
                        }
                        _UpdateSequence.Sequence = i + 1;
                        entity.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool FunctionDeleteFolder(string  Id)
        {
            try
            {
                long _Id = Convert.ToInt32(Id);
                MailBoxFolder _MailBoxFolders = new MailBoxFolder();
               
                    using (var Entity = new WebMailEntities())
                    {
                        MailBoxFolder _MailBoxUpdate = Entity.MailBoxFolders.Where(s => s.MailBoxFolderId == _Id).FirstOrDefault();
                        if (_MailBoxUpdate != null)
                        {
                            _MailBoxUpdate.IsActive =false;                            
                            Entity.SaveChanges();
                        }
                    }
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}