using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NotifyIcon = System.Windows.Forms.NotifyIcon;
using System.Diagnostics;

namespace SharpDesktopInspector
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private NotifyIcon? notifyIcon = null;
        private FileSystemWatcher? fileSystemWatcher = null;

        private int Target { get; set; }

        private string? TargetType { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Stream? icon = GetResourceStream(new Uri("SharpDesktopInspectorNotify.ico", UriKind.Relative)).Stream;

            System.Windows.Forms.ContextMenuStrip contextMenu = new System.Windows.Forms.ContextMenuStrip();
            contextMenu.Items.Add("設定", null, ShowMainWindow);
            contextMenu.Items.Add("終了", null, ExitClick);

            notifyIcon = new NotifyIcon()
            {
                Visible = true,
                Icon = new System.Drawing.Icon(icon),
                Text = VersionInfo.AppName,
                ContextMenuStrip = contextMenu
            };

            Settings? settings = SettingsConfiguration.ConfigureApplicationSettings();
            if (settings != null)
            {
                this.Target = settings.Target;
                this.TargetType = settings.TargetType;
            }
            else
            {
                this.Target = 0;
                this.TargetType = string.Empty;
            }

            fileSystemWatcher = new FileSystemWatcher()
            {
                Path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                IncludeSubdirectories = true,
                Filter = this.Target == 1 && this.TargetType != null ? this.TargetType : "",
                NotifyFilter = NotifyFilters.Attributes
                                | NotifyFilters.CreationTime
                                | NotifyFilters.DirectoryName
                                | NotifyFilters.FileName
                                | NotifyFilters.LastAccess
                                | NotifyFilters.LastWrite
                                | NotifyFilters.Security
                                | NotifyFilters.Size
            };

            fileSystemWatcher.Created += new FileSystemEventHandler(FileCreated);
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void ShowMainWindow(object? sender, EventArgs e)
        {
            MainWindow mainWindow = new()
            {
                Target = this.Target,
                TargetType = this.TargetType
            };

            if (mainWindow.ShowDialog() == true)
            {
                this.Target = mainWindow.Target;
                this.TargetType = mainWindow.TargetType;

                if (fileSystemWatcher != null)
                    fileSystemWatcher.Filter = this.Target == 1 && this.TargetType != null ? this.TargetType : "";
            }
        }

        private void ExitClick(object? sender, EventArgs e)
        {
            Settings settings = new()
            {
                Target = this.Target,
                TargetType = this.TargetType
            };
            try
            {
                SettingsConfiguration.SaveSettings(settings);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }

            if (notifyIcon != null) { notifyIcon.Dispose(); }

            Shutdown();
        }

        private void FileCreated(object? sender, FileSystemEventArgs e)
        {
            string path = e.FullPath;

            if (notifyIcon != null)
            {
                notifyIcon.BalloonTipTitle = "ファイル作成通知";
                notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                notifyIcon.BalloonTipText = $"デスクトップにファイルが作成されました\n{Path.GetFileName(path)}";
                notifyIcon.ShowBalloonTip(5000);
            }
        }
    }
}
