﻿using System;
using System.Collections.Generic;
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

namespace FtpExplorerApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Content = new SignInPage(this);
        }
    }
}
/*
 * SignIn =                                                             true
 * Загрузка файлов в ListBox(нужно запарсить)                           false
 * Переход по дерикториям и скачивание файла =                          check
 *      Сделать возможность вернуться назад
 * Загрузка файла =                                                     false
 */
