using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Doctors.Models;

namespace DoctorsDashboard.ViewModels
{
    class DoctorViewModel : INotifyPropertyChanged
    {

        private Doctor _doctor;

        public string Name { get; set; }



        public DoctorViewModel(Doctor doctor)
        {
            _doctor = doctor;
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
