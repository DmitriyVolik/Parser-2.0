using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using Doctors.Models;

namespace DoctorsDashboard.ViewModels
{
    class FormViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Doctor> Doctors { get; set; }

        public ObservableCollection<Service> Services { get; set; }

        private Doctor _currentDoctor;

        private DataBase dataBase;

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
    }
}
