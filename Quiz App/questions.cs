using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_App
{
    class ques_id
    {
        public int Ques_id { get; set; }

        public string q_title { get; set; }
        public string q_opA { get; set; }
        public string q_opB { get; set; }
        public string q_opC { get; set; }
        public string q_opD { get; set; }
        public string q_correctOpn { get; set; }
        public string q_correctDate { get; set; }
        public string ad_id_fk { get; set; }
        public string ex_id_fk { get; set; }

        
    }
}
