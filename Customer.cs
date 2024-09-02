using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNGK
{
    class Customer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public String Phone { get; set; }
        public string Addr { get; set; }
        public int Age { get; set; }
        public string Type { get; set; }
        public int No { get; set; }
        public string Gender { get; set; }
        public string Unpaid { get; set; }
        public Customer(string Id, string Type, string Name,string Gender, string Phone, string Addr, string Unpaid) {
            this.Id = Id;
            this.Name = Name;
            this.Phone = Phone;
            this.Addr = Addr;
            this.Type = Type;
            this.Gender = Gender;
            this.Unpaid = Unpaid;

        }
    }
}
