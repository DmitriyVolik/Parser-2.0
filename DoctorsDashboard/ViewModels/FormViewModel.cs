﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using Doctors.Models;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Management;

namespace DoctorsDashboard.ViewModels
{
    class FormViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Doctor> Doctors { get; set; }

        public ObservableCollection<Service> Services { get; set; }

        private Doctor _currentDoctor;

        private DataBase dataBase;

        private Process process;

        public string StartStopTextButton { get => process == null ? "Запуск" : "Стоп"; }

        private Service _currentService;

        public double IsServicesVisible
        {
            get =>CurrentDoctor != null &&CurrentDoctorServices.Count > 0 ? 100 : 0;
        }

        public Service CurrentService
        {
            get { return _currentService; }
            set
            {
                _currentService = value;

                Doctors.Clear();
                dataBase.FindAllByService(value).ForEach(i => Doctors.Add(i));
                OnPropertyChanged("CurrentService");
            }
        }

        public Doctor CurrentDoctor
        {
            get { return _currentDoctor; }
            set
            {
                _currentDoctor = value;
                OnPropertyChanged("CurrentDoctor");
                OnPropertyChanged("CurrentDoctorServices");
                OnPropertyChanged("IsServicesVisible");
                OnPropertyChanged("DoctorImage");
            }
        }

        public ImageSource DoctorImage
        {
            get
            {
                string selectedFileName;
                if (CurrentDoctor.LocalImageFile == null)
                {
                    selectedFileName = @"E:\\ДЗ Шаг\\С#\\WPF\\Parser 2.0\\Parser 2.0\\bin\\Debug\\netcoreapp3.1\\Pictures\\Default.png";
                } else
                {
                    selectedFileName = @"E:\\ДЗ Шаг\\С#\\WPF\\Parser 2.0\\Parser 2.0\\bin\\Debug\\netcoreapp3.1\\Pictures" + "\\"+ CurrentDoctor.LocalImageFile;
                }
                
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();

                return bitmap;

                //return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap, IntPtr.Zero, Int32Rect.Empty,
                //    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
        }




        public ObservableCollection<Service> CurrentDoctorServices
        {
            get => new ObservableCollection<Service>(dataBase.Find(_currentDoctor));
        }


        public RelayCommand ResetButton
        {
            get
            {
                return new RelayCommand(
                        obj =>
                        {

                            _currentService = null;

                            Doctors.Clear();

                            dataBase.FindAll().ForEach(i => Doctors.Add(i));

                            OnPropertyChanged("CurrentService");
                        }
                    );
            }
        }

        public RelayCommand StartStopButton
        {
            get
            {
                return new RelayCommand(
                        obj =>
                        {


                            if (process == null)
                            {
                                process = new Process();
                                process.StartInfo.FileName = @"E:\ДЗ Шаг\С#\WPF\Parser 2.0\Parser 2.0\bin\Debug\netcoreapp3.1\Parser 2.0.exe";
                                process.StartInfo.CreateNoWindow = true;
                                process.Start(); 
                            }
                            else
                            {
                                foreach (var item in GetChildProcesses(process))
                                {
                                    item.Kill();
                                }
                                process.Kill();

                                process = null;
                            }

                            OnPropertyChanged("StartStopTextButton");
                        }
                    );
            }
        }

        public FormViewModel()
        {
            dataBase = new DataBase(@"Data Source=.\SQLEXPRESS;Initial Catalog=Doctors;Integrated Security=True");
            Doctors = new ObservableCollection<Doctor>();
            dataBase.FindAll().ForEach(i => Doctors.Add(i));

            Services = new ObservableCollection<Service>();
            dataBase.FindAllServices().ForEach(i => Services.Add(i));
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public static IEnumerable<Process> GetChildProcesses(Process process)
        {
            List<Process> children = new List<Process>();
            ManagementObjectSearcher mos = new ManagementObjectSearcher(String.Format("Select * From Win32_Process Where ParentProcessID={0}", process.Id));
            foreach (ManagementObject mo in mos.Get())
            {
                children.Add(Process.GetProcessById(Convert.ToInt32(mo["ProcessID"])));
            }
            return children;
        }
    }
}
