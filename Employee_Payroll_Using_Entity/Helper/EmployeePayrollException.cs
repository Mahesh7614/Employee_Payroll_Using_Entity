using System;

namespace Employee_Payroll_Using_Entity.Helper
{
    public class EmployeePayrollException : Exception
    {
        public EmployeePayrollException() : base()
        { }
        public EmployeePayrollException(string message) : base(message) { }
    }
}
