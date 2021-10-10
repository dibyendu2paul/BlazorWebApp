using BlazorWebApp.Server.Models;
using BlazorWebApp.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebApp.Server.Services
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext appDbContext;

        public EmployeeRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<Employee> AddEmployee(Employee employee)
        {
            if (employee.Department != null)
            {
                appDbContext.Entry(employee.Department).State = EntityState.Unchanged;
            }
            var result = await appDbContext.Employees.AddAsync(employee);
            await appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteEmployee(int employeeId)
        {
            var result = await appDbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
            if (result != null)
            {
                appDbContext.Employees.Remove(result);
                await appDbContext.SaveChangesAsync();
            }
        }

        public async Task<Employee> GetEmployee(int employeeId)
        {
            List<string> words = new List<string> { "Apple", "Boy", "April" };
            List<string> UpdatedWord = RemoveWord(words);
            int[] A = {-1,-3};
            int missingNumber = MissingNumber(A);
            int[] arr = { 2, 0, 2, 1, 1, 0};
            int[] sortCol = SortColors(arr); // Array sort
            return await appDbContext.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
        }
        public int[] SortColors(int[] nums)
        {
            int[] arr = new int[nums.Length]; ;
            for (int i = 0; i < nums.Length; i++)
            {
                int temp = 0;
                
                for (int j = i+1; j < nums.Length; j++)
                {
                    if (nums[i] > nums[j])
                    {
                        temp = nums[i];
                        nums[i] = nums[j];
                        nums[j] = temp;
                    }
                }
            }
            return nums;

        }
        private int MissingNumber(int[] a)
        {
            int missingNumber = 1;
            int min = a.Min();
            int max = a.Max();
            
            for (int i = min; i <= max; i++)
            {
                bool presentInTheArr = false;
                for (int j = 0; j <= a.Length - 1; j++)
                {
                    if (i == a[j])
                    {
                        presentInTheArr = true;
                        break;
                    }
                }
                if (!presentInTheArr && i > 0)
                    return i;
            }
            return (missingNumber > 0 && max >0)?max+1:missingNumber;

        }

        private List<string> RemoveWord(List<string> words)
        {
            List<string> word1 = (words.Where(e => !e.ToUpper().StartsWith("A")).Select(e => e.ToUpper())).ToList();
            List<string> updatedWords = new List<string>();
            foreach (string word in words)
            {
                if (!word.ToUpper().StartsWith("A"))
                    updatedWords.Add(word);
            }
            return updatedWords;
        }

        public async Task<Employee> GetEmployeeByEmail(string mail)
        {
            return await appDbContext.Employees
                .FirstOrDefaultAsync(e => e.Email == mail);
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await appDbContext.Employees.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> SearchEmployee(string name, Gender? gender)
        {
            IQueryable<Employee> query = appDbContext.Employees;
            if (!string.IsNullOrEmpty(name)) query = query.Where(e => e.FirstName.Contains(name) || e.LastName.Contains(name));
            if (gender != null)
                query = query.Where(e => e.Gender == gender);
            return await query.ToListAsync();
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            var result = await appDbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);
            if (result != null)
            {
                result.FirstName = employee.FirstName;
                result.LastName = employee.LastName;
                result.Email = employee.Email;
                result.DateOfBirth = employee.DateOfBirth;
                result.Gender = employee.Gender;
                result.DepartmentId = employee.DepartmentId;
                result.PhotoPath = employee.PhotoPath;
                await appDbContext.SaveChangesAsync();
                return result;
            }
            return result;

        }
    }
}
