﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreNoteWPF.Core.Model;

namespace MoreNoteWPF.Core.Model
{
    public class FileListViewNode
    {
        public FileType FileType { get;set;}
        public string ImagePath { get;set;}
        public string Name { get;set;}
        public string FullName { get; set; }
        public string CreationTime{get;set;}
        public string LastWriteTime { get;set;}
        public string Extension { get;set;}
        public string Length { get;set;}


        public FileListViewNode()
        {

        }
     
        public FileListViewNode(FileInfo fileInfo)
        {
            this.ImagePath="/Resources/UI/Default/File.png";
            this.FileType= FileType.File;
            this.Name=fileInfo.Name;
            this.FullName = fileInfo.FullName;
             this.CreationTime = fileInfo.CreationTime.ToString("G");
            this.LastWriteTime = fileInfo.LastWriteTime.ToString("G");
            this.Extension = fileInfo.Extension;
            this.Length = fileInfo.Length+"字节";
        }
        public FileListViewNode(DirectoryInfo directoryInfo)
        {
            this.ImagePath = "/Resources/UI/Default/FolderOrange.png";
            this.FileType = FileType.Directory;
            
            this.Name = directoryInfo.Name;
            this.FullName = directoryInfo.FullName;
            this.CreationTime = directoryInfo.CreationTime.ToString("G");
            this.LastWriteTime = directoryInfo.LastWriteTime.ToString("G");
            this.Extension = "文件夹";
            this.Length = "";
        }
    }
}
