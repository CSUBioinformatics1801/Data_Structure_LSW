using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exp4
{
    public class Info
    {
        private string id;
        private int sex;
        private int age;
        private DateTime date;
        private int charge; 
        public Info()
        {

        }    
        public Info(string Id, int Sex, int Age, int Year, int Month, int Day, int Hour, int Minute, int Charge)
        {
            id = Id;
            sex = Sex;
            age = Age;
            date = new DateTime(Year, Month, Day, Hour, Minute, 0);
            charge = Charge;
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public int Sex
        {
            get { return sex; }
            set { sex = value; }
        }
        public int Age
        {
            get { return age; }
            set { age = value; }
        }
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
        public int Charge
        {
            get { return charge; }
            set { charge = value; }
        }
    }
}
