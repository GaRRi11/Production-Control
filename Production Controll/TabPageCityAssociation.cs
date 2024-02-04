using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_Controll
{
    public class TabPageCityAssociation
    {
        public TabPage TabPage { get; }
        public long cityId { get; }

        public TabPageCityAssociation(TabPage tabPage, long cityId)
        {
            this.TabPage = tabPage;
            this.cityId = cityId;
        }
    }

}
