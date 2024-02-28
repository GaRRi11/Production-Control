using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace Production_Controll
{
    public class Modification
    {
        public long id {  get; set; }
        public long productId { get; set; }

        public Operation operation { get; set; }

        public int quantity { get; set; }

        public DateTime date { get; set; }

        public long SourceCityId { get; set; }
        public long TargetCityId { get; set; }



        public Modification(long productId, Operation operation,long sourceCityId,long targetCityId, int quantity, DateTime date)
        {
            this.productId = productId;
            this.operation = operation;
            this.SourceCityId = sourceCityId;
            this.TargetCityId = targetCityId;
            this.quantity = quantity;
            this.date = date;
        }
        public Modification(long productId, Operation operation, int quantity, DateTime date)
        {
            this.productId = productId;
            this.operation = operation;
            this.SourceCityId = 0;
            this.TargetCityId = 0;
            this.quantity = quantity;
            this.date = date;
        }

        public Modification(long id, long productId, Operation operation,long sourceCityId,long targetCityId, int quantity, DateTime date)
        {
            this.id = id;
            this.productId = productId;
            this.operation = operation;
            this.SourceCityId = sourceCityId;
            this.TargetCityId = targetCityId;
            this.quantity = quantity;
            this.date = date;
        }

        public enum Operation{
            CREATE,
            Addition,
            Substraction,
            DELETE,
            TRANSFER
        }
    }
}
