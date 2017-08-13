using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TelerikMvcWebMail.Common
{
    public class Folders
    {
        public string text { get; set; }
        public string value { get; set; }
        public string number { get; set; }
        public string MailBox { get; set; }

        
    }
    public static class Common
    {
        public static List<Folders> FolderList( string MainBoxName=null)
        {
            string SelectedMailBox = MainBoxName;
            if (string.IsNullOrEmpty(MainBoxName))
            {
                 List< SelectListItem > MailBoxList = TelerikMvcWebMail.Common.Common.MailBoxList();
                SelectedMailBox = MailBoxList.Where(x => x.Selected == true).Select(s => s.Value).FirstOrDefault();
            }

            List<Folders> _FolderList = new List<Folders>()
            {
               
                new Folders() {
                  value = "MailBox1_Folder1",
                  text = "MailBox1_Folder1",
                  MailBox="MailBox1"
                },
                  new Folders() {
                  value = "MailBox1_Folder2",
                  text = "MailBox1_Folder2",
                  MailBox="MailBox1"
                },
                    new Folders() {
                  value = "MailBox1_Folder3",
                  text = "MailBox1_Folder3",
                  MailBox="MailBox1"
                },
              
                      new Folders() {
                  value = "MailBox2_Folder1",
                  text = "MailBox2_Folder1",
                  MailBox="MailBox2"
                },
                        new Folders() {
                  value = "MailBox2_Folder2",
                  text = "MailBox2_Folder2",
                  MailBox="MailBox2"
                },
              
                 new Folders() {
                  value = "MailBox3_Folder1",
                  text = "MailBox3_Folder1",
                  MailBox="MailBox3"
                },
                      new Folders() {
                  value = "MailBox3_Folder2",
                  text = "MailBox3_Folder2",
                  MailBox="MailBox3"
                },
                        new Folders() {
                  value = "MailBox3_Folder3",
                  text = "MailBox3_Folder3",
                  MailBox="MailBox3"
                },
            };

            if (!string.IsNullOrEmpty(SelectedMailBox))
            {
                List<Folders> SpecificMailBox = _FolderList.Where(s => s.MailBox == SelectedMailBox).ToList();
                return SpecificMailBox;
            }
            else {
                return _FolderList;
                    }
        }
        public static List<SelectListItem> MailBoxList()
        {
            List<SelectListItem> _MainBoxList = new List<SelectListItem>() {
              new SelectListItem() {
                  Text = "MailBox1",
                  Value = "MailBox1",
                  Selected=true
              },
              new SelectListItem() {
                  Text = "MailBox2",
                  Value = "MailBox2"
              },
              new SelectListItem() {
                  Text = "MailBox3",
                  Value = "MailBox3"
              }
          };
            return _MainBoxList;
        }


    }
}