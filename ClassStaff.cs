using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNGK
{
     class ClassStaff
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int age { get; set; }
        public String Phone { get; set; }
        public string Addr { get; set; }
        public string gender { get; set;}

        public ClassStaff(string Id,string Name,int age,string gender,String Phone, string Addr)
        {
            this.Id = Id;
            this.Name = Name;
            this.age = age;
            this.gender = gender;
            this.Phone = Phone;
            this.Addr = Addr;

        }
    }
}
