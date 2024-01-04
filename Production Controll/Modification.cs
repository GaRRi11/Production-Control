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
        private long id {  get; set; }

        //@manytoone
        private long productId { get; set; }

        private Operation operation { get; set; }

        private int quantity { get; set; }

        private DateTime date { get; set; }


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
