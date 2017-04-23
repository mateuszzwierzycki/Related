using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Related.Sets {
    public class SetsUtils {

        static double JaccardCoefficient (HashSet<object>A , HashSet<object> B) {
            HashSet<object> Sum = new HashSet<object>(A);

            foreach (object item in B ) {
                Sum.Add(item); 
            }

            return (double)A.Count / (double)(Sum.Count - A.Count); 
        }
    }
}
