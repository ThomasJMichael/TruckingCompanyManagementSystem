using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.GUI.Models;

namespace TCMS.GUI.ViewModels
{
    class ShipmentsViewModel : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;
        public TimeOnly ShipmentTracking
        {
            get { return _pageModel.ShipmentDelivery; }
            set { _pageModel.ShipmentDelivery = value; OnPropertyChanged(); }
        }

        public ShipmentsViewModel()
        {
            _pageModel = new PageModel();
            TimeOnly time = TimeOnly.FromDateTime(DateTime.Now);
            ShipmentTracking = time;
        }
    }
}
