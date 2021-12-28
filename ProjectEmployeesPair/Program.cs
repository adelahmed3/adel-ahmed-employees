using System;
using ProjectEmployeesPair.Service;

namespace ProjectEmployeesPair
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationService applicationService = new ApplicationService();

            var projectEmpData = applicationService.GetProjectEmployeeData();
            if (projectEmpData != null && projectEmpData.Count > 0)
            {
                var projectPairedEmployees = applicationService.GetProjectPairedEmployees(projectEmpData);
                Console.WriteLine("Pair of employees that have worked as a team for the longest time");
                Console.WriteLine("-----------------------------------------------------------------");
                Console.WriteLine();

                foreach (var data in projectPairedEmployees)
                {
                    if (string.IsNullOrEmpty(data.Description))
                        Console.WriteLine($"Employee ID #1: {data.FirstEmpId}, Employee ID #2: {data.SecondEmpId}, Project ID: {data.ProjectId}, Days worked: {data.WorkingDays}");
                    else
                        Console.WriteLine($"Employee ID #1: {data.FirstEmpId}, Employee ID #2: {data.SecondEmpId}, Project ID: {data.ProjectId}, Days worked: {data.WorkingDays}, Description: {data.Description}");
                }

                Console.WriteLine();
                Console.WriteLine("Please press enter to exit");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Thier isn't any valid data");
            }
        }
    }
}
