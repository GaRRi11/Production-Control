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

        //@manytoone
        public long productId { get; set; }

        public Operation operation { get; set; }

        public int quantity { get; set; }

        public DateTime date { get; set; }


        public static long lastId = 0;
        private long GenerateId()
        {
            return ++lastId;
        }

        public Modification(long productId, Operation operation, int quantity, DateTime date)
        {
            this.id = GenerateId();
            this.productId = productId;
            this.operation = operation;
            this.quantity = quantity;
            this.date = date;
        }

        public enum Operation{
            Add,
            Addition,
            Substraction
        }
    }
}
