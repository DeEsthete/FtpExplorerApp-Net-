using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace FtpExplorerApp
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private Window window;
        private FtpWebRequest ftpWebRequest;
        private List<FileEntity> files;
        private string address;
        private string user;
        private string password;

        public MainPage(Window window, FtpWebRequest ftpWebRequest, string address, string user, string password)
        {
            InitializeComponent();
            this.window = window;
            this.ftpWebRequest = ftpWebRequest;
            this.address = address;
            this.user = user;
            this.password = password;
            GetListDirecotry(ftpWebRequest);
        }

        private void ExplorerListBoxMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FileEntityUserControll fileUserControll = (FileEntityUserControll)explorerListBox.SelectedItem;
            FileEntity file = fileUserControll.file;
            FtpWebRequest tempWebRequest = (FtpWebRequest)WebRequest.Create(address + @"\" + file.FileName);

            if (file.FileType == FileType.Directory)
            {
                tempWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                tempWebRequest.Credentials = new NetworkCredential(user, password);
                GetListDirecotry(tempWebRequest);
            }
            else
            {
                tempWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                tempWebRequest.Credentials = new NetworkCredential(user, password);

                FtpWebResponse webResponse = (FtpWebResponse)tempWebRequest.GetResponse();
                using (var stream = webResponse.GetResponseStream())
                {
                    byte[] buffer = new byte[10 * 1024 * 1024];
                    int bytes = stream.Read(buffer, 0, buffer.Length);

                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\files");
                    File.WriteAllBytes(Directory.GetCurrentDirectory() + @"\files"+ @"\" + file.FileName, buffer);
                }
            }
        }

        private async void GetListDirecotry(FtpWebRequest webRequest)
        {
            string direcotry = await DownloadListDirectory(webRequest);
            string[] entities = direcotry.Split(new char[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
            files = new List<FileEntity>();
            for (int i = 0; i < entities.Length; i++)
            {
                FileEntity temp = new FileEntity();
                string[] tmp = entities[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                //temp.FileName = 
                if (entities[i][0] == 'd')
                {
                    temp.FileType = FileType.Directory;
                }
                else
                {
                    temp.FileType = FileType.File;
                }
            }
            //files нужно заполнить //распарсить entity

            //for (int i = 0; i < files.Count; i++)
            //{
            //    FileEntityUserControll temp = new FileEntityUserControll(files[i]);
            //    explorerListBox.Items.Add(temp);
            //}
        }
        private Task<string> DownloadListDirectory(FtpWebRequest webRequest)
        {
            return Task.Run(() => 
            {
                webRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse webResponse = (FtpWebResponse)webRequest.GetResponse();

                using (var stream = webResponse.GetResponseStream())
                {
                    byte[] buffer = new byte[1024];
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    string data = Encoding.Default.GetString(buffer);
                    return data;
                }
            });
        }

        private void UploadButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();

            if (d.ShowDialog() == true)
            {
                //FtpWebRequest temp = (FtpWebRequest)WebRequest.Create(address);
                //temp.Method = WebRequestMethods.Ftp.UploadFile;
                //ftpWebRequest.Credentials = new NetworkCredential(user, password);
                //d.FileName();
                byte[] data = File.ReadAllBytes(d.FileName);
            }
        }
    }
}
