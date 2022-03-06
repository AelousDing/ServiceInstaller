using MahApps.Metro.Controls;
using Microsoft.Win32;
using ServiceInstaller.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServiceInstaller
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// 注册服务D:\app\srvany\instsrv.exe  nginx  D:\app\srvany\srvany.exe
    /// 卸载服务D:\app\srvany\instsrv.exe nginx  remove
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        const string ToolPath = "Tools";
        const string InstsrvExe = "instsrv.exe";
        const string SrvanyExe = "srvany.exe";
        const string RemoveArg = "remove";

        const string Parameters = "Parameters";
        const string ApplicationValue = "Application";
        const string AppDirectory = "AppDirectory";
        const string AppParameters = "AppParameters";
        //SYSTEM\CurrentControlSet\Services
        const string RegistryPath = "SYSTEM\\CurrentControlSet\\Services";

        const string CmdExe = "cmd.exe";
        public MainWindow()
        {
            InitializeComponent();
            installToolPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ToolPath, InstsrvExe);
            srcToolPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ToolPath, SrvanyExe);
        }
        string installToolPath;
        string srcToolPath;
        string exePath = null;
        string exeServiceName = null;

        string filePath = null;
        string fileServiceName = null;
        //sc create svnserve binpath= "c:\svnserve\svnserve.exe --service --root c:\repos" displayname= "Subversion" depend= tcpip start= auto
        string scCreateCmd;
        string scDeleteCmd;
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "可执行文件 (*.exe,*.bat)|*.exe;*.bat"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                exePath = openFileDialog.FileName;
                exeServiceName = exePath.GetFileName();
                tbExePath.Text = exePath;
                tbExeServiceName.Text = exeServiceName;
            }
        }

        private void btnInstall_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(exePath))
            {
                MessageBox.Show("请选择文件");
                return;
            }
            ProcessExtension.Start(
                installToolPath,
                $"{tbExeServiceName.Text} {srcToolPath}",
                5000,
                out string output);
            tbExeOutput.AppendText(output);
            SetRegedit();
        }

        private void SetRegedit()
        {
            var key = Registry.LocalMachine;
            if (key.IsRegistryExist(RegistryPath, tbExeServiceName.Text))
            {
                if (!key.IsRegistryExist($"{RegistryPath}\\{tbExeServiceName.Text}", Parameters))
                {
                    key.CreateRegistryKey($"{RegistryPath}\\{tbExeServiceName.Text}\\{Parameters}");
                }
                if (!key.IsRegistryValueExist($"{RegistryPath}\\{tbExeServiceName.Text}\\{Parameters}", ApplicationValue))
                {
                    key.CreateRegistryValue($"{RegistryPath}\\{tbExeServiceName.Text}\\{Parameters}", ApplicationValue, exePath);
                    key.CreateRegistryValue($"{RegistryPath}\\{tbExeServiceName.Text}\\{Parameters}", AppDirectory, exePath.GetDirectoryName());
                    key.CreateRegistryValue($"{RegistryPath}\\{tbExeServiceName.Text}\\{Parameters}", AppParameters, "");
                }
            }
        }

        private void btnUnInstall_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(exePath))
            {
                MessageBox.Show("请选择文件");
                return;
            }
            ProcessExtension.Start(
                installToolPath,
                $"{tbExeServiceName.Text} {RemoveArg}",
                5000,
                out string output);
            tbExeOutput.AppendText(output);
        }

        private void btnFileSelect_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = ""
            };
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                fileServiceName = filePath.GetFileName();
                tbFilePath.Text = filePath;
                tbFileServiceName.Text = fileServiceName;
            }
        }

        private void btnFileInstall_Click(object sender, RoutedEventArgs e)
        {
            string args = $"sc create {fileServiceName} binpath={filePath} displayname={fileServiceName} start= auto";
            ProcessExtension.Cmd(CmdExe, args, 5000, out string output);
            tbFileOutput.Text = output;
        }

        private void btnFileUnInstall_Click(object sender, RoutedEventArgs e)
        {
            string args = $"sc delete {fileServiceName}";
            ProcessExtension.Cmd(CmdExe, args, 5000, out string output);
            tbFileOutput.Text = output;
        }
    }
}
