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
        private string currentAddress;

        public MainPage(Window window, FtpWebRequest ftpWebRequest, string address, string user, string password)
        {
            InitializeComponent();
            this.window = window;
            this.ftpWebRequest = ftpWebRequest;
            this.address = address;
            this.user = user;
            this.password = password;
            currentAddress = address;
            GetListDirecotry(ftpWebRequest);
        }

        private async void ExplorerListBoxMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FileEntityUserControll fileUserControll = (FileEntityUserControll)explorerListBox.SelectedItem;
            FileEntity file = fileUserControll.file;

            if (file.FileType == FileType.Directory)
            {
                FtpWebRequest tempWebRequest = (FtpWebRequest)WebRequest.Create(address + @"\" + file.FileName);
                tempWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                tempWebRequest.Credentials = new NetworkCredential(user, password);
                GetListDirecotry(tempWebRequest);
                currentAddress = address + @"\" + file.FileName;
            }
            else
            {
                SaveFileDialog fileDialog = new SaveFileDialog();
                if (fileDialog.ShowDialog() == true)
                {
                    string path = fileDialog.FileName;
                    await DownloadFile(path, file.FileName);
                }
            }
        }

        private Task DownloadFile(string path,string fileName)
        {
            return Task.Run(() =>
            {
                FtpWebRequest webRequest = (FtpWebRequest)WebRequest.Create(address + "/.txt");//имя файла
                webRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                webRequest.Credentials = new NetworkCredential(user, password);

                FtpWebResponse webResponse = (FtpWebResponse)webRequest.GetResponse();
                using (var stream = webResponse.GetResponseStream())
                {

                    byte[] buffer = new byte[10 * 1024 * 1024];
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    File.WriteAllBytes(path, buffer);
                }
            });
        }

        private async void GetListDirecotry(FtpWebRequest webRequest)
        {
            explorerListBox.Items.Clear();
            explorerListBox.Items.Refresh();
            string direcotry = await DownloadListDirectory(webRequest);
            string[] entities = direcotry.Split(new char[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
            files = new List<FileEntity>();
            for (int i = 0; i < entities.Length; i++)
            {
                FileEntity temp = new FileEntity();
                var str = entities[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Last();
                temp.FileName = str.Substring(0, str.Length - 1);
                if (entities[i][0] == 'd')
                {
                    temp.FileType = FileType.Directory;
                }
                else
                {
                    temp.FileType = FileType.File;
                }
                FileEntityUserControll tempControll = new FileEntityUserControll(temp);
                explorerListBox.Items.Add(tempControll);
            }
        }
        private Task<string> DownloadListDirectory(FtpWebRequest webRequest)
        {
            return Task.Run(() => 
            {
                webRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                FtpWebResponse webResponse = (FtpWebResponse)webRequest.GetResponse();

                long byteLength;
                using (var stream = webResponse.GetResponseStream())
                {
                    byteLength = webRequest.ContentLength;
                    byte[] buffer = new byte[10 * 1024];
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    string data = Encoding.Default.GetString(buffer);
                    //data = data.Substring((int)byteLength);
                    return data;
                }
            });
        }

        private void UploadButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();

            if (d.ShowDialog() == true)
            {
                FileInfo fileInfo = new FileInfo(d.FileName);


                byte[] data = File.ReadAllBytes(d.FileName);

                FtpWebRequest temp = (FtpWebRequest)WebRequest.Create(new Uri($@"{currentAddress}/{fileInfo.Name}"));
                temp.Method = WebRequestMethods.Ftp.UploadFile;
                temp.Credentials = new NetworkCredential(user, password);
                temp.ContentLength = data.Length;
                FtpWebResponse response = (FtpWebResponse)temp.GetResponse();

                using (var stream = temp.GetRequestStream())
                {
                    stream.Write(data,0,data.Length);
                }
            }
        }
    }
}
