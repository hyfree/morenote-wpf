﻿using MoreNoteWPF.Core.Model;
using MoreNoteWPF.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MediaDevices;

namespace MoreNoteWPF.CustomUserControl.Main
{
    /// <summary>
    /// DiskTreeControl.xaml 的交互逻辑
    /// </summary>
    public partial class DiskTreeControl : UserControl
    {
        public DiskTreeControl()
        {
            InitializeComponent();
        }

        //初始化管理器界面的显示（初始化左窗体的驱动器树形视图和右窗体的文件列表视图）
        private void InitDisplay()
        {
            myComputer.Items.Clear();
            TreeViewItem recentFilesNode = new TreeViewItem() { Header = "最近访问" };
            myComputer.Items.Add(recentFilesNode);
            recentFilesNode.Tag = "最近访问";

            // recentFilesNode.ImageIndex = IconsIndexes.RecentFiles;
            //recentFilesNode.SelectedImageIndex = IconsIndexes.RecentFiles;

            DriveInfo[] driveInfos = DriveInfo.GetDrives();

            foreach (DriveInfo info in driveInfos)
            {
                TreeViewItem driveNode = null;

                switch (info.DriveType)
                {
                    //固定磁盘
                    case DriveType.Fixed:

                        //显示的名称
                        driveNode = new TreeViewItem()
                        {
                            Header = "本地磁盘(" + info.Name.Split('\\')[0] + ")",
                            Tag = info.Name
                        };
                        myComputer.Items.Add(driveNode);

                        break;

                    //光驱
                    case DriveType.CDRom:

                        //显示的名称
                        //driveNode = diskTreeView.Nodes.Add("光驱(" + info.Name.Split('\\')[0] + ")");
                        driveNode = new TreeViewItem()
                        {
                            Header = "光驱(" + info.Name.Split('\\')[0] + ")",
                            Tag = info.Name
                        };
                        myComputer.Items.Add(driveNode);
                        //真正的路径
                        //driveNode.Tag = info.Name;

                        // driveNode.ImageIndex = IconsIndexes.CDRom;
                        // driveNode.SelectedImageIndex = IconsIndexes.CDRom;

                        break;

                    //可移动磁盘
                    case DriveType.Removable:

                        //显示的名称
                        // driveNode = diskTreeView.Nodes.Add("可移动磁盘(" + info.Name.Split('\\')[0] + ")");
                        driveNode = new TreeViewItem()
                        {
                            Header = "可移动磁盘(" + info.Name.Split('\\')[0] + ")",
                            Tag = info.Name
                        };
                        myComputer.Items.Add(driveNode);
                        //真正的路径
                        //driveNode.Tag = info.Name;

                        //driveNode.ImageIndex = IconsIndexes.RemovableDisk;
                        // driveNode.SelectedImageIndex = IconsIndexes.RemovableDisk;

                        break;
                    case DriveType.Network:

                        //显示的名称
                        driveNode = new TreeViewItem()
                        {
                            Header = "网络(" + info.Name.Split('\\')[0] + ")",
                            Tag = info.Name
                        };
                        
                        myComputer.Items.Add(driveNode);
                        
                        break;
                    case DriveType.Ram:

                        //显示的名称
                        driveNode = new TreeViewItem()
                        {
                            Header = "Ram(" + info.Name.Split('\\')[0] + ")",
                            Tag = info.Name
                        };
                        myComputer.Items.Add(driveNode);
                        break;
                    case DriveType.Unknown:

                        //显示的名称
                        driveNode = new TreeViewItem()
                        {
                            Header = "Unknown(" + info.Name.Split('\\')[0] + ")",
                            Tag = info.Name
                        };
                        myComputer.Items.Add(driveNode);
                        break;
                    case DriveType.NoRootDirectory:
                        break;
                    default:
                        driveNode = new TreeViewItem()
                        {
                            Header = "Unknown(" + info.Name.Split('\\')[0] + ")",
                            Tag = info.Name
                        };
                        myComputer.Items.Add(driveNode);
                        break;
                }
            }



            //加载每个磁盘下的子目录
            foreach (TreeViewItem node in myComputer.Items)
            {
                
                if (((string)node.Tag).Equals("最近访问")||DiskUtil.CheckBitLockerIsOn((string)node.Tag))
                {
                    continue;
                }
                LoadChildNodes(node);

            }

            //加载移动设备
            // var devices = MediaDevice.GetDevices();
            // foreach (var item in devices)
            // {
            //     
            //     TreeViewItem driveNode = new TreeViewItem()
            //     {
            //         Header = "移动终端(" + item.FriendlyName.Split('\\')[0] + ")",
            //         Tag = item.FriendlyName
            //
            //     };
            //     myComputer.Items.Add(driveNode);
            //     
            //
            // }

        }

        //加载子节点（加载当前目录下的子目录）
        private void LoadChildNodes(TreeViewItem node)
        {
            try
            {
                //清除空节点，然后才加载子节点
                node.Items.Clear();

                if (node.Tag==null||node.Tag.ToString() == "最近访问")
                {
                    return;
                }
                else
                {
                   
                    DirectoryInfo directoryInfo = new DirectoryInfo(node.Tag.ToString());
                    DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();

                    foreach (DirectoryInfo info in directoryInfos)
                    {
                        
                        //显示的名称
                        TreeViewItem childNode = new TreeViewItem();

                        childNode.Header = info.Name;
                     
                        //真正的路径
                        childNode.Tag = info.FullName;

                        //加载空节点，以实现“+”号
                        childNode.Items.Add("");
                        node.Items.Add(childNode);
                    }
                }

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            InitDisplay();

        }

        public event RoutedPropertyChangedEventHandler<object>  SelectedDiskChanged
        {
            add
            {
                base.AddHandler(TreeView.SelectedItemChangedEvent, value);
            }
            remove
            {
                RemoveHandler(TreeView.SelectedItemChangedEvent, value);
            }
        }
        
        private void DiskTreeView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DodiskTreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            
        }
        /// <summary>
        /// 当点击展开按钮 的时候
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyComputer_OnExpanded(object sender, RoutedEventArgs e)
        {
           TreeViewItem treeViewItem=e.Source as TreeViewItem;
            if (treeViewItem.Header.Equals("我的电脑"))
            {
                return;

            }
           LoadChildNodes(treeViewItem);
        }
    }
}
