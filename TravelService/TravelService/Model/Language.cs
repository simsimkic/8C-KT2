using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Serializer;

namespace TravelService.Model
{
    public class Language: ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Language() { }

        public Language(string name)
        {
            Name = name;
        }

        public string[] ToCSV()
        {

            string[] csvValues = { Id.ToString(), Name};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];

        }
    }
}
