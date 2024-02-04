using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_Controll
{
    public class PanelProductAssociation
    {
        public Panel Panel { get; set; }
        public long productId { get; set; }

        public PanelProductAssociation(Panel panel, long productId)
        {
            this.Panel = panel;
            this.productId = productId;
        }
    }

}
