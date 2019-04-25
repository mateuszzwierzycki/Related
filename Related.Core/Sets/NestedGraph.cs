using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Related.Graphs.Example {
    /// <summary>
    /// This is an example of library user/client-visible comment. Use it by default.
    /// </summary>
    [Serializable]
    public class CodeFormattingExample {

        //This is a documentation example visible only to the people with access to the source. 
        //DON'T DESCRIBE THE METHODS THAT WAY. USE THE <summary>.

        //For the sake of VB <> C# communication:
        //void is to be named a Method.
        //any method returning a value is called a Function.

        //==========================PROPERTIES and FIELDS
        //==========================We use these splits only 

        /// <summary>
        /// DON'T document self-explanatory properies like Id, Value, CustomerNumber etc.
        /// </summary>
        public int Id { get; set; } = 0;
        public double Value { get; set; } = 0;
        public double SomeOtherValue { get; private set; }
        public List<CodeFormattingExample> Parents { get; set; } = new List<CodeFormattingExample>();
        public List<CodeFormattingExample> Children { get; set; } = new List<CodeFormattingExample>();

        //==========================CONSTRUCTORS and DEEP/SHALLOW CLONE FUNCTIONS
        //==========================Various ways of getting an instance of a class.

        public CodeFormattingExample() { }

        //Use UpperCamelCase/PascalCase to name all the input arguments.
        public CodeFormattingExample(int Id, 
            double Value, 
            double OtherValue = 100) {
            this.Id = this.Id;
            this.Value = Value;
            this.SomeOtherValue = OtherValue; 

            //Use lowerCamelCase for variables in the method scopes. 
            double theValueToCalculate = 1 / Value;
            double result = theValueToCalculate * Value;
            }

        //==========================METHODS and FUNCTIONS

        // Example of a public one line function. 
        public bool IsRoot() => Parents.Count == 0;

        // Example of a method.
        public void AddChild(CodeFormattingExample Child) {
            if (Child != this) {
                this.Children.Add(Child);
                Child.Parents.Add(this);
                }
            }

        // Example of a function. Note the lack of "else" statement when not necessary.
        public bool AddParent(CodeFormattingExample Parent) {
            if (Parent != this) {
                Parents.Add(Parent);
                Parent.Children.Add(this);
                return true;
                }
            return false;
            }

        //==========================OVERRIDEN METHODS and FUNCTIONS
        //==========================Distingush only when the file becomes unreadable.

        //public override bool AddToValue(double Value) {
        //    this.Value += Value;
        //    }

        //==========================OBJECT DEFAULT METHODS and FUNCTIONS
        //==========================Basically ToString() function only.

        public override string ToString() {
            return "CodeFormattingExample";
            }

        }
    }
