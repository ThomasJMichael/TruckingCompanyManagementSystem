using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.GUI.Models;

namespace TCMS.GUI.ViewModels
{
    public class EquipmentViewModel : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;
        public bool Settings
        {
            get { return _pageModel.LocationStatus; }
            set { _pageModel.LocationStatus = value; OnPropertyChanged(); }
        }

        public EquipmentViewModel()
        {
            _pageModel = new PageModel();
            Settings = true;
        }
    }
}
