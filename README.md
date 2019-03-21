# EmployeePayrollProgram

Console application that generates payslips for an HR manager.

Running the program: The Console prompts for year and month.

It then prompts for the amount of hours each employee worked in the given month.

The list of staff is contained in the staff.txt file.
The initial text file staff.txt is in bin --> debug --> netcoreapp2.1.

Example Staff.txt

Yvonne, Manager
Peter, Manager
John, Admin
Carol, Admin

The console displays a payslip and also creates a text file of the paylslip for each employee. 
The text file follow the format of 'employeeName' + .txt and is in the same directory as staff.txt.

Lastly, the program generates a report of employees who worked less than 10 hours in the last month called it summary.txt.

To run the program again, just delete all text files except staff.txt.
