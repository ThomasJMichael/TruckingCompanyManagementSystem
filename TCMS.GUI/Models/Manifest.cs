using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.GUI.Models
{
    public class Manifest
    {
        public int ManifestId { get; set; }
        public ObservableCollection<ManifestItem> Items { get; set; }
    }
}
