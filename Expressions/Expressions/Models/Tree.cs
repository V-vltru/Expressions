namespace Expressions.Models
{
    /// <summary>
    /// Main tree contains the expression.
    /// </summary>
    public class Tree
    {
        private string _data;
        private EssenceType _dataType;
        private double _leftOperand;
        private double _rightOperand;

        private string _stringLeft;
        private string _stringRight;

        /// <summary>
        /// Gets or sets the left subtree
        /// </summary>
        public Tree LeftLeave { get; set; }

        /// <summary>
        /// Gets or sets the right subtree
        /// </summary>
        public Tree RightLeave { get; set; }

        /// <summary>
        /// Default costructor
        /// </summary>
        public Tree() { }

        /// <summary>
        /// Constructor sets the next parameters of the class
        /// </summary>
        /// <param name="data">The data of the tree</param>
        /// <param name="dataType">The type of data (Nothing, Number, Standard function, Operator)</param>
        /// <param name="leftOperand">The operand of LEFT subtree</param>
        /// <param name="rightOperand">The operand of RIGHT subtree</param>
        /// <param name="stringLeft">The string which is supposed to be sent to LEFT subtree as 'Data' parameter</param>
        /// <param name="stringRight">The the string which is supposed to be sent to RIGHT subtree as 'Data' parameter</param>
        public Tree(string data, EssenceType dataType, double leftOperand, double rightOperand, string stringLeft, string stringRight)
        {
            this.Data = data;
            this.DataType = dataType;
            this.LeftOperand = leftOperand;
            this.RightOperand = rightOperand;
            this.StringLeft = stringLeft;
            this.StringRight = stringRight;
        }

        #region Class properties

        /// <summary>
        /// Gets or sets the data of the tree
        /// </summary>
        public string Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of data (Nothing, Number, Standard function, Operator)
        /// </summary>
        public EssenceType DataType
        {
            get
            {
                return _dataType;
            }
            set
            {
                _dataType = value;
            }
        }

        /// <summary>
        /// Gets or sets the operand of LEFT subtree
        /// </summary>
        public double LeftOperand
        {
            get
            {
                return _leftOperand;
            }
            set
            {
                _leftOperand = value;
            }
        }

        /// <summary>
        /// Gets or sets the operand of RIGHT subtree
        /// </summary>
        public double RightOperand
        {
            get
            {
                return _rightOperand;
            }
            set
            {
                _rightOperand = value;
            }
        }

        /// <summary>
        /// Get or sets the string which is supposed to be sent to LEFT subtree as 'Data' parameter 
        /// </summary>
        public string StringLeft
        {
            get
            {
                return _stringLeft;
            }
            set
            {
                _stringLeft = value;
            }
        }

        /// <summary>
        /// Gets or sets the the string which is supposed to be sent to RIGHT subtree as 'Data' parameter 
        /// </summary>
        public string StringRight
        {
            get
            {
                return _stringRight;
            }
            set
            {
                _stringRight = value;
            }
        }

        #endregion
    }
}
