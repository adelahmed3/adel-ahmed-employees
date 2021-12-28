using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectEmployeesPair.Common;
using ProjectEmployeesPair.DataModel;

namespace ProjectEmployeesPair.Service
{
    public class ApplicationService
    {
        #region Properties
        private static readonly string filePath = Helper.GetConfig("config")["filePath"];
        #endregion

        #region Public Methods
        public Dictionary<int, List<Employee>> GetProjectEmployeeData()
        {
            try
            {
                Dictionary<int, List<Employee>> projectEmpData = new Dictionary<int, List<Employee>>();

                if (!File.Exists(filePath))
                {
                    Console.WriteLine("File is not exist, Please check the file path in config file");
                    Environment.Exit(1);
                }

                if (Path.GetExtension(filePath).Equals("txt", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("File extension is not txt, Please enter a valid file");
                    Environment.Exit(1);
                }

                IEnumerable<string> lines = File.ReadLines(filePath);
                int counter = 0;

                foreach (var singleLine in lines)
                {
                    string[] data = singleLine.Split(',');
                    int projectId = 0;
                    int empId = 0;
                    bool isValidLine = true;

                    if (counter > 0)
                    {
                        if (string.IsNullOrEmpty(data[0]) || !int.TryParse(data[0], out empId))
                        {
                            Console.WriteLine($"Error in Line {counter}, EmpID is not an intger");
                            isValidLine = false;
                        }

                        if (string.IsNullOrEmpty(data[1]) || !int.TryParse(data[1], out projectId))
                        {
                            Console.WriteLine($"Error in Line {counter}, ProjectID is not an intger");
                            isValidLine = false;
                        }

                        var dates = ValidateDate(data[2], data[3], counter);
                        if (!dates.Item1)
                        {
                            Console.WriteLine($"Error in Line {counter}, Dates is invalid");
                            isValidLine = false;
                        }

                        if (isValidLine)
                        {
                            if (!projectEmpData.ContainsKey(projectId))
                            {
                                projectEmpData.Add(projectId, new List<Employee>() { new Employee(empId, dates.Item2, dates.Item3) });
                            }
                            else if (!projectEmpData[projectId].Any(e => e.Id == empId))
                            {
                                projectEmpData[projectId].Add(new Employee(empId, dates.Item2, dates.Item3));
                            }
                            else
                            {
                                Console.WriteLine($"Error in Line {counter} You already assign that emp value to this project before");
                                counter++;
                                continue;
                            }
                        }
                        else
                        {
                            counter++;
                            continue;
                        }
                    }
                    else
                    {
                        if (data.Count() != 4 || !data[0].Equals("EmpID", StringComparison.OrdinalIgnoreCase) 
                            || !data[1].Equals("ProjectID", StringComparison.OrdinalIgnoreCase) 
                            || !data[2].Equals("DateFrom", StringComparison.OrdinalIgnoreCase) 
                            || !data[3].Equals("DateTo", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("Please enter the vaild column order [EmpID,ProjectID,DateFrom,DateTo]");
                            Environment.Exit(1);
                        }

                        counter++;
                    }
                }

                return projectEmpData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public List<ProjectPairedEmployees> GetProjectPairedEmployees(Dictionary<int, List<Employee>> projectEmpData)
        {
            try
            {
                List<ProjectPairedEmployees> projectPairedEmployees = new List<ProjectPairedEmployees>();

                foreach (var projectEmp in projectEmpData)
                {
                    var employees = projectEmp.Value;
                    var pairedEmpData = new ProjectPairedEmployees();
                    pairedEmpData.ProjectId = projectEmp.Key;

                    for (int i = 0; i < employees.Count; i++)
                    {
                        for (int j = i + 1; j < employees.Count; j++)
                        {
                            if (employees[i].DateFrom < employees[j].DateTo && employees[j].DateFrom < employees[i].DateTo)
                            {
                                var startDate = employees[i].DateFrom > employees[j].DateFrom ? employees[i].DateFrom : employees[j].DateFrom;
                                var endDate = employees[i].DateTo > employees[j].DateTo ? employees[j].DateTo : employees[i].DateTo;

                                var workingDays = (endDate - startDate).Days;

                                if (workingDays > pairedEmpData.WorkingDays)
                                {
                                    pairedEmpData.FirstEmpId = employees[i].Id;
                                    pairedEmpData.SecondEmpId = employees[j].Id;
                                    pairedEmpData.WorkingDays = workingDays;
                                }
                            }
                        }
                    }

                    if (pairedEmpData.FirstEmpId == 0 && pairedEmpData.SecondEmpId == 0)
                    {
                        pairedEmpData.Description = "No employees worked together for this project";
                    }

                    projectPairedEmployees.Add(pairedEmpData);
                }

                return projectPairedEmployees;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        #endregion

        #region Private Methods
        private Tuple<bool, DateTime, DateTime> ValidateDate(string dateFrom, string dateTo, int counter)
        {
            bool isValid = true;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();

            if (string.IsNullOrEmpty(dateFrom) || !DateTime.TryParse(dateFrom, out startDate))
            {
                Console.WriteLine($"Error in Line {counter}, DateFrom is invalid");
                isValid = false;
            }

            if (string.IsNullOrEmpty(dateTo) || dateTo.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                endDate = DateTime.Now;
            }
            else if(!DateTime.TryParse(dateTo, out endDate))
            {
                Console.WriteLine($"Error in Line {counter}, DateTo is invalid");
                isValid = false;
            }

            if (isValid && startDate > endDate)
            {
                Console.WriteLine($"Error in Line {counter}, start date is bigger than end date");
                isValid = false;
            }

            return Tuple.Create(isValid, startDate, endDate);
        }
        #endregion
    }
}
